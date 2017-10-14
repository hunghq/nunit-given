namespace NUnit.Given
{
    public interface IHasContext<out T> where T : class, IGiven
    {
        T Context { get; }
    }
}