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
        [SerializeField] private UnityEvent onReset;
        public event Action Reset;

        private IPool ownerPool;
        internal IPool OwnerPool => ownerPool;

        internal bool isUsed;

        internal void SetOwner(IPool ownerPool)
        {
            this.ownerPool = ownerPool;
        }

        public void ResetState()
        {
            onReset?.Invoke();
            Reset?.Invoke();
        }

        public void SetActive(bool active)
        {
            if (gameObject.activeSelf != active)
                gameObject.SetActive(active);
        }
    }
}
