using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BP.RefPool.Editor
{
    [CustomEditor(typeof(PoolAsset))]
    public class PoolAssetEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            treeAsset.CloneTree(root);
            return root;
        }
    }
}
