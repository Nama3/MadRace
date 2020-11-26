#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LargeLabelDecoratorDrawer : BaseLabelDecoratorDrawer<LargeLabelAttribute>
    {
        protected override void DrawLabel(BaseMightyMember mightyMember, LargeLabelAttribute attribute, string prefix, string label)
        {
            if (string.IsNullOrEmpty(prefix))
                EditorGUILayout.LabelField(label, EditorStyles.largeLabel, GUILayout.Height(20),
                    GUILayout.Width(MightyGUIUtilities.TextWidth(label) + MightyGUIUtilities.TAB_SIZE));
            else
                EditorGUILayout.LabelField(prefix, label, EditorStyles.largeLabel, GUILayout.Height(20));
        }
    }
}
#endif