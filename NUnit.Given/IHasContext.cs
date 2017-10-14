namespace NUnit.Given
{
    public interface IHasContext<out T> where T : class
    {
        T Context { get; }
    }
}