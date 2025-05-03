using UnityEngine;

namespace BP.RefPool
{
    /// <summary>
    /// Interface for pool components that manage the lifecycle of <see cref="GameObject"/> instances.
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Initializes the pool component if it has not been initialized yet.
        /// </summary>
        void Initialize();

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
