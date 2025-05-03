using UnityEngine;

namespace BP.RefPool
{
    /// <summary>
    /// ScriptableObject representing a referenceable group of pool assets.
    /// Allows configuring pooled objects and pick strategy via asset.
    /// </summary>
    [CreateAssetMenu(fileName = "PoolGroup", menuName = "RefPool/Pool Group")]
    public class PoolGroupAsset : PoolResource
    {
        [SerializeField] private string groupName = "New Group";
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private PoolAsset[] pools;
        [SerializeField] private bool isPersistent = false;

        private PoolGroup groupRef;

        /// <summary>
        /// Optional display name for the pool group.
        /// </summary>
        public string GroupName => groupName;

        /// <summary>
        /// Strategy used to pick which pool to pull from.
        /// </summary>
        public PoolPickMode PickMode => pickMode;

        /// <summary>
        /// Array of pool assets that make up this group.
        /// </summary>
        public PoolAsset[] Pools => pools;

        /// <summary>
        /// Whether this group should persist across scene loads.
        /// </summary>
        public bool IsPersistent => isPersistent;

        /// <summary>
        /// Initializes the internal PoolGroup instance.
        /// </summary>
        public override void Initialize()
        {
            if (groupRef == null)
            {
                groupRef = PoolUtils.CreatePoolGroup(this);
                groupRef.Initialize();
            }
        }

        /// <summary>
        /// Gets a pooled object using the group strategy.
        /// </summary>
        public override GameObject Get() => GetPoolGroup().Get();

        /// <summary>
        /// Attempts to release the GameObject to one of the internal pools.
        /// </summary>
        public override bool Release(GameObject gameObject)
        {
            return groupRef != null && groupRef.Release(gameObject);
        }

        /// <summary>
        /// Ensures the group is initialized and returns the internal reference.
        /// </summary>
        private PoolGroup GetPoolGroup()
        {
            Initialize();
            return groupRef;
        }
    }
}
