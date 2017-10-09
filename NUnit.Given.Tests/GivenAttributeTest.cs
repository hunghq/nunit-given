using System.Linq;
using NUnit.Framework;
using NUnit.Given.Tests.Given;
using NUnit.Given.Tests.Given.Defect;

namespace NUnit.Given.Tests
{
    public class GivenAttributeTest : ContextualTest<GivenObject>
    {
        private static int? _testCount1;
        private static int? _testCount2;
        private static int? _testCount3;
        private static int? _testCount4;
        private static int? _testCount5;

        [Given(typeof(GivenObject))]
        public void DefaultConstructor_ShouldBeInitialized()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("Default"));
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
            IncrementTestCount(ref _testCount1);
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount1)
            {
                case 1:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_ShouldCreateTwoTestCases) + "() [one]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
                case 2:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_ShouldCreateTwoTestCases) + "() [two]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    break;
            }
        }

        [Given(typeof(GivenObject.WithTwoParameters), "action1")]
        public void WithTwoParameters_AndOneMethodArgument_ShouldCreateTwoTestCases(string action)
        {
            IncrementTestCount(ref _testCount2);
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount2)
            {
                case 1:
                    Assert.That(action, Is.EqualTo("action1"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndOneMethodArgument_ShouldCreateTwoTestCases) + "(\"action1\") [one]"));                    
                    break;
                case 2:
                    Assert.That(action, Is.EqualTo("action1"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndOneMethodArgument_ShouldCreateTwoTestCases) + "(\"action1\") [two]"));                    
                    break;
            }
        }

        [Given(typeof(GivenObject.WithTwoParameters), "action1", "action1more")]
        public void WithTwoParameters_AndTwoMethodArguments_ShouldCreateTwoTestCases(string action, string moreAction)
        {
            IncrementTestCount(ref _testCount3);
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount3)
            {
                case 1:
                    Assert.That(action, Is.EqualTo("action1"));
                    Assert.That(moreAction, Is.EqualTo("action1more"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndTwoMethodArguments_ShouldCreateTwoTestCases) + "(\"action1\",\"action1more\") [one]"));
                    break;
                case 2:
                    Assert.That(action, Is.EqualTo("action1"));
                    Assert.That(moreAction, Is.EqualTo("action1more"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndTwoMethodArguments_ShouldCreateTwoTestCases) + "(\"action1\",\"action1more\") [two]"));
                    break;
            }
        }

        [Given(typeof(GivenObject.WithTwoParameters), "action1", "action1more")]
        [Given(typeof(GivenObject.WithTwoParameters), "action2", "action2more")]
        public void DoubleGivensWithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases(string action, string moreAction)
        {
            IncrementTestCount(ref _testCount4);
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount4)
            {
                case 1:
                    Assert.That(action, Is.EqualTo("action1"));
                    Assert.That(moreAction, Is.EqualTo("action1more"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    Assert.That(testname, Is.EqualTo(nameof(DoubleGivensWithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action1\",\"action1more\") [one]"));
                    break;
                case 2:
                    Assert.That(action, Is.EqualTo("action1"));
                    Assert.That(moreAction, Is.EqualTo("action1more"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    Assert.That(testname, Is.EqualTo(nameof(DoubleGivensWithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action1\",\"action1more\") [two]"));
                    break;
                case 3:
                    Assert.That(action, Is.EqualTo("action2"));
                    Assert.That(moreAction, Is.EqualTo("action2more"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    Assert.That(testname, Is.EqualTo(nameof(DoubleGivensWithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action2\",\"action2more\") [one]"));
                    break;
                case 4:
                    Assert.That(action, Is.EqualTo("action2"));
                    Assert.That(moreAction, Is.EqualTo("action2more"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    Assert.That(testname, Is.EqualTo(nameof(DoubleGivensWithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action2\",\"action2more\") [two]"));
                    break;
            }
        }

        [Test(Description = "Exception in given object should result in ignored test cases which can be run explicitly.")]
        [Given(typeof(GivenDefectObject))]
        public void ExceptionInDefaultConstructor_ShouldMakeTestIgnored()
        {
            var errorContext = TestContext.CurrentContext.Test.Properties.Get(ContextKey) as ErrorTestContext;
            Assert.Fail("Test should not be executed because there was exception when setting up the test context: " + errorContext?.Exception);
        }

        [Test(Description = "Exception in given object should result in ignored test cases which can be run explicitly.")]
        [Given(typeof(GivenDefectObject.WithTwoParameters))]
        public void ExceptionInConstructorWithTwoParameters_ShouldMakeTwoTestsIgnored()
        {
            IncrementTestCount(ref _testCount5);
            var errorContext = TestContext.CurrentContext.Test.Properties.Get(ContextKey) as ErrorTestContext;
            Assert.Fail("Test should not be executed because there was exception when setting up the test context: " + errorContext?.Exception);
        }

        [OneTimeTearDown]
        public void CheckTotalTestCounts()
        {
            AssertTestCount(_testCount1, 2);
            AssertTestCount(_testCount2, 2);
            AssertTestCount(_testCount3, 2);
            AssertTestCount(_testCount4, 4);
            AssertTestCount(_testCount5, 2);
        }

        private static void IncrementTestCount(ref int? testCount)
        {
            if (testCount == null) testCount = 0;
            testCount += 1;
        }

        private static void AssertTestCount(int? testCount, int expected)
        {
            if (testCount.HasValue)
                Assert.That(testCount, Is.EqualTo(expected));
        }
    }
}