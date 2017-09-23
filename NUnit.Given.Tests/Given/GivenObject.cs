using System.Collections.Generic;

namespace NUnit.Given.Tests.Given
{
    public class GivenObject : GivenTestContext
    {
        public GivenObject()
        {
            Value = "Default";
        }

        public GivenObject(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public class WithOneParameter : GivenObject
        {
            public WithOneParameter() : base ("one") { }
        }

        public class WithTwoParameters : GivenObject
        {
            public WithTwoParameters() {}
            public WithTwoParameters(string value) : base(value) { }

            public override IEnumerable<object[]> GetParameters()
            {
                yield return new[] { "one" };
                yield return new[] { "two" };
            }
        }
    }
}