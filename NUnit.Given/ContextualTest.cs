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
            if (context == null)
            {
                Assert.Fail($"Test Context has not been set! Expected: {typeof(T).FullName}");
            }

            var errorContext = context as ContextWithError;
            if (errorContext != null)
            {
                Assert.Fail(errorContext.ToString());
            }

            var typed = context as T;
            if (typed == null)
            {
                Assert.Fail($"Test Context is not valid. Expected: {typeof(T).FullName}. Got: {context.GetType().FullName}");
            }

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

        protected static void Set<TType>(TType obj)
        {
            Set(typeof(TType).FullName, obj);
        }

        protected static void Set<TType>(string key, TType obj)
        {
            TestContext.CurrentContext.Test.Properties.Set(key, obj);
        }

        protected static TType Get<TType>()
        {
            return Get<TType>(typeof(TType).FullName);
        }

        protected static TType Get<TType>(string key)
        {
            return (TType)TestContext.CurrentContext.Test.Properties.Get(key);
        }
    }
}