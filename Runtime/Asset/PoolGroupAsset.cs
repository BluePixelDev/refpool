using UnityEngine;

namespace BP.PoolIO
{
    public enum PoolPickMode
    {
        Random,
        Sequential,
        Back2Back
    }

    [CreateAssetMenu(fileName = "PoolGroup", menuName = "Pooling/PoolGroup")]
    public class PoolGroupAsset : PoolResource
    {
        [SerializeField] private string groupName;
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private PoolAsset[] pools;

        public string GroupName => groupName;
        public PoolPickMode PickMode => pickMode;
        public PoolAsset[] Pools => pools;

        private PoolGroup groupRef;

        public override void Init()
        {
            if (PoolUtils.IsNull(groupRef))
            {
                groupRef = GetPoolGroup();
            }
        }
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
