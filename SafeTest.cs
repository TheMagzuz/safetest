using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace SafeTest
{
    public static class SafeTest
    {

        private static Logger logger = new Logger("SafeTest");
        private static List<Test> testMethods = new List<Test>();

        public static void RunTests(Assembly projectAssembly)
        {
            FindTests();
            CheckCoverage(projectAssembly);
            ExecuteTests();
            Console.ReadKey();
        }

        private static void CheckCoverage(Assembly projectAssembly)
        {
            logger.Info("Checking coverage");
            Type[] types = projectAssembly.GetTypes();
            bool failure = false;

            ConsoleProgressCounter progress = new ConsoleProgressCounter("[SafeTest/INFO]: Checking coverage for types: {0}/{1} {2}", types.Length, 20);
            progress.Draw();

            foreach (Type t in types)
            {
                if (!testMethods.Any(tm => tm.classesTested.Contains(t)))
                {
                    logger.Warning($"Type {t.FullName} is not covered by any tests");
                    failure = true;
                }
                progress.Increment();
            }
            if (failure)
            {
                logger.Warning("Some coverage checks failed. Check the logs for further details");
            }
            else
            {
                logger.Succes("Coverage checks finished with no errors");
            }
        }

        private static void FindTests()
        {
            logger.Info("Finding tests...");

            ConsoleCounter testsFoundCounter = new ConsoleCounter("[SafeTest/INFO] Tests found: {0}");
            testsFoundCounter.Draw();

            // This will get all classes in the assembly of the function calling this function, which have the TestClassAttribute
            foreach (Type t in Reflection.GetTypesWithAttribute(Assembly.GetEntryAssembly(), typeof(TestClassAttribute)))
            {
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
                        logger.Warning($"Method {fullMethodName} is non-static. It will not be called");
                    }
                    else
                    {
                        CoversAttribute[] attributes = (CoversAttribute[])m.GetCustomAttributes(typeof(CoversAttribute), false);

                        if (attributes.Length > 1)
                        {
                            logger.Warning($"The test {fullMethodName} covers more than one type. This is generally considered bad practice");
                        }

                        List<Type> coveredMethods = new List<Type>();

                        foreach (CoversAttribute a in attributes)
                        {
                            coveredMethods.Add(a.typeCovered);
                        }

                        testMethods.Add(new Test(m, coveredMethods));
                        testsFoundCounter.Increment();
                    }
                }
            }
            logger.Info("Done finding tests");
        }

        private static void ExecuteTests()
        {
            logger.Info("Running tests");
            ConsoleProgressCounter progressCounter = new ConsoleProgressCounter("[SafeTest/INFO] Tests done: {0}/{1} {2}", testMethods.Count, 35);
            int testsDone = 0;
            List<FailedTest> failedTests = new List<FailedTest>();
            progressCounter.Draw();

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
                    progressCounter.Increment();
                }

            }
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
                    }
                    else
                    {
                        logger.Error($"{t.methodName} threw an unexpected exception: {t.exception}");
                    }
                }
            }
        }

    }
}
