using System;

namespace NUnit.Given
{
    public class ErrorTestContext : AbstractGivenTestContext
    {
        public Type ContextType { get; }
        public object[] Arguments { get; }
        public Exception Exception { get; }

        public ErrorTestContext(Type contextType, object[] arguments, Exception exception)
        {
            ContextType = contextType;
            Arguments = arguments;
            Exception = exception;
        }
    }
}