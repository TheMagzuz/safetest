using System;
using System.Reflection;
using SafeTest;

namespace exampletest
{
    class Program
    {
        static void Main(string[] args)
        {
            SafeTest.SafeTest.RunTests(typeof(exampleproject.Program).Assembly);
        }
    }
}
