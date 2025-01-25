using UnityEngine;

namespace BP.PoolIO
{
    public interface IPooled
    {
        /// <summary>
        /// Called upon release.
        /// </summary>
        void OnRelease();

        /// <summary>
        /// Called upon being acquired.
        /// </summary>
        void OnAcquire();
    }
}
