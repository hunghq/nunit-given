﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NUnit.Given
{
    public class GivenAttribute : TestCaseAttribute, ITestBuilder
    {
        private static readonly NUnitTestCaseBuilder Builder = new NUnitTestCaseBuilder();

        public GivenAttribute(Type contextType, object arg) : base(arg)
        {
            SetContextType(contextType);
        }

        public GivenAttribute(Type contextType, params object[] arguments) : base(arguments)
        {
            SetContextType(contextType);
        }

        public GivenAttribute(Type contextType, object arg1, object arg2) : base(arg1, arg2)
        {
            SetContextType(contextType);
        }

        public GivenAttribute(Type contextType, object arg1, object arg2, object arg3) : base(arg1, arg2, arg3)
        {
            SetContextType(contextType);
        }

        public Type ContextType { get; private set; }

        public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            foreach (var testMethod in base.BuildFrom(method, suite))
            foreach (var testMethodInstance in GetTestMethodsWithContext(testMethod, suite))
                yield return testMethodInstance;
        }

        private IEnumerable<TestMethod> GetTestMethodsWithContext(TestMethod testMethod, Test suite)
        {
            var context = GivenTestContext.From(ContextType, null);
            var givens = context.Parameterize().ToList();
            if (givens.Count == 1)
            {
                SetTestContext(testMethod, context);
                yield return testMethod;
            }
            else
            {
                foreach (var given in givens)
                {
                    var testMethodPerGivenContext = Builder.BuildTestMethod(testMethod.Method, suite,
                        new TestCaseParameters(testMethod.Arguments));
                    SetTestContext(testMethodPerGivenContext, given);
                    yield return testMethodPerGivenContext;
                }
            }
        }

        private void SetTestContext(TestMethod testMethod, GivenTestContext context)
        {
            testMethod.Properties.Set(ContextualTest.ContextKey, context);
            var args = context.CurrentParameterssAsString;
            testMethod.Name += string.IsNullOrEmpty(args) ? "" : $" [{args}]";
        }

        private void SetContextType(Type contextType)
        {
            if (!contextType.IsSubclassOf(typeof(GivenTestContext)))
                throw new ArgumentException($"ContextType {contextType.Name} must extend GivenTestContext.");

            ContextType = contextType;
        }
    }
}