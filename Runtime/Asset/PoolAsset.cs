using UnityEngine;

namespace BP.RefPool
{
    /// <summary>
    /// ScriptableObject representing a pooled GameObject resource.
    /// This asset acts as a referenceable and configurable container for a shared object pool.
    /// </summary>
    [CreateAssetMenu(fileName = "Pool", menuName = "RefPool/Pool")]
    public class PoolAsset : PoolResource
    {
        [SerializeField] private string poolName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize = 10;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private bool reuseObjects = true;
        [SerializeField] private bool isPersistent = false;

        private IPool poolRef;

        /// <summary>
        /// Display name of the pool. Defaults to the prefab name if unset.
        /// </summary>O
        public string PoolName => string.IsNullOrEmpty(poolName) && prefab ? prefab.name : poolName;

        /// <summary>
        /// The prefab that will be pooled.
        /// </summary>
        public GameObject Prefab => prefab;

        /// <summary>
        /// Number of instances to preallocate at initialization.
        /// </summary>
        public int InitSize => initSize;

        /// <summary>
        /// Maximum number of instances allowed in the pool.
        /// </summary>
        public int MaxSize => maxSize;

        /// <summary>
        /// Whether the pool reuses released instances when max size is reached.
        /// </summary>
        public bool ReuseObjects => reuseObjects;

        /// <summary>
        /// Whether the pool persists across scene changes.
        /// </summary>
        public bool IsPersistent => isPersistent;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(poolName) && prefab != null)
            {
                poolName = prefab.name;
            }
        }

        /// <summary>
        /// Initializes the internal pool if it hasn't been created yet.
        /// </summary>
        public override void Initialize()
        {
            if (PoolUtils.IsNull(poolRef))
            {
                poolRef = PoolUtils.CreatePool(this);
                poolRef.Initialize();
            }
        }

        /// <summary>
        /// Gets an instance from the pool, creating the pool if necessary.
        /// </summary>
        public override GameObject Get() => GetPool().Get();

        /// <summary>
        /// Returns an instance to the pool.
        /// </summary>
        public override bool Release(GameObject gameObject) => poolRef?.Release(gameObject) ?? false;

        /// <summary>
        /// Ensures the pool is initialized and returns the internal pool reference.
        /// </summary>
        private IPool GetPool()
        {
            Initialize();
            return poolRef;
        }
    }
}
