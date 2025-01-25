using UnityEngine;

namespace BP.PoolIO
{
    public enum PoolObjectMode
    {
        Time,
        Disable
    }
    public class PoolObject : MonoBehaviour
    {
        [SerializeField] private PoolObjectMode mode;
        [SerializeField] private float time;

        private float timer;
        private void Update()
        {

        }
    }
}
