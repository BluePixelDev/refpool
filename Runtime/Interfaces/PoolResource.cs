using UnityEngine;

namespace BP.RefPool
{
    /// <summary>
    /// Abstract base class for pool resources that manage the lifecycle of <see cref="GameObject"/> instances.
    /// </summary>
    public abstract class PoolResource : ScriptableObject, IPool
    {
        public abstract void Initialize();
        public abstract GameObject Get();
        public abstract bool Release(GameObject gameObject);
    }
}
