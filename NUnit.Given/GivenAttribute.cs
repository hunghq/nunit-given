using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public class GivenAttribute : TestAttribute, ITestBuilder
    {
        public GivenAttribute(Type contextType)
        {
            if (!contextType.IsSubclassOf(typeof(GivenTestContext)))
                throw new ArgumentException($"ContextType {contextType.Name} must extend GivenTestContext.");

            ContextType = contextType;
        }

        public Type ContextType { get; }

        public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var context = GivenTestContext.From(ContextType, null);
            foreach (var contextPerParameter in context.Parameterize())
                yield return CreateTestMethod(method, suite, contextPerParameter);
        }

        private TestMethod CreateTestMethod(IMethodInfo method, Test suite, GivenTestContext context)
        {
            var testMethod = base.BuildFrom(method, suite);
            testMethod.Properties.Set(ContextualTest.ContextKey, context);
            var args = context.CurrentParameterssAsString;
            testMethod.Name += string.IsNullOrEmpty(args) ? "" : $" [{args}]";
            return testMethod;
        }
    }
}