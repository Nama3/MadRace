#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class BoldLabelDecoratorDrawer : BaseLabelDecoratorDrawer<BoldLabelAttribute>
    {
        protected override void DrawLabel(BaseMightyMember mightyMember, BoldLabelAttribute attribute, string prefix, string label)
        {
            if (string.IsNullOrEmpty(prefix))
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Width(MightyGUIUtilities.TextWidth(label) + MightyGUIUtilities.TAB_SIZE));
            else
                EditorGUILayout.LabelField(prefix, label, EditorStyles.boldLabel);
        }
    }
}
#endif