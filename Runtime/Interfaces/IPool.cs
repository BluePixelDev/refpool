using UnityEngine;

namespace BP.PoolIO
{
    public interface IPool
    {
        /// <summary>
        /// Retrieves a GameObject from the pool group.
        /// </summary>
        /// <returns>The pooled GameObject.</returns>
        GameObject Get();


        /// <summary>
        /// Releases a GameObject back into the pool group.
        /// </summary>
        /// <param name="gameObject">The GameObject to release.</param>
        /// <returns>True if the object was successfully released; otherwise, false.</returns>
        bool Release(GameObject gameObject);
    }
}
