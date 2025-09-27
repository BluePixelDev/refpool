using System.Collections.Generic;
using UnityEngine;

namespace BP.RefPool
{
    public class RefPoolerGroup : RefComponent
    {
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private List<RefComponent> pools = new();

        public PoolPickMode PickMode { get => pickMode; set => pickMode = value; }

        private int seqIndex;
        private int backIndex;
        private int backDir = 1;
        private bool isInitialized;

        private void OnValidate()
        {
            for (int i = pools.Count - 1; i >= 0; i--)
            {
                var poolComp = pools[i];
                if (poolComp == null)
                    continue;

                if (poolComp == this)
                {
                    pools.RemoveAt(i);
                    continue;
                }
            }
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

        public void Add(RefComponent pool)
        {
            if (pools.Contains(pool)) return;
            pools.Add(pool);
        }
        public bool Remove(RefComponent pool) => pools.Remove(pool);
        public bool Contains(RefComponent targetPool) => pools.Contains(targetPool);
        public override RefItem Get()
        {
            if (pools.Count == 0)
            {
                Debug.LogWarning($"[RefPoolerGroup] No pools available in '{name}' to get from.");
                return null;
            }

            RefComponent selectedPool = null;
            switch (pickMode)
            {
                case PoolPickMode.Random:
                    int randomIndex = Random.Range(0, pools.Count);
                    selectedPool = pools[randomIndex];
                    break;

                case PoolPickMode.Sequential:
                    selectedPool = pools[seqIndex];
                    seqIndex = (seqIndex + 1) % pools.Count;
                    break;

                case PoolPickMode.Back2Back:
                    selectedPool = pools[backIndex];
                    backIndex += backDir;

                    if (backIndex >= pools.Count || backIndex < 0)
                    {
                        backDir = -backDir;
                        backIndex += backDir * 2;
                        backIndex = Mathf.Clamp(backIndex, 0, pools.Count - 1);
                    }
                    break;
            }
            return selectedPool.Get();
        }
    }
}
