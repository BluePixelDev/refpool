using System.Collections.Generic;
using UnityEngine;

namespace BP.RefPool
{
    public class RefPooler : RefComponent, IResourceApplier<PoolAsset>
    {
        [SerializeField] private GameObject prefab;
        [SerializeField, Min(0)] private int maxSize = 50;
        [SerializeField, Min(0)] private int initSize = 10;
        [SerializeField] private bool reuseObjects = false;
        [SerializeField] private bool isPersistent = false;

        public GameObject Prefab { get => prefab; set => prefab = value; }
        public bool ReuseObjects { get => reuseObjects; set => reuseObjects = value; }
        public int InitSize { get => initSize; set => initSize = value; }
        public int MaxSize { get => maxSize; set => maxSize = value; }

        private readonly Queue<RefItem> availableQueue = new();
        private readonly LinkedList<RefItem> usedList = new();
        private bool isInitialized;

        public int AvalibleCount => availableQueue.Count;
        public int UsedCount => usedList.Count;
        public int CurrentSize => AvalibleCount + UsedCount;

        private void Start() => Initialize();

        private void OnValidate()
        {
            maxSize = Mathf.Clamp(maxSize, 0, int.MaxValue);
            initSize = Mathf.Clamp(initSize, 0, maxSize);
        }

        public void ApplyResource(PoolAsset asset)
        {
            prefab = asset.Prefab;
            initSize = asset.InitSize;
            maxSize = asset.MaxSize;
            reuseObjects = asset.ReuseObjects;
            isPersistent = asset.IsPersistent;
        }

        public override void Initialize()
        {
            if (isInitialized) return;

            if (isPersistent)
                DontDestroyOnLoad(gameObject);

            initSize = Mathf.Clamp(initSize, 0, maxSize);
            for (int i = 0; i < initSize; i++)
            {
                var item = RefUtils.CreatePooledItem(this);
                availableQueue.Enqueue(item);
            }
            isInitialized = true;
        }

        public override RefItem Get()
        {
            RefItem refItem;
            if (availableQueue.Count > 0)
            {
                refItem = availableQueue.Dequeue();
            }
            else if (CurrentSize < maxSize)
            {
                refItem = RefUtils.CreatePooledItem(this);
            }
            else if (reuseObjects && usedList.Count > 0)
            {
                refItem = usedList.Last.Value;
                usedList.RemoveLast();
            }
            else
            {
                return null;
            }

            refItem.ResetState();
            refItem.SetActive(true);
            refItem.isUsed = true;
            usedList.AddFirst(refItem);
            return refItem;
        }

        public override bool Release(RefItem refItem)
        {
            if (!refItem.isUsed) return false;
            if (refItem.OwnerPool.Equals(this))
            {
                refItem.SetActive(false);
                refItem.isUsed = false;

                usedList.Remove(refItem);
                availableQueue.Enqueue(refItem);
                return true;
            }
            return false;
        }

        public override void MakePersistent()
        {
            if (!isPersistent)
            {
                isPersistent = true;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
