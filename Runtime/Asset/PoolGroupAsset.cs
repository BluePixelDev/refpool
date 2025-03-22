using UnityEngine;

namespace BP.PoolIO
{
    /// <summary>
    /// Enumeration for selecting the pooling strategy.
    /// </summary>
    public enum PoolPickMode
    {
        Random,
        Sequential,
        Back2Back
    }

    /// <summary>
    /// ScriptableObject asset representing a group of pools.
    /// </summary>
    [CreateAssetMenu(fileName = "PoolGroup", menuName = "Pooling/PoolGroup")]
    public class PoolGroupAsset : PoolResource
    {
        [SerializeField] private string groupName;
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private PoolAsset[] pools;
        [SerializeField] private bool isPersistent;

        /// <summary>
        /// Gets the name of the pool group.
        /// </summary>
        public string GroupName => groupName;

        /// <summary>
        /// Gets the picking mode for selecting pools.
        /// </summary>
        public PoolPickMode PickMode => pickMode;

        /// <summary>
        /// Gets the array of pool assets in the group.
        /// </summary>
        public PoolAsset[] Pools => pools;

        /// <summary>
        /// Gets a value indicating whether the pool group should persist across scene loads.
        /// </summary>
        public bool IsPersistent => isPersistent;

        private PoolGroup groupRef;

        public override GameObject Get() => GetPoolGroup().Get();
        public override bool Release(GameObject gameObject)
        {
            if (PoolUtils.IsNull(groupRef)) return false;
            return groupRef.Release(gameObject);
        }
        private PoolGroup GetPoolGroup()
        {
            if (PoolUtils.IsNull(groupRef))
            {
                groupRef = PoolUtils.CreatePoolGroup(this);
            }
            return groupRef;
        }
    }
}
