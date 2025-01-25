using UnityEngine;

namespace BP.PoolIO
{
    [CreateAssetMenu(fileName = "Pool", menuName = "Pooling/Pool")]
    public class PoolAsset : PoolResource
    {
        [SerializeField] private string poolName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool reuseObjects;
        [SerializeField] private int initSize;
        [SerializeField] private int maxSize;

        public string PoolName => poolName;
        public GameObject Prefab => prefab;
        public bool ReuseObjects => reuseObjects;
        public int InitSize => initSize;
        public int MaxSize => maxSize;

        private IPoolable poolRef;

        public override void Init()
        {
            if (PoolUtils.IsNull(poolRef))
            {
                poolRef = GetPool();
            }
        }
        public override GameObject Get() => GetPool().Get();
        public override bool Release(GameObject gameObject) => poolRef?.Release(gameObject) ?? false;
        private IPoolable GetPool()
        {
            if (PoolUtils.IsNull(poolRef))
            {
                poolRef = PoolUtils.CreatePool(this);
            }

            return poolRef;
        }
    }
}
