namespace AOP.Hydra.PostSharp
{
    public interface IService
    {
        ITransaction Transaction { get; set; }
    }
}