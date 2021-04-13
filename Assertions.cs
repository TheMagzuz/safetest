using System;

namespace SafeTest
{
    public static class Assertions
    {
        private static Logger logger = new Logger("SafeTest");

        public static void Assert(bool assertion, string message)
        {
            if (!assertion)
            {
                throw new AssertionException(message);
            }
        }
    }

    public class AssertionException : Exception
    {
       public AssertionException(string message) : base(message) {} 
    }

}
