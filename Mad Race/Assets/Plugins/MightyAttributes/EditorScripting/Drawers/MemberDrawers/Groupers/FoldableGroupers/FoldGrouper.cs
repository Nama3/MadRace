#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FoldGrouper : BaseFoldGrouper<FoldGroupAttribute>
    {
        protected override bool BeginFoldout(bool foldout, string label, int indentLevel, FoldGroupAttribute attribute) => 
            BeginFoldout(foldout, label);

        protected override void BeginGroupImpl(int indentLevel) => DrawBody();

        protected override void EndGroupImpl(int indentLevel) => EndFoldout(indentLevel);

        public bool BeginFoldout(bool foldout, string label)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginVertical();
            foldout = MightyGUIUtilities.DrawFoldout(foldout, label);
            GUILayout.EndVertical();

            if (foldout) return true;
            GUILayout.EndVertical();
            return false;
        }

        public void DrawBody() => EditorGUILayout.BeginVertical();

        public void EndFoldout(int indentLevel)
        {
            EditorGUI.indentLevel = indentLevel;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif