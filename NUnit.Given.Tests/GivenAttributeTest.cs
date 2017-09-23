using NUnit.Framework;
using NUnit.Given.Tests.Given;

namespace NUnit.Given.Tests
{
    public class GivenAttributeTest : ContextualTest<GivenObject>
    {
        private static int _testCount;
        
        [Given(typeof(GivenObject))]
        public void DefaultConstructor_ShouldBeInitialized()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("Default"));
        }

        [Given(typeof(GivenExtendedObject))]
        public void ExtendedContext_ShouldBeInitialized()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("one"));
        }

        [Given(typeof(GivenObject.WithOneParameter))]
        public void WithOneParameter_ShouldAcceptArgument()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("one"));
        }

        [Given(typeof(GivenObject.WithTwoParameters))]
        public void WithTwoParameters_ShouldCreateTwoTestCases()
        {
            _testCount++;
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount)
            {
                case 1:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_ShouldCreateTwoTestCases) + " [one]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
                case 2:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_ShouldCreateTwoTestCases) + " [two]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    break;
            }
        }

        [OneTimeTearDown]
        public void CheckTotalTestCounts()
        {
            Assert.That(_testCount, Is.EqualTo(2));
        }
    }
}