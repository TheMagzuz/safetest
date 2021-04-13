using System;
using System.Reflection;
using System.Collections.Generic;

namespace SafeTest
{
    public static class SafeTest
    {

        private static Logger logger = new Logger("SafeTest");
        private static List<MethodInfo> testMethods = new List<MethodInfo>();

        public static void RunTests()
        {
            logger.Debug(Assembly.GetCallingAssembly().Location);
            logger.Info("Finding tests...");
            Console.Write("Tests found:  ");

            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            int writeLine = Console.CursorTop + 1;
            foreach (Type t in Reflection.GetTypesWithAttribute(Assembly.GetCallingAssembly(), typeof(TestClassAttribute)))
            {
                Console.SetCursorPosition(0, writeLine);
                writeLine++;
                Console.WriteLine("Found test class: " + t.Name);
                foreach (MethodInfo m in t.GetMethods())
                {
                    if (!m.IsStatic)
                    {
                        Console.SetCursorPosition(0, writeLine);
                        logger.Warning($"Method {t.Name}.{m.Name} is non-static. It will not be called");
                        writeLine++;
                        Console.SetCursorPosition(left, top);
                    }
                    else
                    {
                        Console.SetCursorPosition(left - (testMethods.Count.ToString().Length), top);
                        testMethods.Add(m);
                        Console.Write(testMethods.Count.ToString());
                    }
                }
            }
            Console.ReadKey();
        }

    }
}
