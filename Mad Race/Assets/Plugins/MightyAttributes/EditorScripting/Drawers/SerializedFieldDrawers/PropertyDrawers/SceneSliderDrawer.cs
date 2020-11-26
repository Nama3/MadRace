#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MightyAttributes.Editor
{
    public class SceneSliderDrawer : BasePropertyDrawer<SceneSliderAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, SceneSliderAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawSlider(property, attribute.Options);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField.GetElement(index), baseAttribute.Options);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) => DrawSlider(serializedField.GetElement(index), baseAttribute.Options, label);
        
        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawSlider(position, serializedField.GetElement(index), baseAttribute.Options);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            serializedField.GetElement(index).propertyType != SerializedPropertyType.Integer
                ? MightyGUIUtilities.WARNING_HEIGHT
                : (MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING) * 2;

        public void DrawSlider(SerializedProperty property, FieldOption option, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{nameof(SceneSliderAttribute)} can be used only on int fields");
                return;
            }

            var max = SceneManager.sceneCountInBuildSettings - 1;

            property.intValue = option.Contains(FieldOption.HideLabel)
                ? EditorGUILayout.IntSlider(property.intValue, 0, max)
                : EditorGUILayout.IntSlider(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue, 0, max);

            EditorGUILayout.LabelField(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(property.intValue)),
                EditorStyles.boldLabel);
        }

        public void DrawSlider(Rect position, SerializedProperty property, FieldOption option)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{nameof(SceneSliderAttribute)} can be used only on int fields");
                return;
            }

            var max = SceneManager.sceneCountInBuildSettings - 1;

            position.height = MightyGUIUtilities.FIELD_HEIGHT;
            
            property.intValue = option.Contains(FieldOption.HideLabel)
                ? EditorGUI.IntSlider(position, property.intValue, 0, max)
                : EditorGUI.IntSlider(position, property.displayName, property.intValue, 0, max);

            position = MightyGUIUtilities.JumpLine(position, false);

            EditorGUI.LabelField(position, Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(property.intValue)),
                EditorStyles.boldLabel);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, SceneSliderAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif