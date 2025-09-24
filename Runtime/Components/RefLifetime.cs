using UnityEngine;

namespace BP.RefPool
{
    [AddComponentMenu("RefPool/Ref Lifetime")]
    [RequireComponent(typeof(RefItem))]
    public class RefLifetime : MonoBehaviour
    {
        [SerializeField] private float lifetime = 5;

        private RefItem refItem;
        private float lifetimeTimer;

        private void OnEnable()
        {
            refItem = GetComponent<RefItem>();

            if (!refItem) return;
            refItem.Reset += PoolItem_Reset;
        }

        private void OnDestroy()
        {
            if (!refItem) return;
            refItem.Reset -= PoolItem_Reset;
        }

        private void PoolItem_Reset()
        {
            lifetimeTimer = lifetime;
        }

        private void Update()
        {
            if (!refItem) return;

            lifetimeTimer -= Time.deltaTime;
            if (lifetimeTimer <= 0)
            {
                refItem.OwnerPool.Release(refItem);
            }
        }
    }
}
