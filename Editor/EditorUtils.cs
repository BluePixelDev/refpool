using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BP.RefPool.Editor
{
    public static class EditorUtils
    {
        public static void TrackVisibilityBasedOnProperty(
           this VisualElement element,
           SerializedProperty property,
           System.Func<SerializedProperty, bool> condition
        )
        {
            element.style.display = condition(property) ? DisplayStyle.Flex : DisplayStyle.None;
            element.TrackPropertyValue(property, val =>
                element.style.display = condition(val) ? DisplayStyle.Flex : DisplayStyle.None
            );
        }

        public static void PropertyAction(
            this VisualElement element,
            SerializedProperty property,
            System.Action<SerializedProperty> action
        )
        {
            action(property);
            element.TrackPropertyValue(property, action);
        }
    }
}
