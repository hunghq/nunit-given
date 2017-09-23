using System.Collections;
using NUnit.Framework.Internal;

namespace NUnit.Given
{
    public class GivenTestContextReader
    {
        public static IEnumerable GetAll<T>() where T : GivenTestContext
        {
            var givenContext = (T)Reflect.Construct(typeof(T), null);
            return givenContext.Parameterize();
        }

        public static T GetDefault<T>() where T : GivenTestContext
        {
            return Reflect.Construct(typeof(T), null) as T;
        }
    }
}