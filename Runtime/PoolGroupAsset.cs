using System.Collections.Generic;
using UnityEngine;

namespace BP.RefPool
{
    public enum PoolPickMode
    {
        Random,
        Sequential,
        Back2Back
    }

    [CreateAssetMenu(fileName = "PoolGroup", menuName = "RefPool/Group")]
    public class PoolGroupAsset : RefResource
    {
        const string POOLGROUP_NAME_PREFIX = "[PG]";

        [SerializeField] private string displayName = "New Group";
        [SerializeField] private PoolPickMode pickMode = PoolPickMode.Random;
        [SerializeField] private List<RefResource> pools = new();

        // Runtime Reference
        [HideInInspector] private RefComponent groupRef;

        private void OnValidate()
        {
            for (int i = pools.Count - 1; i >= 0; i--)
            {
                var poolRes = pools[i];
                if (poolRes == null)
                    continue;

                if (poolRes == this)
                {
                    pools.RemoveAt(i);
                    Debug.LogWarning("[PoolGroupAsset] Cannot assign itself as pool entry");
                    continue;
                }

                if (poolRes.HasDependencyOn(this))
                {
                    pools.RemoveAt(i);
                    Debug.LogWarning("[PoolGroupAsset] Cyclic dependency detected, removing!");
                    continue;
                }
            }
        }

        public override RefItem Get()
        {
            Prepare();
            return groupRef.Get();
        }
        public override void Prepare()
        {
            if (RefUtils.IsUnityNull(groupRef))
            {
                groupRef = CreateComponent();
            }
        }
        public override RefComponent CreateComponent()
        {
            var groupName = RefUtils.FormatName(displayName, POOLGROUP_NAME_PREFIX);
            var gameObject = new GameObject(groupName);
            var groupComponent = gameObject.AddComponent<RefPoolerGroup>();
            groupComponent.PickMode = pickMode;

            foreach (var poolAsset in pools)
            {
                if (poolAsset == this)
                {
                    Debug.LogWarning("Recursion detected: skipping");
                    continue;
                }
                var newRefComponent = poolAsset.CreateComponent();
                groupComponent.Add(newRefComponent);

#if UNITY_EDITOR
                newRefComponent.transform.SetParent(gameObject.transform);
#endif
            }
            return groupComponent;
        }

        public override bool HasDependencyOn(RefResource resource)
        {
            foreach (var poolAsset in pools)
            {
                if (poolAsset == resource) return true;
                if (poolAsset.HasDependencyOn(resource))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
