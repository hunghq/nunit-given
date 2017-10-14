using System;

namespace NUnit.Given
{
    public class ContextWithError
    {
        public Type ContextType { get; }
        public object[] Arguments { get; }
        public Exception Exception { get; }

        public ContextWithError(Type contextType, object[] arguments, Exception exception)
        {
            ContextType = contextType;
            Arguments = arguments;
            Exception = exception;
        }
    }
}