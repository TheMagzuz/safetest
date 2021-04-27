using System;
using System.Threading;
using SafeTest;

namespace exampletests
{
    [TestClass]
    static class Tests
    {
        public static void Test()
        {
            Assertions.Assert(true, "True is equal to true");
        }

        public static void LongTest()
        {
            Thread.Sleep(500);
            Assertions.Assert(false, "This should fail!");
        }

    }
}
