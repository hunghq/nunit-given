using NUnit.Framework;
using NUnit.Given.Tests.Given;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Given.Tests
{
    [GivenTestFixture(typeof(GivenObject))]
    public class GivenTestFixtureAttributeTest : IHasContext<GivenObject>
    {
        public GivenObject Context { get; set; }

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
    }
}
    