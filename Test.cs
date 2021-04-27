using System.Reflection;
using System.Collections.Generic;

namespace SafeTest
{
    class Test
    {
        public readonly MethodInfo testMethod;
        public readonly List<MethodInfo> methodsTested;

        public string methodName => testMethod.DeclaringType.Name + "." + testMethod.Name;

        public Test(MethodInfo testMethod, List<MethodInfo> methodsTested)
        {
            this.testMethod = testMethod;
            this.methodsTested = methodsTested;
        }
    }
}
