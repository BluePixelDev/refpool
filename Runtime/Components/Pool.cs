using System.Collections.Generic;
using UnityEngine;

namespace BP.RefPool
{
    /// <summary>
    /// A basic GameObject pool that supports initialization, reuse, and optional persistence across scenes.
    /// </summary>
    public class Pool : PoolComponent
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initSize = 10;
        [SerializeField] private int maxSize = 50;
        [SerializeField] private bool reuseObjects = false;
        [SerializeField] private bool dontDestroyOnLoad = false;

        /// <summary>
        /// The prefab used for pooled instances.
        /// </summary>
        public GameObject Pooled { get => prefab; set => prefab = value; }

        /// <summary>
        /// If true, old objects will be reused when the pool exceeds its maximum size.
        /// </summary>
        public bool ReuseObjects { get => reuseObjects; set => reuseObjects = value; }

        /// <summary>
        /// Initial number of objects to populate in the pool.
        /// </summary>
        public int InitSize { get => initSize; set => initSize = value; }

        /// <summary>
        /// Maximum number of objects allowed in the pool.
        /// </summary>
        public int MaxSize { get => maxSize; set => maxSize = value; }

        private readonly Queue<GameObject> availableQueue = new();
        private readonly LinkedList<GameObject> usedList = new();
        private bool isInitialized;

        private void Start() => Initialize();

        /// <summary>
        /// Configures this pool using the data from a <see cref="PoolAsset"/>.
        /// </summary>
        public void SetAsset(PoolAsset asset)
        {
            prefab = asset.Prefab;
            initSize = asset.InitSize;
            maxSize = asset.MaxSize;
            reuseObjects = asset.ReuseObjects;
            dontDestroyOnLoad = asset.IsPersistent;
        }

        /// <summary>
        /// Initializes the pool by instantiating the initial objects.
        /// </summary>
        public override void Initialize()
        {
            if (isInitialized) return;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            for (int i = 0; i < initSize; i++)
            {
                var item = CreatePooledObject();
                availableQueue.Enqueue(item);
            }

            isInitialized = true;
        }

        /// <summary>
        /// Retrieves a GameObject from the pool.
        /// </summary>
        public override GameObject Get()
        {
            GameObject item;

            if (availableQueue.Count > 0)
            {
                item = availableQueue.Dequeue();
            }
            else if (usedList.Count < maxSize)
            {
                item = CreatePooledObject();
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

        /// <summary>
        /// Returns a GameObject to the pool, if it was previously retrieved.
        /// </summary>
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

        /// <summary>
        /// Ensures this pool persists across scene loads.
        /// </summary>
        public override void MakePersistent()
        {
            if (!dontDestroyOnLoad)
            {
                dontDestroyOnLoad = true;
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Prepares a reused object by resetting it and disabling it.
        /// </summary>
        private void PrepareForReuse(GameObject pooledObject)
        {
            if (pooledObject.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnReuse();
            }

            pooledObject.SetActive(false);
        }

        /// <summary>
        /// Instantiates and sets up a new pooled object.
        /// </summary>
        private GameObject CreatePooledObject()
        {
            var go = Instantiate(prefab);
            go.name = $"Pooled_{prefab.name}";
            go.AddComponent<PoolObject>();
            go.SetActive(false);
            go.transform.SetParent(transform);
            return go;
        }
    }
}
