using UnityEngine;

namespace BP.PoolIO
{
    public static class PoolUtils
    {
        public static Pool CreatePool(PoolAsset poolAsset)
        {
            var gameObject = new GameObject(poolAsset.PoolName ?? "Unnamed Pool");
            var pool = gameObject.AddComponent<Pool>();
            pool.SetAsset(poolAsset);
            return pool;
        }

        public static PoolGroup CreatePoolGroup(PoolGroupAsset groupAsset)
        {
            var gameObject = new GameObject(groupAsset.GroupName ?? "Unnamed Group");
            var pg = gameObject.AddComponent<PoolGroup>();
            foreach (var pool in groupAsset.Pools)
            {
                pg.AddPool(CreatePool(pool));
            }
            pg.SetAsset(groupAsset);
            return pg;
        }

        public static bool IsNull(this object gameObject) => gameObject == null || gameObject.Equals(null);
    }
}
