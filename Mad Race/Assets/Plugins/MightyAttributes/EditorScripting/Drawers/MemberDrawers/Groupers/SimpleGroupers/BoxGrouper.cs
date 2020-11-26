#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class BoxGrouper : BaseSimpleGrouper<BoxGroupAttribute>
    {
        public override void BeginDrawGroup(int indentLevel = 0, string groupID = null, bool indentInside = true)
        {
            GUILayout.BeginVertical(MightyStyleUtilities.GetBox(indentLevel));

            if (indentInside)
                EditorGUI.indentLevel = 1;
        }

        public override void EndDrawGroup(int indentLevel = 0)
        {
            EditorGUI.indentLevel = indentLevel;
            GUILayout.EndVertical();
        }
    }
}
#endif