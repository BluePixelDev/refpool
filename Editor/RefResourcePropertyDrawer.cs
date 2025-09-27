using UnityEditor;
using UnityEngine;

namespace BP.RefPool.Editor
{
    [CustomPropertyDrawer(typeof(RefResource), true)]
    public class RefResourcePropertyDrawer : PropertyDrawer
    {
        private const float ButtonWidth = 20f;
        private const float VerticalSpacing = 2f;
        private const float IndentSpacing = 15f;
        private const float BoxPadding = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var fieldRect = new Rect(position.x, position.y, position.width - ButtonWidth, EditorGUIUtility.singleLineHeight);
            var buttonRect = new Rect(position.x + position.width - ButtonWidth, position.y, ButtonWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(fieldRect, property, label, false);

            GUIStyle iconStyle = new(GUI.skin.button)
            {
                padding = new RectOffset(2, 2, 2, 2),
                margin = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter
            };

            GUI.enabled = property.boxedValue != null;
            var buttonIcon = EditorGUIUtility.IconContent(property.isExpanded ? "d_SceneViewVisibility@2x" : "d_scenevis_visible_hover@2x");
            if (GUI.Button(buttonRect, buttonIcon, iconStyle))
                property.isExpanded = !property.isExpanded;
            GUI.enabled = true;

            if (property.isExpanded && property.boxedValue is Object target)
            {
                var serialized = new SerializedObject(target);
                var iterator = serialized.GetIterator();

                Rect boxRect = new(
                    position.x,
                    fieldRect.yMax + VerticalSpacing,
                    position.width,
                    GetBoxHeight(serialized)
                );
                EditorGUI.HelpBox(boxRect, "", MessageType.None);

                float y = boxRect.y + BoxPadding;
                iterator.NextVisible(true);
                while (iterator.NextVisible(false))
                {
                    float h = EditorGUI.GetPropertyHeight(iterator, true);
                    var r = new Rect(position.x + IndentSpacing, y, position.width - IndentSpacing, h);

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.PropertyField(r, iterator, true);
                    EditorGUI.EndDisabledGroup();

                    y += h + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded && property.boxedValue is Object target)
            {
                SerializedObject serialized = new(target);
                float boxHeight = GetBoxHeight(serialized);
                height += VerticalSpacing + boxHeight;
            }

            return height;
        }

        private float GetBoxHeight(SerializedObject serialized)
        {
            float height = 0f;
            SerializedProperty iterator = serialized.GetIterator();
            iterator.NextVisible(true);

            while (iterator.NextVisible(false))
            {
                height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height + BoxPadding * 2;
        }
    }
}
