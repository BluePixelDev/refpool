using UnityEngine;

namespace BP.PoolIO
{
    /// <summary>
    /// ScriptableObject asset representing a pool for GameObjects.
    /// </summary>
    [CreateAssetMenu(fileName = "Pool", menuName = "Pooling/Pool")]
    public class PoolAsset : PoolResource
    {
        [SerializeField] private string poolName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize;
        [SerializeField] private int maxSize;
        [SerializeField] private bool reuseObjects;
        [SerializeField] private bool isPersistant;

        /// <summary>
        /// Gets the name of the pool.
        /// </summary>
        public string PoolName => poolName;

        /// <summary>
        /// Gets the prefab associated with this pool.
        /// </summary>
        public GameObject Prefab => prefab;

        /// <summary>
        /// Gets the initial size of the pool.
        /// </summary>
        public int InitSize => initSize;
        /// <summary>
        /// Gets the maximum size of the pool.
        /// </summary>
        public int MaxSize => maxSize;

        /// <summary>
        /// Gets a value indicating whether objects in the pool should be reused.
        /// </summary>
        public bool ReuseObjects => reuseObjects;

        /// <summary>
        /// Gets a value indicating whether the pool should persist across scene loads.
        /// </summary>
        public bool IsPersistant => isPersistant;

        private IPool poolRef;

        /// <summary>
        /// Retrieves a GameObject from the pool.
        /// </summary>
        /// <returns>The pooled GameObject.</returns>
        public override GameObject Get() => GetPool().Get();

        /// <summary>
        /// Releases a GameObject back into the pool.
        /// </summary>
        /// <param name="gameObject">The GameObject to release.</param>
        /// <returns>True if the object was successfully released; otherwise, false.</returns>
        public override bool Release(GameObject gameObject) => poolRef?.Release(gameObject) ?? false;

        /// <summary>
        /// Gets the pool instance, creating it if necessary.
        /// </summary>
        /// <returns>The pool instance implementing IPoolable.</returns>
        private IPool GetPool()
        {
            if (PoolUtils.IsNull(poolRef))
            {
                poolRef = PoolUtils.CreatePool(this);
            }

            return poolRef;
        }
    }
}
