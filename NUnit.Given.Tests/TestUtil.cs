using NUnit.Framework;

namespace NUnit.Given.Tests
{
    public class TestUtil
    {
        public static void IncrementTestCount(ref int? testCount)
        {
            if (testCount == null) testCount = 0;
            testCount += 1;
        }

        public static void AssertTestCount(int? testCount, int expected)
        {
            if (testCount.HasValue)
                Assert.That(testCount, Is.EqualTo(expected));
        }
    }
}