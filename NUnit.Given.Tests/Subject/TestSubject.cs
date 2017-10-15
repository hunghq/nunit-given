namespace NUnit.Given.Tests.Subject
{
    public class TestSubject
    {
        public TestSubject()
        {
            
        }

        public TestSubject(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public void ChangeValue(string value)
        {
            Value = value;
        }
    }
}