using System.Collections.Generic;

namespace NUnit.Given.Tests.Given
{
    public class GivenExtendedObject : GivenObject
    {
        public string MoreValue { get; }

        public GivenExtendedObject() : base("one")
        {
            MoreValue = "one more";
        }

        public GivenExtendedObject(string value, string moreValue) : base(value)
        {
            MoreValue = moreValue;
        }

        public new class WithTwoParameters : GivenExtendedObject
        {
            public WithTwoParameters() { }
            public WithTwoParameters(string value, string moreValue) : base(value, moreValue) { }

            public override IEnumerable<object[]> GetParameters()
            {
                yield return new object[] { "one", "one more" };
                yield return new object[] { "two", "two more" };
            }
        }
    }
}