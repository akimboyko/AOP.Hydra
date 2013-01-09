namespace AOP.Hydra.PostSharp
{
    public interface ITransaction
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}