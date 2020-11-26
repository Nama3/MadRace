#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class NoLabelDrawer : BasePropertyDrawer<NoLabelAttribute>
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, NoLabelAttribute attribute) => 
            MightyGUIUtilities.DrawPropertyField(property, GUIContent.none);

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, NoLabelAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif