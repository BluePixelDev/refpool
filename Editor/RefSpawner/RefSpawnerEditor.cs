using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BP.RefPool.Editor
{
    [CustomEditor(typeof(RefSpawner))]
    public class RefSpawnerEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            treeAsset.CloneTree(root);

            var spawnOptionsElm = root.Q<VisualElement>("spawn-options");
            var helpBoxElm = root.Q<HelpBox>("help-box");

            var autoSpawnProp = serializedObject.FindProperty("autoSpawn");
            var refResourceProp = serializedObject.FindProperty("refResource");

            spawnOptionsElm.TrackVisibilityBasedOnProperty(autoSpawnProp, (prop) => prop.boolValue);
            helpBoxElm.TrackVisibilityBasedOnProperty(refResourceProp, (prop) => prop.boxedValue == null);

            return root;
        }
    }
}
