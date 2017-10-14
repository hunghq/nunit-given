using System;
using System.Linq;

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
        
        public override string ToString()
        {
            var arguments = Arguments == null ? "" : "arguments = " + string.Join(",", Arguments?.Select(x => x.ToString()));
            return "The test cannot be run because there was an error when setting up its test context " 
                + $"{ContextType.FullName}({arguments})."
                + Environment.NewLine
                + Exception;
        }
    }
}