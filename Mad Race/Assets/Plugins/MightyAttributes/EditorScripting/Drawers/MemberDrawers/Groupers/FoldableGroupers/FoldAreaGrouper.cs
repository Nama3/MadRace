#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FoldAreaGrouper : BaseFoldGrouper<FoldAreaGroupAttribute>
    {
        protected override bool BeginFoldout(bool foldout, string label, int indentLevel, FoldAreaGroupAttribute attribute) => 
            BeginFoldout(foldout, label, indentLevel, attribute.DrawLine, attribute.LineColor);

        protected override void BeginGroupImpl(int indentLevel) => DrawBody();

        protected override void EndGroupImpl(int indentLevel) => EndFoldout(indentLevel);

        public bool BeginFoldout(bool foldout, string label, int indentLevel, bool drawLine, ColorValue lineColor)
        {
            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;

            GUI.backgroundColor = foldout ? backgroundColor : MightyColorUtilities.DarkenColor(backgroundColor, .3f);
            GUI.contentColor = foldout ? contentColor : MightyColorUtilities.DarkenColor(contentColor, .25f);

            GUILayout.BeginVertical(MightyStyleUtilities.GetFoldAreaHeader(indentLevel));
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(MightyStyles.FoldBoxHeaderContent);
            foldout = MightyGUIUtilities.DrawFoldout(foldout, label, MightyStyles.FoldAreaLabel);
            GUILayout.EndVertical();

            if (!foldout)
            {
                EditorGUI.indentLevel = indentLevel;
                GUILayout.EndVertical();
            }
            else if (drawLine) DrawLine(lineColor);

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;

            return foldout;
        }

        public void DrawBody() => EditorGUILayout.BeginVertical(MightyStyles.FoldAreaBody);

        public void EndFoldout(int indentLevel)
        {
            EditorGUI.indentLevel = indentLevel;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif