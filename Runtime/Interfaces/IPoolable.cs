using UnityEngine;

namespace BP.PoolIO
{
    public interface IPoolable
    {
        GameObject Get();
        bool Release(GameObject gameObject);
    }
}
