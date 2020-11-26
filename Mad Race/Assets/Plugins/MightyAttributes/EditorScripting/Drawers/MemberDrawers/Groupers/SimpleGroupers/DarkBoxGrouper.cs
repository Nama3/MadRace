#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class DarkBoxGrouper : BaseSimpleGrouper<DarkBoxGroupAttribute>
    {
        public override void BeginDrawGroup(int indentLevel = 0, string groupID = null, bool indentInside = true)
        {
            GUILayout.BeginVertical(MightyStyleUtilities.GetDarkBox(indentLevel));

            if (indentInside)
                EditorGUI.indentLevel = 1;
        }

        public override void EndDrawGroup(int indentLevel = 0)
        {
            EditorGUI.indentLevel = indentLevel;
            GUILayout.EndVertical();
        }
        
        // ReSharper disable once RedundantOverriddenMember
        // ReSharper disable once OptionalParameterHierarchyMismatch
        public override void DrawLine(ColorValue color = ColorValue.HardContrast) => base.DrawLine(color);
    }
}
#endif