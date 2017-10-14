using System.Collections;

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
        
        public class WithTwoParameters : GivenObject
        {
            public WithTwoParameters() {}
            public WithTwoParameters(string value) : base(value) { }
            
            [GivenCaseSource]
            public static IEnumerable GetTestCases()
            {
                yield return "one";
                yield return "two";
            }
        }
    }
}