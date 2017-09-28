using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace NUnit.Given
{
    public class GivenTestFixtureAttribute : TestFixtureAttribute, IFixtureBuilder
    {
        public GivenTestFixtureAttribute(Type contextType) : base()
        {
            ContextType = contextType;
        }

        public GivenTestFixtureAttribute(Type contextType, params object[] arguments) : base(arguments)
        {
            ContextType = contextType;
        }

        public Type ContextType { get; }

        public override object TypeId => base.TypeId;

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        public new IEnumerable<TestSuite> BuildFrom(ITypeInfo type)
        {
            return base.BuildFrom(type);
        }
    }
}