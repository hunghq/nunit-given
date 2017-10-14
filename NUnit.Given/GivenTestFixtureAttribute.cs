using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public class GivenTestFixtureAttribute : TestFixtureAttribute, ITestAction
    {
        public GivenTestFixtureAttribute(Type contextType) : this(contextType, null)
        {
        }

        public GivenTestFixtureAttribute(Type contextType, params object[] arguments) : base(arguments)
        {
            ContextualTest.Validate(contextType);
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
                var fixture = test.Fixture as IHasContext<object>;
                if (fixture == null)
                    throw new ArgumentException($"Fixture {test.Fixture.GetType()} must implement IHasContext<{ContextType.Name}>");

                var contextSetter = fixture.GetType().GetProperty(nameof(fixture.Context),
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

                if (contextSetter == null || !contextSetter.CanWrite)
                    throw new ArgumentException($"Fixture {test.Fixture.GetType().FullName} must have a setter for its Context.");

                InjectTestContext(test, contextSetter);
            }
        }

        private void InjectTestContext(ITest test, PropertyInfo contextSetter)
        {
            var testContext = ContextualTest.From(ContextType, null);
            test.Properties.Set(ContextualTest.ContextKey, testContext);

            if (HandleErrorTestContext(test, testContext)) return;

            contextSetter.SetValue(test.Fixture, testContext);
        }

        private static bool HandleErrorTestContext(ITest test, object testContext)
        {
            var errorContext = testContext as ContextWithError;
            if (errorContext == null) return false;
            
            IgnoreTest((Test)test, errorContext);
            foreach (var testCase in test.Tests)
            {
                IgnoreTest((Test)testCase, errorContext);
            }
            return true;
        }

        private static void IgnoreTest(Test test, ContextWithError errorContext)
        {
            var ignoreAttribute = new IgnoreAttribute(
                $"The test is ignored because there was an error when setting up its test context {errorContext.ContextType.FullName}."
                + Environment.NewLine
                + errorContext.Exception);

            ignoreAttribute.ApplyToTest(test);
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