using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefComponent : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract RefItem Get();
    }
}
