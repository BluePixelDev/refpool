using UnityEngine;

namespace BP.RefPool
{
    public static class PoolUtils
    {
        const string POOL_NAME_PREFIX = "[POOL]_";
        const string POOLGROUP_NAME_PREFIX = "[PGROUP]_";

        public static Pool CreatePool(PoolAsset poolAsset)
        {
            var poolName = FormatName(poolAsset.PoolName, POOL_NAME_PREFIX);
            var gameObject = new GameObject(poolName);
            var pool = gameObject.AddComponent<Pool>();
            pool.SetAsset(poolAsset);
            return pool;
        }

        public static PoolGroup CreatePoolGroup(PoolGroupAsset groupAsset)
        {
            var groupName = FormatName(groupAsset.GroupName, POOLGROUP_NAME_PREFIX);
            var gameObject = new GameObject(groupName);
            var pg = gameObject.AddComponent<PoolGroup>();
            foreach (var pool in groupAsset.Pools)
            {
                pg.AddPool(CreatePool(pool));
            }
            pg.SetAsset(groupAsset);
            return pg;
        }

        private static string FormatName(string name, string prefix) => string.IsNullOrEmpty(name) ? $"{prefix}Unnamed" : $"{prefix}{name}";
        public static bool IsNull(this object gameObject) => gameObject == null || gameObject.Equals(null);
    }
}
