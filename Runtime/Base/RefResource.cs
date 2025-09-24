using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefResource : ScriptableObject, IPool
    {
        public abstract void Initialize();
        public abstract RefItem Get();
        public abstract bool Release(RefItem item);
    }
}
