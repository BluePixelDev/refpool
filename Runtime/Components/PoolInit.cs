using UnityEngine;

namespace BP.PoolIO
{
    public class PoolInit : MonoBehaviour
    {
        [SerializeField] private PoolResource[] pools;

        private void Awake()
        {
            foreach (var pool in pools)
            {
                pool.Init();
            }
        }
    }
}
