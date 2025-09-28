using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[assembly: InternalsVisibleTo("BP.RefPool.Tests")]
namespace BP.RefPool
{
    [DisallowMultipleComponent]
    [AddComponentMenu("RefPool/Ref Item")]
    public class RefItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent onUse;
        public event Action Used;

        private IReleasable releasable;
        internal IReleasable Releasable => releasable;

        private bool isUsed;
        public bool IsUsed => isUsed;

        internal void SetOwner(IReleasable owner)
        {
            releasable = owner;
        }

        internal void Use()
        {
            isUsed = true;
            SetActive(true);
            onUse?.Invoke();
            Used?.Invoke();
        }

        public void Release()
        {
            if (!isUsed) return;
            releasable.Release(this);
            isUsed = false;
            SetActive(false);
        }

        internal void SetActive(bool active) => gameObject.SetActive(active);
    }
}
