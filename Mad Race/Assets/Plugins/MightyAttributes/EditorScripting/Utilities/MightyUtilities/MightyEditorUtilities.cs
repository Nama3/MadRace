#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MightyAttributes.Editor
{
    public static class MightyEditorUtilities
    {
        private static bool m_editorChanged;

        public static IEnumerable<BaseMightyEditor> GetMightyEditors()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var script in ReferencesUtilities.FindAllObjects<MonoBehaviour>(SceneManager.GetSceneAt(i)))
                    if (CreateMightyEditor<MonoBehaviourEditor>(script, out var mightyEditor))
                        yield return mightyEditor;

            foreach (var script in typeof(ScriptableObject).FindAssetsOfType())
                if (CreateMightyEditor<ScriptableObjectEditor>(script, out var mightyEditor))
                    yield return mightyEditor;
        }

        public static bool IsMightyScript(Object script)
        {
            if (script == null) return false;

            var type = script.GetType();

            return type.HasAttributeOfType<BaseMightyAttribute>() || type.GetMembers(ReflectionUtilities.ANY_MEMBER_FLAGS)
                .Any(memberInfo => memberInfo.HasAttributeOfType<BaseMightyAttribute>());
        }

        public static bool CreateMightyEditor<T>(Object script, out T mightyEditor) where T : BaseMightyEditor
        {
            var isMightyScript = IsMightyScript(script);
            mightyEditor = isMightyScript ? (T) UnityEditor.Editor.CreateEditor(script, typeof(T)) : null;
            return isMightyScript;
        }

        public static void RegisterChange() => m_editorChanged = true;
        public static void ResetChange() => m_editorChanged = false;

        public static bool HasEditorChanged() => m_editorChanged;

        public static void ManageValueChanged(this SerializedObject serializedObject)
        {
            if (serializedObject == null) return;

            if (serializedObject.hasModifiedProperties) serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
    }
}
#endif