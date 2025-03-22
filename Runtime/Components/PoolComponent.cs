using UnityEngine;

namespace BP.PoolIO
{
    /// <summary>
    /// Base class for components that implemnt IPool interface.
    /// </summary>
    public abstract class PoolComponent : MonoBehaviour, IPool
    {
        public abstract GameObject Get();
        public abstract bool Release(GameObject gameObject);
        public abstract void MakePersistent();
    }
}
