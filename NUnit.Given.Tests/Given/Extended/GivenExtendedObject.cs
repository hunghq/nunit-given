using System.Collections;

namespace NUnit.Given.Tests.Given.Extended
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
            public WithTwoParameters(string value, string moreValue) : base(value, moreValue) { }
            
            [GivenCaseSource]
            public static IEnumerable GetParameters()
            {
                yield return new [] { "one", "one more" };
                yield return new [] { "two", "two more" };
            }
        }
    }
}