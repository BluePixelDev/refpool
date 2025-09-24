using UnityEngine;

namespace BP.RefPool
{
    [CreateAssetMenu(fileName = "Pool", menuName = "RefPool/Pool")]
    public class PoolAsset : RefResource
    {
        [SerializeField] private string displayName = "New Pool";
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize = 10;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private bool reuseObjects = true;
        [SerializeField] private bool isPersistent = false;

        private IPool poolRef;

        public string DisplayName => displayName;
        public GameObject Prefab => prefab;
        public int InitSize => initSize;
        public int MaxSize => maxSize;
        public bool ReuseObjects => reuseObjects;
        public bool IsPersistent => isPersistent;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(displayName) && prefab != null)
            {
                displayName = prefab.name;
            }
        }

        public override void Initialize()
        {
            if (RefUtils.IsUnityNull(poolRef))
            {
                poolRef = RefUtils.CreatePool(this);
                poolRef.Initialize();
            }
        }

        public override RefItem Get() => GetPool().Get();
        public override bool Release(RefItem item) => poolRef?.Release(item) ?? false;
        private IPool GetPool()
        {
            Initialize();
            return poolRef;
        }
    }
}
