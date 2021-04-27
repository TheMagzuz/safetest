using System;

namespace SafeTest
{
    // Marks a class as a collection of tests
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CoversAttribute : Attribute
    {
        public readonly Action methodCovered;

        public CoversAttribute(Action methodCovered)
        {
            this.methodCovered = methodCovered;
        }
    }

}
