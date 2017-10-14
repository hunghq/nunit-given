using System;
using System.Linq;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public class GivenTestContext : AbstractGivenTestContext
    {
    }

    public abstract class AbstractGivenTestContext
    {
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
                return given;
            }
            catch (Exception e)
            {
                return new ErrorTestContext(type, arguments, e.InnerException ?? e);
            }
        }
    }
}