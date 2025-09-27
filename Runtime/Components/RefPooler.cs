using System.Collections.Generic;
using UnityEngine;

namespace BP.RefPool
{
    public class RefPooler : RefComponent, IReleasable
    {
        [SerializeField] private GameObject prefab;
        [SerializeField, Min(0)] private int maxSize = 50;
        [SerializeField, Min(0)] private int initSize = 10;
        [SerializeField] private bool reuseObjects = false;

        public GameObject Prefab { get => prefab; set => prefab = value; }
        public bool ReuseObjects { get => reuseObjects; set => reuseObjects = value; }
        public int InitSize { get => initSize; set => initSize = value; }
        public int MaxSize { get => maxSize; set => maxSize = value; }

        private readonly Queue<RefItem> availibleItems = new();
        private readonly LinkedList<RefItem> usedItems = new();
        private bool isInitialized;

        public int Count => availibleItems.Count + usedItems.Count;
        public int UsedCount => usedItems.Count;
        public int AvailibleCount => availibleItems.Count;

        private void Start() => Initialize();

        private void OnValidate()
        {
            maxSize = Mathf.Clamp(maxSize, 0, int.MaxValue);
            initSize = Mathf.Clamp(initSize, 0, maxSize);
        }

        public override void Initialize()
        {
            if (isInitialized) return;
            if (prefab == null) return;

            initSize = Mathf.Clamp(initSize, 0, maxSize);
            for (int i = 0; i < initSize; i++)
            {
                var item = RefUtils.CreatePooledItem(this);
                item.SetActive(false);
                availibleItems.Enqueue(item);
            }
            isInitialized = true;
        }

        public override RefItem Get()
        {
            RefItem refItem = GetAvailibleItem();
            if (refItem == null) return null;
            usedItems.AddFirst(refItem);
            refItem.Use();
            return refItem;
        }

        private RefItem GetAvailibleItem()
        {
            if (availibleItems.Count > 0)
            {
                return availibleItems.Dequeue();
            }

            if (Count < MaxSize)
            {
                var newItem = RefUtils.CreatePooledItem(this);
                return newItem;
            }

            if (reuseObjects)
            {
                var lastNode = usedItems.Last;
                usedItems.RemoveLast();
                return lastNode.Value;
            }

            return null;
        }
        public bool Release(RefItem item)
        {
            if (item == null) return false;
            if (!item.IsUsed) return false;
            if (!item.Releasable.Equals(this)) return false;
            usedItems.Remove(item);
            availibleItems.Enqueue(item);
            return true;
        }
    }
}
