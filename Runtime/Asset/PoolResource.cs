using UnityEngine;

namespace BP.PoolIO
{
    public abstract class PoolResource : ScriptableObject, IPool
    {
        public abstract GameObject Get();
        public abstract bool Release(GameObject gameObject);
    }
}
