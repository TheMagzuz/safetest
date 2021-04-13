using System;

namespace SafeTest
{
    // Marks a class as a collection of tests
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute
    {

    }
}
