using System.Collections.Generic;
using UnityEngine;

namespace BP.PoolIO
{
    /// <summary>
    /// A simple pool implementation.
    /// </summary>
    public class Pool : PoolComponent
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize;
        [SerializeField] private bool reuseObjects;
        [SerializeField] private bool dontDestroyOnLoad;

        public GameObject Pooled { get => prefab; set => prefab = value; }
        public bool ReuseObjects { get => reuseObjects; set => reuseObjects = value; }
        public int InitSize { get => initSize; set => initSize = value; }

        private readonly Queue<GameObject> availableQueue = new();
        private readonly LinkedList<GameObject> usedList = new();

        public void SetAsset(PoolAsset descriptor)
        {
            prefab = descriptor.Prefab;
            reuseObjects = descriptor.ReuseObjects;
            initSize = descriptor.InitSize;
            dontDestroyOnLoad = descriptor.IsPersistant;
        }

        private void Start()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            for (var i = 0; i < initSize; i++)
            {
                var item = CreatePooledObject();
                availableQueue.Enqueue(item);
            }
        }

        public override GameObject Get()
        {
            GameObject item;
            if (availableQueue.Count > 0)
            {
                item = availableQueue.Dequeue();
            }
            else if (reuseObjects && usedList.Count > 0)
            {
                item = usedList.Last.Value;
                usedList.RemoveLast();
                PrepareForReuse(item);
            }
            else
            {
                item = CreatePooledObject();
            }

            usedList.AddFirst(item);
            return item;
        }
        private void PrepareForReuse(GameObject pooledObject)
        {
            if (pooledObject.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnReuse();
            }

            pooledObject.SetActive(false);
        }

        private GameObject CreatePooledObject()
        {
            var go = Instantiate(prefab);
            go.name = $"Pooled_{prefab.name}";
            go.AddComponent<PoolObject>();
            go.SetActive(false);
            go.transform.SetParent(transform);
            return go;
        }
        public override bool Release(GameObject gameObject)
        {
            if (usedList.Contains(gameObject))
            {
                usedList.Remove(gameObject);
                availableQueue.Enqueue(gameObject);
                return true;
            }
            return false;
        }

        public override void MakePersistent()
        {
            if (!dontDestroyOnLoad)
            {
                dontDestroyOnLoad = true;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
