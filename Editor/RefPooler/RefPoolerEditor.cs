using UnityEditor;
using UnityEditor.UIElements;
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
            var maxSizeProp = serializedObject.FindProperty("maxSize");
            var initSizeProp = serializedObject.FindProperty("initSize");

            initSizeSlider.highValue = maxSizeProp.intValue;
            initSizeSlider.TrackPropertyValue(maxSizeProp, (prop) =>
            {
                initSizeSlider.highValue = maxSizeProp.intValue;
            });

            runtimeStats = root.Q("runtime-stats");
            usageProgressBar = runtimeStats.Q<ProgressBar>("usage-progress");
            capacityProgressBar = runtimeStats.Q<ProgressBar>("capacity-progress");

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

            int usedCount = refPooler.UsedCount;
            int currentSize = refPooler.CurrentSize;
            int maxSize = refPooler.MaxSize;

            usageProgressBar.value = usedCount / (float)currentSize;
            usageProgressBar.title = $"{usedCount}/{currentSize}";

            // Capacity
            capacityProgressBar.value = currentSize / (float)maxSize;
            capacityProgressBar.title = $"{currentSize}/{maxSize}";
        }

        private void UpdateStatsVisibility(bool show)
        {
            runtimeStats.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
