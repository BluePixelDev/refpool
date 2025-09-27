using UnityEditor;
using UnityEditor.UIElements;
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

            var spawnOptions = root.Q<VisualElement>("spawn-options");
            var autoSpawnProp = serializedObject.FindProperty("autoSpawn");

            UpdateSpawnSettingsVisibility();
            spawnOptions.TrackPropertyValue(autoSpawnProp, (prop) => UpdateSpawnSettingsVisibility());

            void UpdateSpawnSettingsVisibility()
            {
                spawnOptions.style.display = autoSpawnProp.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            }

            return root;
        }
    }
}
