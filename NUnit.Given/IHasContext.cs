namespace NUnit.Given
{
    public interface IHasContext<T> where T : GivenTestContext
    {
        T Context { get; set; }
    }
}