using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace NUnit.Given
{
    public class GivenTestFixtureAttribute : TestFixtureAttribute, ITestAction
    {
        public GivenTestFixtureAttribute(Type contextType) : this(contextType, null)
        {
        }

        public GivenTestFixtureAttribute(Type contextType, params object[] arguments) : base(arguments)
        {
            ContextType = contextType;
            Properties.Set(GivenTestFixtureAttributeHash, GetAttributeHash());
        }

        public Type ContextType { get; }
        private static readonly string GivenTestFixtureAttributeHash = nameof(GivenTestFixtureAttributeHash);

        public void BeforeTest(ITest test)
        {
            var fixtureHash = test.Properties.Get(GivenTestFixtureAttributeHash);
            if (fixtureHash != null && GetAttributeHash().Equals(fixtureHash))
            {
                var testContext = AbstractGivenTestContext.From(ContextType, null);
                test.Properties.Set(ContextualTest.ContextKey, testContext);
                InjectTestContext(test, testContext);
            }
        }

        private void InjectTestContext(ITest test, AbstractGivenTestContext testContext)
        {
            var fixture = test.Fixture as IHasContext<GivenTestContext>;
            if (fixture == null)
                throw new ArgumentException($"Fixture {test.Fixture.GetType()} must implement IHasContext<{ContextType.Name}>");

            var contextSetter = fixture.GetType().GetProperty(nameof(fixture.Context),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            if (contextSetter == null || !contextSetter.CanWrite)
                throw new ArgumentException($"Fixture {test.Fixture.GetType().FullName} must have a setter for its Context.");

            contextSetter.SetValue(fixture, testContext);
        }

        public void AfterTest(ITest test)
        {
            //Do nothing
        }

        public virtual ActionTargets Targets => ActionTargets.Default;

        private int GetAttributeHash()
        {
            return ContextType.GetHashCode() + GetHashCode(Arguments);
        }

        private static int GetHashCode(object[] array)
        {
            if (array == null) return 0;
            unchecked
            {
                return array.Aggregate(17, (current, item) => current * 23 + (item != null ? item.GetHashCode() : 0));
            }
        }
    }
}