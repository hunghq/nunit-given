using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public abstract class ContextualTest<T> : ContextualTest where T : GivenTestContext
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

        public static IEnumerable<T> GetAll<T>() where T : GivenTestContext
        {
            var givenContext = (T) Reflect.Construct(typeof(T), null);
            return givenContext.Parameterize() as IEnumerable<T>;
        }

        public static T GetDefault<T>() where T : GivenTestContext
        {
            return Reflect.Construct(typeof(T), null) as T;
        }
    }
}