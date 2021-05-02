using System;
using System.Reflection;
using System.Collections.Generic;

namespace SafeTest
{
    class Test
    {
        public readonly MethodInfo testMethod;
        public readonly List<Type> classesTested;

        public string methodName => testMethod.DeclaringType.Name + "." + testMethod.Name;

        public Test(MethodInfo testMethod, List<Type> classesTested)
        {
            this.testMethod = testMethod;
            this.classesTested = classesTested;
        }
    }
}
