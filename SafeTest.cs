using System;
using System.Reflection;
using System.Collections.Generic;

namespace SafeTest
{
    public static class SafeTest
    {

        private static Logger logger = new Logger("SafeTest");
        private static List<MethodInfo> testMethods = new List<MethodInfo>();

        public static void RunTests(object sender)
        {
            logger.Info("Finding tests...");
            Console.Write("Tests found: ");
            foreach (Type t in Reflection.GetTypesWithAttribute(sender.GetType().Assembly, typeof(TestClassAttribute)))
            {
                foreach (MethodInfo m in t.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Static))
                {
                    if (!m.IsStatic)
                    {
                        var (left, top) = Console.GetCursorPosition();
                        logger.Warning($"Method {t.Name}.{m.Name} is non-static. It will not be called");
                        Console.SetCursorPosition(left, top);
                        continue;
                    }
                    var (left, top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(left-(testMethods.Count.ToString().Length), top);
                    testMethods.Add(m);
                    Console.Write(testMethods.Count.ToString());
                }
            }
        }

    }
}
