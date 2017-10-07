using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public class GivenTestContext : AbstractGivenTestContext
    {
        public override IEnumerable<AbstractGivenTestContext> Parameterize()
        {
            if (GetParameters().Any())
                foreach (var contextArguments in GetParameters())
                    yield return From(GetType(), contextArguments);
            else
                yield return this;
        }
    }

    public abstract class AbstractGivenTestContext
    {
        public object[] CurrentParameters { get; protected set; }

        public string CurrentParametersAsString => CurrentParameters == null ? "" : string.Join(",", CurrentParameters?.Select(x => x.ToString()));

        public virtual IEnumerable<object[]> GetParameters()
        {
            return Enumerable.Empty<object[]>();
        }

        public virtual IEnumerable<AbstractGivenTestContext> Parameterize()
        {
            yield return this;
        }

        public static AbstractGivenTestContext From(Type type, object[] arguments)
        {
            if (!type.IsSubclassOf(typeof(AbstractGivenTestContext)))
                throw new ArgumentException($"ContextType {type.Name} must extend GivenTestContext.");

            try
            {
                var context = arguments != null && arguments.Any()
                    ? Reflect.Construct(type, arguments)
                    : Reflect.Construct(type);

                var given = (AbstractGivenTestContext) context;
                given.CurrentParameters = arguments;
                return given;
            }
            catch (Exception e)
            {
                return new ErrorTestContext(type, arguments, e.InnerException ?? e);
            }
        }
    }
}