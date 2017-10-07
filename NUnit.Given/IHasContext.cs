namespace NUnit.Given
{
    public interface IHasContext<out T> where T : AbstractGivenTestContext, new()
    {
        T Context { get; }
    }
}