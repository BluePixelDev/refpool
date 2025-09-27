using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefResource : ScriptableObject
    {
        public abstract void Prepare();
        public abstract RefItem Get();

        public abstract RefComponent CreateComponent();
        public abstract bool HasDependencyOn(RefResource resource);
    }
}
