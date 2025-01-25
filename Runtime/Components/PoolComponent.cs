using UnityEngine;

namespace BP.PoolIO
{
    /// <summary>
    /// Base class for components that implemnt IPool interface.
    /// </summary>
    public abstract class PoolComponent : MonoBehaviour, IPoolable
    {
        public abstract void Init();
        public abstract GameObject Get();
        public abstract bool Release(GameObject gameObject);
    }
}
