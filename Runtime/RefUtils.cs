using UnityEngine;

namespace BP.RefPool
{
    internal static class RefUtils
    {
        const string POOL_ITEM_NAME_PREFIX = "[PI]";
        public static RefItem CreatePooledItem(RefPooler pooler)
        {
            if (pooler == null) return null;
            if (pooler.Prefab == null) return null;

            var prefab = pooler.Prefab;
            var go = Object.Instantiate(prefab);
            go.name = $"{POOL_ITEM_NAME_PREFIX}{prefab.name}";

            if (!go.TryGetComponent<RefItem>(out var refItem))
            {
                refItem = AddRefItem(go, pooler);
            }
            else
            {
                refItem.SetOwner(pooler);
            }
            // Avoids setting parent in build to save on performance
#if UNITY_EDITOR
            go.transform.SetParent(pooler.transform);
#endif
            return refItem;
        }

        public static RefItem AddRefItem(GameObject gameObject, RefPooler pooler)
        {
            var refItem = gameObject.AddComponent<RefItem>();
            refItem.SetOwner(pooler);
            return refItem;
        }

        public static string FormatName(string name, string prefix) => string.IsNullOrEmpty(name) ? $"{prefix}Unnamed" : $"{prefix}{name}";
        public static bool IsUnityNull(this object gameObject) => gameObject == null || gameObject.Equals(null);


        public static float ClampMin(float value, float min)
        {
            if (value < min) return min;
            return value;
        }
        public static int ClampMin(int value, int min)
        {
            if (value < min) return min;
            return value;
        }
    }
}
