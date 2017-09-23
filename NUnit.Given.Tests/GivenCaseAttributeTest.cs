using NUnit.Framework;
using NUnit.Given.Tests.Given;

namespace NUnit.Given.Tests
{
    public class GivenCaseAttributeTest : ContextualTest<GivenObject>
    {
        private static int _testCount1;
        private static int _testCount2;
        private static int _testCount3;
        private static int _testCount4;

        [GivenCase(typeof(GivenObject))]
        public void DefaultConstructor_ShouldBeInitialized()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("Default"));
        }

        [GivenCase(typeof(GivenObject.WithOneParameter))]
        public void WithOneParameter_ShouldAcceptArgument()
        {
            Assert.NotNull(Context);
            Assert.That(Context.Value, Is.EqualTo("one"));
        }

        [GivenCase(typeof(GivenObject.WithTwoParameters))]
        public void WithTwoParameters_ShouldCreateTwoTestCases()
        {
            _testCount1++;
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

        [GivenCase(typeof(GivenObject.WithTwoParameters), "action1")]
        public void WithTwoParameters_AndOneMethodArgument_ShouldCreateTwoTestCases(string action)
        {
            _testCount2++;
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount2)
            {
                case 1:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndOneMethodArgument_ShouldCreateTwoTestCases) + "(\"action1\") [one]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
                case 2:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndOneMethodArgument_ShouldCreateTwoTestCases) + "(\"action1\") [two]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    break;
            }
        }

        [GivenCase(typeof(GivenObject.WithTwoParameters), "action1", "action1more")]
        public void WithTwoParameters_AndTwoMethodArguments_ShouldCreateTwoTestCases(string action, string moreAction)
        {
            _testCount3++;
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount3)
            {
                case 1:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndTwoMethodArguments_ShouldCreateTwoTestCases) + "(\"action1\",\"action1more\") [one]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
                case 2:
                    Assert.That(testname, Is.EqualTo(nameof(WithTwoParameters_AndTwoMethodArguments_ShouldCreateTwoTestCases) + "(\"action1\",\"action1more\") [two]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    break;
            }
        }

        [GivenCase(typeof(GivenObject.WithTwoParameters), "action1", "action1more")]
        [GivenCase(typeof(GivenObject.WithTwoParameters), "action2", "action2more")]
        public void DoubleCases_WithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases(string action, string moreAction)
        {
            _testCount4++;
            var testname = TestContext.CurrentContext.Test.Name;

            switch (_testCount4)
            {
                case 1:
                    Assert.That(testname, Is.EqualTo(nameof(DoubleCases_WithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action1\",\"action1more\") [one]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
                case 2:
                    Assert.That(testname, Is.EqualTo(nameof(DoubleCases_WithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action1\",\"action1more\") [two]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    break;
                case 3:
                    Assert.That(testname, Is.EqualTo(nameof(DoubleCases_WithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action2\",\"action2more\") [one]"));
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
                case 4:
                    Assert.That(testname, Is.EqualTo(nameof(DoubleCases_WithTwoParameters_AndTwoMethodArguments_ShouldCreateFourTestCases) + "(\"action2\",\"action2more\") [two]"));
                    Assert.That(Context.Value, Is.EqualTo("two"));
                    break;
            }
        }

        [OneTimeTearDown]
        public void CheckTotalTestCounts()
        {
            Assert.That(_testCount1, Is.EqualTo(2));
            Assert.That(_testCount2, Is.EqualTo(2));
            Assert.That(_testCount3, Is.EqualTo(2));
            Assert.That(_testCount4, Is.EqualTo(4));
        }
    }
}