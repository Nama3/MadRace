#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentButtonDrawer : BasePropertyDrawer<GetComponentButtonAttribute>
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            GetComponentButtonAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference ||
                !mightyMember.PropertyType.IsSubclassOf(typeof(Component)))
            {
                MightyGUIUtilities.DrawHelpBox($"{nameof(GetComponentButtonAttribute)} can be used only on Components fields");
                return;
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.ObjectField(property, !attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUIUtility.TrTextContent(property.displayName)
                : GUIContent.none);

            if (GUILayout.Button("Get Component", GUILayout.Width(100)))
            {
                var component = property.GetGameObject().GetComponent(mightyMember.PropertyType);
                if (component != null) property.objectReferenceValue = component;
            }

            EditorGUILayout.EndHorizontal();
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, GetComponentButtonAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif