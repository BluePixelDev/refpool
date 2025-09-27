using UnityEngine;

namespace BP.RefPool
{
    public abstract class RefComponent : MonoBehaviour
    {
        public abstract void Prepare();
        public abstract RefItem Get();
    }
}
