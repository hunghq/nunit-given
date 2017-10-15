using NUnit.Framework;
using NUnit.Given.Tests.Given;
using NUnit.Given.Tests.Subject;

namespace NUnit.Given.Tests
{
    [Parallelizable(ParallelScope.All)]
    public class GivenAttributeParallelTest : ContextualTest<GivenObject>
    {
        private static int? _testCount;
        protected TestSubject TestSubject => Get<TestSubject>();

        [SetUp]
        public void SetUp()
        {
            Set(new TestSubject());
            Set("secondary", new TestSubject("default value"));
        }

        [Test]
        [Given(typeof(GivenObject))]
        public void ContextObject_ShouldBeIndependentBetweenTests()
        {
            TestUtil.IncrementTestCount(ref _testCount);
            Assert.That(Context.MutableValue, Is.Null);
            Context.MutableValue = nameof(ContextObject_ShouldBeIndependentBetweenTests);
        }

        [Given(typeof(GivenObject.WithTwoParameters))]
        public void ContextObject_ShouldBeIndependentBetweenTestsAgain()
        {
            TestUtil.IncrementTestCount(ref _testCount);
            Assert.That(Context.MutableValue, Is.Null);
            Context.MutableValue = nameof(ContextObject_ShouldBeIndependentBetweenTestsAgain) + _testCount;
        }

        [Test]
        public void TestSubjects_ShouldBeIndependentBetweenTests()
        {
            GetAndChangeTestSubjects(nameof(TestSubjects_ShouldBeIndependentBetweenTests));
        }

        [Test]
        public void TestSubjects_ShouldBeIndependentBetweenTestsAgain()
        {
            GetAndChangeTestSubjects(nameof(TestSubjects_ShouldBeIndependentBetweenTestsAgain));
        }

        [OneTimeTearDown]
        public void CheckTotalTestCounts()
        {
            TestUtil.AssertTestCount(_testCount, 3);
        }

        private void GetAndChangeTestSubjects(string testName)
        {
            Assert.That(TestSubject.Value, Is.Null);
            TestSubject.ChangeValue(testName);

            var secondary = Get<TestSubject>("secondary");
            Assert.NotNull(secondary);
            Assert.That(secondary.Value, Is.EqualTo("default value"));

            secondary.ChangeValue(testName);
        }
    }
}
