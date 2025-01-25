using System.Collections.Generic;
using UnityEngine;

namespace BP.PoolIO
{
    public class PoolGroup : PoolComponent
    {
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private List<PoolComponent> pools = new();

        private int seqIndex;
        private int backIndex;
        private int backDir = 1;

        private void OnValidate()
        {
            HashSet<PoolComponent> uniquePools = new();
            for (int i = 0; i < pools.Count; i++)
            {
                var poolComp = pools[i];

                if (uniquePools.Contains(poolComp))
                {
                    pools.RemoveAt(i);
                    continue;
                }

                if (poolComp is PoolGroup poolGroup && poolGroup.ContainsPool(this))
                {
                    Debug.LogError($"Removed {poolComp.name} from {name}: cyclic reference detected.");
                    pools.RemoveAt(i);
                    continue;
                }

                uniquePools.Add(poolComp);
            }
        }

        public void SetAsset(PoolGroupAsset poolGroupAsset)
        {
            pickMode = poolGroupAsset.PickMode;
        }

        public void AddPool(PoolComponent pool)
        {
            if (pools.Contains(pool)) return;

            if (pool is PoolGroup poolGroup && poolGroup.ContainsPool(this))
            {
                Debug.LogError($"Cannot add {pool.name} to {name}: it would create a cyclic reference.");
                return;
            }

            pools.Add(pool);
        }
        public bool RemovePool(PoolComponent pool)
        {
            return pools.Remove(pool);
        }
        public bool ContainsPool(PoolComponent targetPool)
        {
            if (pools.Contains(targetPool)) return true;

            foreach (var pool in pools)
            {
                if (pool is PoolGroup group && group.ContainsPool(targetPool))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Init()
        {
            foreach (var pool in pools)
            {
                if (pool == null) continue;
                pool.Init();
            }
        }
        public override GameObject Get()
        {
            return pickMode switch
            {
                PoolPickMode.Random => GetRandom(),
                PoolPickMode.Sequential => GetSequential(),
                PoolPickMode.Back2Back => GetBack2Back(),
                _ => GetRandom()
            };
        }

        private GameObject GetRandom()
        {
            var randInt = Random.Range(0, pools.Count);
            return pools[randInt].Get();
        }
        private GameObject GetSequential()
        {
            var pool = pools[seqIndex];
            seqIndex = (seqIndex + 1) % pools.Count;
            return pool.Get();
        }
        private GameObject GetBack2Back()
        {
            var pool = pools[backIndex];
            backIndex += backDir;
            if (backIndex >= pools.Count || backIndex < 0)
            {
                backDir = -backDir;
            }
            return pool.Get();
        }

        public override bool Release(GameObject gameObject)
        {
            foreach (var pool in pools)
            {
                if (pool.Release(gameObject))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
