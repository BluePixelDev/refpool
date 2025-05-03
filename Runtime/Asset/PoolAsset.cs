using UnityEngine;

namespace BP.RefPool
{
    /// <summary>
    /// ScriptableObject asset representing a pool for GameObjects.
    /// </summary>
    [CreateAssetMenu(fileName = "Pool", menuName = "RefPool/Pool")]
    public class PoolAsset : PoolResource
    {
        [SerializeField] private string poolName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize;
        [SerializeField] private int maxSize;
        [SerializeField] private bool reuseObjects;
        [SerializeField] private bool isPersistent;

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
        public bool IsPersistent => isPersistent;

        private IPool poolRef;

        public override void Initialize()
        {
            if (PoolUtils.IsNull(poolRef))
            {
                poolRef = PoolUtils.CreatePool(this);
                poolRef.Initialize();
            }
        }
        public override GameObject Get() => GetPool().Get();
        public override bool Release(GameObject gameObject) => poolRef?.Release(gameObject) ?? false;

        /// <summary>
        /// Gets the pool instance, creating it if necessary.
        /// </summary>
        /// <returns>The pool instance implementing IPoolable.</returns>
        private IPool GetPool()
        {
            Initialize();
            return poolRef;
        }
    }
}
