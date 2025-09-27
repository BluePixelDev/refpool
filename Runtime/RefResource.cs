using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefResource : ScriptableObject
    {
        public abstract void Prepare();
        public abstract RefItem Get();

        public abstract bool IsContainedIn(RefResource resource);
        public abstract RefComponent CreateComponent();
    }
}
