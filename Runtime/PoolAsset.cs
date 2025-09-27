using UnityEngine;

namespace BP.RefPool
{
    [CreateAssetMenu(fileName = "Pool", menuName = "RefPool/Pool")]
    public class PoolAsset : RefResource
    {
        const string POOL_NAME_PREFIX = "[P]";

        [SerializeField] private string displayName = "New Pool";
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize = 10;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private bool reuseObjects = true;

        [HideInInspector] private RefComponent poolRef;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(displayName) && prefab != null)
            {
                displayName = prefab.name;
            }
        }

        public override RefItem Get()
        {
            Prepare();
            return poolRef.Get();
        }
        public override void Prepare()
        {
            if (RefUtils.IsUnityNull(poolRef))
            {
                poolRef = CreateComponent();
            }
        }
        public override RefComponent CreateComponent()
        {
            var poolName = RefUtils.FormatName(displayName, POOL_NAME_PREFIX);
            var gameObject = new GameObject(poolName);
            var poolComponent = gameObject.AddComponent<RefPooler>();
            ApplyAsset(poolComponent);
            return poolComponent;
        }

        private void ApplyAsset(RefPooler pooler)
        {
            pooler.Prefab = prefab;
            pooler.InitSize = initSize;
            pooler.MaxSize = maxSize;
            pooler.ReuseObjects = reuseObjects;
        }
    }
}
