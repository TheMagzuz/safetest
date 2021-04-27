using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeTest
{
    class FailedTest
    {

        public readonly string methodName;
        public readonly Exception exception;
        
        public FailedTest(string methodName, Exception exception)
        {
            this.methodName = methodName;
            this.exception = exception;
        }

    }
}
