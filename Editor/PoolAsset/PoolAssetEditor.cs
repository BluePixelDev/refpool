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

            var helpBox = root.Q<HelpBox>("help-box");
            var prefabProp = serializedObject.FindProperty("prefab");

            helpBox.TrackVisibilityBasedOnProperty(prefabProp, (prop) => prop.boxedValue == null);
            return root;
        }
    }
}
