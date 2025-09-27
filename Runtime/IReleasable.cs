namespace BP.RefPool
{
    public interface IReleasable
    {
        bool Release(RefItem item);
    }
}
