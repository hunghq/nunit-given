using System;
using System.Collections.Generic;

namespace NUnit.Given.Tests.Given.Defect
{
    public class GivenDefectObject : GivenObject
    {
        public GivenDefectObject()
        {
            throw new InvalidOperationException("Something is wrong when setting up this test context.");
        }

        public GivenDefectObject(string value) : base(value)
        {
            
        }

        public new class WithTwoParameters : GivenDefectObject
        {
            public WithTwoParameters() : base("one")
            {
                
            }

            public WithTwoParameters(string value) : base(value)
            {
                throw new InvalidOperationException("Something is wrong when setting up this test context with value: " + value);
            }

            public override IEnumerable<object[]> GetParameters()
            {
                yield return new object[] { "one" };
                yield return new object[] { "two" };
            }
        }
    }
}