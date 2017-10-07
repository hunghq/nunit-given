using NUnit.Framework;
using NUnit.Given.Tests.Given;
using NUnit.Given.Tests.Given.Defect;

namespace NUnit.Given.Tests
{
    [GivenTestFixture(typeof(GivenObject), "action_0")]
    [GivenTestFixture(typeof(GivenObject.WithOneParameter), "action_1")]
    [GivenTestFixture(typeof(GivenDefectObject), "action_X")]
    public class GivenTestFixtureAttributeTest : IHasContext<GivenObject>
    {
        public GivenObject Context { get; set; }
        public string Action { get; }
        private static int _testCount1;

        public GivenTestFixtureAttributeTest() {}

        public GivenTestFixtureAttributeTest(string action)
        {
            Action = action;
        }

        [Test]
        public void ConstructorParameterShouldBePassed()
        {
            _testCount1++;
            switch (_testCount1)
            {
                case 1:
                    Assert.That(Action, Is.EqualTo("action_0"));
                    break;
                case 2:
                    Assert.That(Action, Is.EqualTo("action_1"));
                    break;
                default:
                    Assert.That(_testCount1, Is.LessThan(3));
                    break;
            }
        }

        [Test]
        public void ContextShoulbeInitialized()
        {
            Assert.That(Context, Is.Not.Null);
            
            switch (Action)
            {
                case "action_0":
                    Assert.That(Context.Value, Is.EqualTo("Default"));
                    break;
                case "action_1":
                    Assert.That(Context.Value, Is.EqualTo("one"));
                    break;
            }
        }
    }
}