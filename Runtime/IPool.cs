namespace BP.RefPool
{
    public interface IPool
    {
        void Initialize();
        RefItem Get();
        bool Release(RefItem item);
    }
}
