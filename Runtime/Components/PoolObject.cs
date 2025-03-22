using UnityEngine;
using UnityEngine.Events;

namespace BP.PoolIO
{
    public class PoolObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private UnityEvent Reused;
        public void OnReuse()
        {
            Reused?.Invoke();
        }
    }
}
