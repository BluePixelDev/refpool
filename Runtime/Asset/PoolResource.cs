using UnityEngine;

namespace BP.PoolIO
{
    public abstract class PoolResource : ScriptableObject, IPoolable
    {
        public abstract void Init();
        public abstract GameObject Get();
        public abstract bool Release(GameObject gameObject);
    }
}
