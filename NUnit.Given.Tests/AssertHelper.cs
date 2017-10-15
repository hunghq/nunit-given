using System;
using NUnit.Framework;

namespace NUnit.Given.Tests
{
    public class AssertHelper
    {
        public static void ExpectException<T>(TestDelegate testDelegate, string expectedErrorMessage)
            where T : Exception
        {
            var exception = Assert.Throws<T>(testDelegate);
            Assert.That(exception.Message, Does.Contain(expectedErrorMessage));
        }
    }
}