using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BP.RefPool.Editor
{
    [CustomEditor(typeof(RefPooler))]
    public class RefPoolerEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset treeAsset;
        private VisualElement runtimeStats;
        private ProgressBar usageProgressBar;
        private ProgressBar capacityProgressBar;
        private RefPooler refPooler;

        private void Awake()
        {
            refPooler = (RefPooler)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            treeAsset.CloneTree(root);

            var initSizeSlider = root.Q<SliderInt>("init-size");
            var helpBox = root.Q<HelpBox>("help-box");
            runtimeStats = root.Q("runtime-stats");
            usageProgressBar = runtimeStats.Q<ProgressBar>("usage-progress");
            capacityProgressBar = runtimeStats.Q<ProgressBar>("capacity-progress");

            var maxSizeProp = serializedObject.FindProperty("maxSize");
            var initSizeProp = serializedObject.FindProperty("initSize");
            var prefabProp = serializedObject.FindProperty("prefab");

            initSizeSlider.PropertyAction(initSizeProp, (prop) =>
            {
                initSizeSlider.highValue = prop.intValue;
            });

            helpBox.TrackVisibilityBasedOnProperty(prefabProp, (prop) => prop.boxedValue == null);

            UpdateStatsVisibility(Application.isPlaying);
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;

            EditorApplication.update -= Update;
            EditorApplication.update += Update;

            return root;
        }

        private void PlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.EnteredEditMode)
            {
                UpdateStatsVisibility(EditorApplication.isPlaying);
            }
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            runtimeStats.MarkDirtyRepaint();

            int currentCount = refPooler.Count;
            int useCount = refPooler.UsedCount;
            int maxCount = refPooler.MaxSize;

            capacityProgressBar.value = currentCount / (float)maxCount;
            capacityProgressBar.title = $"Capacity: {currentCount}/{maxCount}";

            usageProgressBar.value = useCount / (float)currentCount;
            usageProgressBar.title = $"Usage: {useCount} / {currentCount}";
        }

        private void UpdateStatsVisibility(bool show)
        {
            runtimeStats.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
