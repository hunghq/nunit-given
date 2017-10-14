using NUnit.Framework;
using NUnit.Given.Tests.Given;
using NUnit.Given.Tests.Given.Extended;

namespace NUnit.Given.Tests
{
    public class GivenAttributeMultiArgumentTest : ContextualTest<GivenExtendedObject>
    {
        private static int _testCount;

        [Given(typeof(GivenObject))]
        public void DifferentContext_ShouldBeInvalid()
        {
            AssertHelper.ExpectException<AssertionException>(
                () => {var context = Context;}, 
                $"Test Context is not valid. Expected: {typeof(GivenExtendedObject).FullName}. Got: {typeof(GivenObject).FullName}");
        }

        [Given(typeof(GivenExtendedObject))]
        public void DefaultConstructor_ShouldBeInitialized()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("one"));
            Assert.That(Context.MoreValue, Is.EqualTo("one more"));
        }

        [Given(typeof(GivenExtendedObject.WithTwoParameters))]
        public void WithTwoParameters_ShouldCreateTwoTestCases()
        {
            _testCount++;
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount)
            {
                case 1:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_ShouldCreateTwoTestCases) + "() [one,one more]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    Assert.That(Context.MoreValue, Is.EqualTo("one more"));
                    break;
                case 2:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_ShouldCreateTwoTestCases) + "() [two,two more]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    Assert.That(Context.MoreValue, Is.EqualTo("two more"));
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