using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public abstract class ContextualTest<T> : ContextualTest where T : class
    {
        protected T Context => GetContext();

        private static T GetContext()
        {
            var context = TestContext.CurrentContext.Test.Properties.Get(ContextKey);
            Assert.NotNull(context, $"Test Context has not been set! Expected: {typeof(T).FullName}");

            var typed = context as T;
            Assert.NotNull(typed,
                $"Test Context is not valid. Expected: {typeof(T).FullName}. Got: {context.GetType().FullName}");

            return typed;
        }
    }

    public abstract class ContextualTest
    {
        public const string ContextKey = "test_context";
        public const string ContextParametersKey = "test_context_parameters";

        public static object From(Type contextType, object[] arguments)
        {
            try
            {
                Validate(contextType);
                return arguments != null && arguments.Any()
                    ? Reflect.Construct(contextType, arguments)
                    : Reflect.Construct(contextType);
            }
            catch (Exception e)
            {
                return new ContextWithError(contextType, arguments, e.InnerException ?? e);
            }
        }

        public static void Validate(Type contextType)
        {
            if (!contextType.IsClass || contextType.IsAbstract)
                throw new ArgumentException($"Context Type {contextType.Name} must be a non-abstract class.");
        }
    }
}