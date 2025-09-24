using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefComponent : MonoBehaviour, IPool
    {
        public abstract void Initialize();
        public abstract RefItem Get();
        public abstract bool Release(RefItem item);
        public abstract void MakePersistent();
    }
}
