using System;
using System.Reflection;
using System.Collections.Generic;

namespace SafeTest
{
    public static class SafeTest
    {

        private static Logger logger = new Logger("SafeTest");
        private static List<Test> testMethods = new List<Test>();

        public static void RunTests(Assembly projectAssembly)
        {
            FindTests();
            ExecuteTests();
            Console.ReadKey();
        }

        private static void FindTests()
        {
            logger.Info("Finding tests...");
            Console.Write("[SafeTest/INFO] Tests found:  ");

            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            int writeLine = Console.CursorTop + 1;
            Console.Write("0");

            // This will get all classes in the assembly of the function calling this function, which have the TestClassAttribute
            foreach (Type t in Reflection.GetTypesWithAttribute(Assembly.GetEntryAssembly(), typeof(TestClassAttribute)))
            {
                Console.SetCursorPosition(0, writeLine);
                writeLine++;
                Console.WriteLine("Found test class: " + t.Name);

                // Loop through all the methods in the class
                foreach (MethodInfo m in t.GetMethods())
                {
                    string fullMethodName = t.Name + "." + m.Name;
                    // Ignore the method if it's a built in method, for example ToString
                    if (Reflection.IsBuiltin(m))
                    {
                        continue;
                    }

                    // If the method is non-static, show a warning and ignore the method
                    if (!m.IsStatic)
                    {
                        Console.SetCursorPosition(0, writeLine);
                        logger.Warning($"Method {fullMethodName} is non-static. It will not be called");
                        writeLine++;
                        Console.SetCursorPosition(left, top);
                    }
                    else
                    {
                        Console.SetCursorPosition(left - testMethods.Count.ToString().Length, top);

                        CoversAttribute[] attributes = (CoversAttribute[]) m.GetCustomAttributes(typeof(CoversAttribute), false);

                        if (attributes.Length > 1)
                        {
                            Console.SetCursorPosition(0, writeLine);
                            logger.Warning($"The test {fullMethodName} covers more than one method. This is generally considered bad practice");
                            writeLine++;
                            Console.SetCursorPosition(left, top);
                        }

                        List<MethodInfo> coveredMethods = new List<MethodInfo>();

                        foreach (CoversAttribute a in attributes)
                        {
                            coveredMethods.Add(a.methodCovered.Method);
                        }

                        testMethods.Add(new Test(m, coveredMethods));
                        Console.Write(testMethods.Count.ToString());
                    }
                }
            }
            Console.SetCursorPosition(0, writeLine);
            logger.Info("Done finding tests");
        }

        private static void ExecuteTests()
        {
            logger.Info("Running tests");
            Console.Write("[SafeTest/INFO] Tests done: ");
            int writeLeft = Console.CursorLeft;
            int writeTop = Console.CursorTop;
            int writeLine = writeTop + 1;
            int testsDone = 0;
            List<FailedTest> failedTests = new List<FailedTest>();
            Console.Write(testsDone + "/" + testMethods.Count + $" ({failedTests.Count} failed)");


            foreach (Test test in testMethods)
            {
                MethodInfo m = test.testMethod;
                try
                {
                    m.Invoke(null, null);
                }
                catch (TargetInvocationException e)
                {
                    failedTests.Add(new FailedTest(m.ReflectedType.Name + "." + m.Name, e.InnerException));
                }
                finally
                {
                    testsDone++;
                    Console.SetCursorPosition(writeLeft, writeTop);
                    Console.Write(testsDone + "/" + testMethods.Count + $" ({failedTests.Count} failed)");
                }

            }
            Console.SetCursorPosition(0, writeLine);
            logger.Info("Done running tests");
            if (failedTests.Count == 0)
            {
                logger.Succes("All tests passed!");
            }
            else
            {
                logger.Error($"{failedTests.Count} tests failed!");
                foreach (FailedTest t in failedTests)
                {
                    if (t.exception is AssertionException)
                    {
                        logger.Error($"{t.methodName} failed: {t.exception.Message}");
                    } else
                    {
                        logger.Error($"{t.methodName} threw an unexpected exception: {t.exception}");
                    }
                }
            }
        }

    }
}
