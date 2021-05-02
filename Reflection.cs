using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SafeTest {
    static class Reflection
    {

        private static string[] builtinMethods = { "ToString", "GetHashCode", "GetType", "Equals" };

        public static IEnumerable<Type> GetTypesWithAttribute(Assembly assembly, Type attribute)
        {
            // Loop through every type in the assembly
            foreach (Type t in assembly.GetTypes())
            {
                // Check if the type has at least one of the given attribute
                if (t.GetCustomAttributes(attribute, false).Length > 0)
                {
                    // If it does, add that type to the returned enumerable
                    yield return t;
                }
            }
        }

        public static bool IsBuiltin(MethodInfo m)
        {
            return builtinMethods.Contains(m.Name);
        }

    }
}
