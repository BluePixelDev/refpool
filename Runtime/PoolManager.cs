using System.Collections.Generic;
using UnityEngine;

namespace BP.PoolIO
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;
        public static PoolManager Instance => GetOrCreateInstance();

        private readonly Dictionary<PoolResource, IPool> registeredPools = new();
        private static PoolManager GetOrCreateInstance()
        {
            if (instance != null) return instance;

            var newGameObj = new GameObject("Pool Manager");
            var newPoolManager = newGameObj.AddComponent<PoolManager>();
            return instance = newPoolManager;
        }

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void RegisterPool(PoolResource descriptor, IPool pool)
        {
            if (registeredPools.ContainsKey(descriptor)) return;
            registeredPools.Add(descriptor, pool);
        }

        public bool TryGetPool(PoolResource descriptor, out IPool pool)
        {
            return registeredPools.TryGetValue(descriptor, out pool);
        }

        public IPool GetPool(PoolResource descriptor)
        {
            return registeredPools.TryGetValue(descriptor, out var pool) ? pool : null;
        }

        public bool RemovePool(PoolResource descriptor)
        {
            return registeredPools.Remove(descriptor);
        }
    }
}
