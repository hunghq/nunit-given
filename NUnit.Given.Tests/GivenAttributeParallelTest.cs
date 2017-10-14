using NUnit.Framework;
using NUnit.Given.Tests.Given;

namespace NUnit.Given.Tests
{
    [Parallelizable(ParallelScope.All)]
    public class GivenAttributeParallelTest : ContextualTest<GivenObject>
    {
        private static int? _testCount1;

        [Test]
        [Given(typeof(GivenObject))]
        public void DefaultConstructor_ShouldBeInitialized()
        {
            TestUtil.IncrementTestCount(ref _testCount1);
            Assert.That(Context.MutableValue, Is.Null);
            Context.MutableValue = nameof(DefaultConstructor_ShouldBeInitialized);
        }

        [Given(typeof(GivenObject.WithTwoParameters))]
        public void WithTwoParameters_ShouldCreateTwoTestCases()
        {
            TestUtil.IncrementTestCount(ref _testCount1);
            Assert.That(Context.MutableValue, Is.Null);
            Context.MutableValue = nameof(WithTwoParameters_ShouldCreateTwoTestCases) + _testCount1;
        }

        [OneTimeTearDown]
        public void CheckTotalTestCounts()
        {
            TestUtil.AssertTestCount(_testCount1, 3);
        }
    }
}
