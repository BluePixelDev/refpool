namespace BP.RefPool
{
    public interface IResourceApplier<T> where T : RefResource
    {
        void ApplyResource(T asset);
    }
}
