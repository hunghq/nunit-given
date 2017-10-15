using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NUnit.Given
{
    public class GivenAttribute : TestCaseAttribute, ITestBuilder, IApplyToContext
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

        public void ApplyToContext(TestExecutionContext context)
        {
            if (context.CurrentTest.IsSuite) return;

            var parameters = context.CurrentTest.Properties.Get(ContextualTest.ContextParametersKey) as List<object>;
            var given = ContextualTest.From(ContextType, parameters?.ToArray());
            context.CurrentTest.Properties.Set(ContextualTest.ContextKey, given);
        }

        public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            TestCaseParameters invalidContextParameters = null;
            var cases = GetCasesFromContext(ContextType);
            var error = ValidateCases(cases);

            if (!string.IsNullOrEmpty(error))
            {
                invalidContextParameters = CreateIgnoredTestCaseParameters(error);
            }

            foreach (var testMethod in base.BuildFrom(method, suite))
            {
                if (invalidContextParameters != null)
                {
                    yield return Builder.BuildTestMethod(method, suite, invalidContextParameters);
                }
                else
                {
                    foreach (var @case in cases)
                    {
                        var testMethodPerContextParameter = Builder.BuildTestMethod(testMethod.Method, suite,
                            new TestCaseParameters(testMethod.Arguments));

                        SetTestContextParameters(testMethodPerContextParameter, @case);
                        yield return testMethodPerContextParameter;
                    }
                }
            }
        }

        private static TestCaseParameters CreateIgnoredTestCaseParameters(string error)
        {
            var invalidContextParameters = new TestCaseParameters
            {
                RunState = RunState.NotRunnable
            };

            invalidContextParameters.Properties.Set(PropertyNames.SkipReason, error);
            return invalidContextParameters;
        }

        private static List<List<object>> GetCasesFromContext(Type type)
        {
            var result = new List<List<object>>();
            var sourceMethod = type
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(x => x.GetCustomAttribute(typeof(GivenCaseSourceAttribute)) != null);

            if (sourceMethod != null)
            {
                if (sourceMethod.IsStatic)
                {
                    var cases = (IEnumerable)sourceMethod.Invoke(null, null);
                    if (cases != null)
                    {
                        foreach (var @case in cases)
                        {
                            var array = @case as Array;

                            if (array != null)
                            {
                                result.Add(array.Cast<object>().ToList());
                            }
                            else
                            {
                                result.Add(new List<object> { @case });
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static void SetTestContextParameters(TestMethod testMethod, List<object> contextParameters)
        {
            testMethod.Properties.Set(ContextualTest.ContextParametersKey, contextParameters);
            var args = contextParameters == null ? "" : string.Join(",", contextParameters.Select(x => x.ToString()));
            testMethod.Name += string.IsNullOrEmpty(args) ? "" : $" [{args}]";
        }

        private void SetContextType(Type contextType)
        {
            ContextualTest.Validate(contextType);
            ContextType = contextType;
        }

        private string ValidateCases(List<List<object>> cases)
        {
            if (cases.Any())
            {
                var firstCase = cases.First();
                if (ContextType.GetConstructors().All(c => c.GetParameters().Length != firstCase.Count))
                {
                    return $"GivenParameterSourceAttribute may not be used as there is no constructor with {firstCase.Count} parameters in {ContextType.FullName}.";
                }
            }
            return null;
        }
    }
}