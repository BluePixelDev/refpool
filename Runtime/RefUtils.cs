using UnityEngine;

namespace BP.RefPool
{
    internal static class RefUtils
    {
        const string POOL_NAME_PREFIX = "[P]";
        const string POOL_ITEM_NAME_PREFIX = "[PI]";
        const string POOLGROUP_NAME_PREFIX = "[PG]";

        public static RefPooler CreatePool(PoolAsset poolAsset)
        {
            var poolName = FormatName(poolAsset.DisplayName, POOL_NAME_PREFIX);
            var gameObject = new GameObject(poolName);
            var pool = gameObject.AddComponent<RefPooler>();
            pool.ApplyResource(poolAsset);
            return pool;
        }

        public static RefPoolerGroup CreatePoolGroup(PoolGroupAsset groupAsset)
        {
            var groupName = FormatName(groupAsset.DisplayName, POOLGROUP_NAME_PREFIX);
            var gameObject = new GameObject(groupName);
            var pg = gameObject.AddComponent<RefPoolerGroup>();
            foreach (var poolAsset in groupAsset.Pools)
            {
                var p = CreatePool(poolAsset);
                pg.AddPool(p);

#if UNITY_EDITOR
                p.transform.SetParent(pg.transform);
#endif
            }
            pg.ApplyResource(groupAsset);
            return pg;
        }

        public static RefItem CreatePooledItem(RefPooler pooler)
        {
            if (pooler == null) return null;

            var prefab = pooler.Prefab;
            var go = Object.Instantiate(prefab);
            go.name = $"{POOL_ITEM_NAME_PREFIX}{prefab.name}";

            if (!go.TryGetComponent<RefItem>(out var refItem))
            {
                refItem = AddRefItem(go, pooler);
            }
            else
            {
                refItem.SetOwner(pooler);
            }

            refItem.isUsed = false;
            // Avoids setting parent in build to save on performance
#if UNITY_EDITOR
            go.transform.SetParent(pooler.transform);
#endif
            return refItem;
        }

        public static RefItem AddRefItem(GameObject gameObject, RefPooler pooler)
        {
            var refItem = gameObject.AddComponent<RefItem>();
            refItem.SetOwner(pooler);
            refItem.ResetState();
            return refItem;
        }

        private static string FormatName(string name, string prefix) => string.IsNullOrEmpty(name) ? $"{prefix}Unnamed" : $"{prefix}{name}";
        public static bool IsUnityNull(this object gameObject) => gameObject == null || gameObject.Equals(null);


        public static float ClampMin(float value, float min)
        {
            if (value < min) return min;
            return value;
        }
        public static int ClampMin(int value, int min)
        {
            if (value < min) return min;
            return value;
        }
    }
}
