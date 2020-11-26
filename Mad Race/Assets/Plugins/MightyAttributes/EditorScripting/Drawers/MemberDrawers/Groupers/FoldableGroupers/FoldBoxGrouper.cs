#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FoldBoxGrouper : BaseFoldGrouper<FoldBoxGroupAttribute>
    {
        protected override bool BeginFoldout(bool foldout, string label, int indentLevel, FoldBoxGroupAttribute attribute) =>
            BeginFoldout(foldout, label, indentLevel);

        protected override void BeginGroupImpl(int indentLevel) => DrawBody();

        protected override void EndGroupImpl(int indentLevel) => EndFoldout(indentLevel);

        public bool BeginFoldout(bool foldout, string label, int indentLevel)
        {
            GUILayout.BeginVertical(MightyStyleUtilities.GetFoldBoxHeader(indentLevel));
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(MightyStyles.FoldBoxHeaderContent);
            foldout = MightyGUIUtilities.DrawFoldout(foldout, label, MightyStyles.BoldFoldout);
            GUILayout.EndVertical();

            if (foldout) return true;
            EditorGUI.indentLevel = indentLevel;
            GUILayout.EndVertical();
            return false;
        }

        public void DrawBody() => EditorGUILayout.BeginVertical(MightyStyles.FoldBoxBody);

        public void EndFoldout(int indentLevel)
        {
            EditorGUI.indentLevel = indentLevel;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif