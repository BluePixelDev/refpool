using System.Collections.Generic;
using UnityEngine;

namespace BP.RefPool
{
    public class RefPoolerGroup : RefComponent, IResourceApplier<PoolGroupAsset>
    {
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private bool isPersistent;
        [SerializeField] private List<RefComponent> pools = new();

        private int seqIndex;
        private int backIndex;
        private int backDir = 1;
        private bool isInitialized;

        private void OnValidate()
        {
            HashSet<RefComponent> uniquePools = new();

            for (int i = pools.Count - 1; i >= 0; i--)
            {
                var poolComp = pools[i];
                if (poolComp == null)
                {
                    pools.RemoveAt(i);
                    continue;
                }

                if (uniquePools.Contains(poolComp))
                {
                    pools.RemoveAt(i);
                    continue;
                }

                if (poolComp is RefPoolerGroup poolGroup && poolGroup.ContainsPool(this))
                {
                    Debug.LogError($"Removed {poolComp.name} from {name}: cyclic reference detected.");
                    pools.RemoveAt(i);
                    continue;
                }

                uniquePools.Add(poolComp);
            }
        }

        public void ApplyResource(PoolGroupAsset asset)
        {
            pickMode = asset.PickMode;
            isPersistent = asset.IsPersistent;
        }

        public void AddPool(RefComponent pool)
        {
            if (pools.Contains(pool)) return;

            if (pool is RefPoolerGroup poolGroup && poolGroup.ContainsPool(this))
            {
                Debug.LogError($"Cannot add {pool.name} to {name}: it would create a cyclic reference.");
                return;
            }

            if (isPersistent) pool.MakePersistent();
            pools.Add(pool);
        }
        public bool RemovePool(RefComponent pool)
        {
            return pools.Remove(pool);
        }
        public bool ContainsPool(RefComponent targetPool)
        {
            if (pools.Contains(targetPool)) return true;

            foreach (var pool in pools)
            {
                if (pool is RefPoolerGroup group && group.ContainsPool(targetPool))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Initialize()
        {
            if (isInitialized) return;
            foreach (var pool in pools)
            {
                if (pool == null) continue;
                pool.Initialize();
            }
            isInitialized = true;
        }
        public override RefItem Get()
        {
            return pickMode switch
            {
                PoolPickMode.Random => GetRandom(),
                PoolPickMode.Sequential => GetSequential(),
                PoolPickMode.Back2Back => GetBack2Back(),
                _ => GetRandom()
            };
        }
        public override bool Release(RefItem item)
        {
            foreach (var pool in pools)
            {
                if (pool.Release(item))
                {
                    return true;
                }
            }

            return false;
        }

        private RefItem GetRandom()
        {
            var randInt = Random.Range(0, pools.Count);
            return pools[randInt].Get();
        }
        private RefItem GetSequential()
        {
            var pool = pools[seqIndex];
            seqIndex = (seqIndex + 1) % pools.Count;
            return pool.Get();
        }
        private RefItem GetBack2Back()
        {
            var pool = pools[backIndex];
            backIndex += backDir;
            if (backIndex >= pools.Count || backIndex < 0)
            {
                backDir = -backDir;
            }
            return pool.Get();
        }
        public override void MakePersistent()
        {
            foreach (var pool in pools)
            {
                pool.MakePersistent();
            }
        }
    }
}
