namespace AOP.Hydra.PostSharp
{
    public interface IAbstractFactory<out T>
        where T : class
    {
        T CreateInstance();
    }
}
