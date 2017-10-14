using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public abstract class ContextualTest<T> : ContextualTest where T : class, IGiven, new()
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

        public static IGiven From(Type type, object[] arguments)
        {
            if (!typeof(IGiven).IsAssignableFrom(type))
                throw new ArgumentException($"ContextType {type.Name} must implement IGiven.");

            try
            {
                var context = arguments != null && arguments.Any()
                    ? Reflect.Construct(type, arguments)
                    : Reflect.Construct(type);

                var given = (IGiven)context;
                return given;
            }
            catch (Exception e)
            {
                return new ErrorTestContext(type, arguments, e.InnerException ?? e);
            }
        }
    }
}