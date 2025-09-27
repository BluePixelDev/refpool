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

        private void Awake()
        {
            refItem = GetComponent<RefItem>();

            if (!refItem) return;
            refItem.Used += PoolItem_Reset;
        }

        private void OnDestroy()
        {
            if (!refItem) return;
            refItem.Used -= PoolItem_Reset;
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
                refItem.Release();
            }
        }
    }
}
