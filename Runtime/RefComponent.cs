using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefComponent : MonoBehaviour
    {
        public abstract void Prepare();
        public abstract RefItem Get();

        public abstract bool HasDependencyOn(RefComponent refComponent);
    }
}
