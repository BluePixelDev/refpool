using UnityEngine;

namespace BP.RefPool
{
    public enum PoolPickMode
    {
        Random,
        Sequential,
        Back2Back
    }

    [CreateAssetMenu(fileName = "PoolGroup", menuName = "RefPool/Pool Group")]
    public class PoolGroupAsset : RefResource
    {
        [SerializeField] private string displayName = "New Group";
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private bool isPersistent = false;
        [SerializeField] private PoolAsset[] pools;

        private RefPoolerGroup groupRef;

        public string DisplayName => displayName;
        public PoolPickMode PickMode => pickMode;
        public PoolAsset[] Pools => pools;
        public bool IsPersistent => isPersistent;

        public override void Initialize()
        {
            if (RefUtils.IsUnityNull(groupRef))
            {
                groupRef = RefUtils.CreatePoolGroup(this);
                groupRef.Initialize();
            }
        }
        public override RefItem Get() => GetPoolGroup().Get();
        public override bool Release(RefItem item) => groupRef != null && groupRef.Release(item);
        private RefPoolerGroup GetPoolGroup()
        {
            Initialize();
            return groupRef;
        }
    }
}
