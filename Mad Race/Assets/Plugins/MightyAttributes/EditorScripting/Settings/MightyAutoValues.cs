#if UNITY_EDITOR
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MightyAttributes.Editor
{
    public class MightyAutoValues : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (MightySettingsServices.Activated && MightySettingsServices.AutoValuesOnBuild)
                ApplyAutoValues();
        }

        [DidReloadScripts]
        public static void OnReloadScripts()
        {
            if (MightySettingsServices.Activated && MightySettingsServices.AutoValuesOnReloadScripts)
                ApplyAutoValues();

            ManageAutoValuesEvent();
        }

        public static void ManageAutoValuesEvent()
        {
            if (MightySettingsServices.Activated && MightySettingsServices.AutoValuesOnPlay)
                EditorApplication.playModeStateChanged += ApplyAutoValuesOnExitEditMode;
            else
                EditorApplication.playModeStateChanged -= ApplyAutoValuesOnExitEditMode;
        }

        private static void ApplyAutoValuesOnExitEditMode(PlayModeStateChange state)
        {
            if (MightySettingsServices.Activated && MightySettingsServices.AutoValuesOnPlay && state == PlayModeStateChange.ExitingEditMode)
                ApplyAutoValues();
        }

        [MenuItem("Tools/[Mighty]Attributes/Apply Auto Values", false, 51)]
        public static async void ApplyAutoValuesAsync()
        {
            AutoValuesWindowUtilities.Open();
            await Task.Delay(50);

            var mightyEditors = MightyEditorUtilities.GetMightyEditors().ToArray();
            AutoValuesWindowUtilities.DisplayCount(mightyEditors.Length);

            for (var i = 0; i < mightyEditors.Length; i++)
            {
                AutoValuesWindowUtilities.SetIndex(i);
                await Task.Yield();
                mightyEditors[i].ApplyAutoValues();
            }

            AutoValuesWindowUtilities.Close();

            MightyDebugUtilities.MightyDebug("Auto Values Applied");
        }

        public static void ApplyAutoValues()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var script in ReferencesUtilities.FindAllObjects<MonoBehaviour>(SceneManager.GetSceneAt(i)))
                    if (MightyEditorUtilities.CreateMightyEditor<MonoBehaviourEditor>(script, out var mightyEditor))
                        mightyEditor.ApplyAutoValues();

            foreach (var script in typeof(ScriptableObject).FindAssetsOfType())
                if (MightyEditorUtilities.CreateMightyEditor<ScriptableObjectEditor>(script, out var mightyEditor))
                    mightyEditor.ApplyAutoValues();

            MightyDebugUtilities.MightyDebug("Auto Values Applied", MightyDebugUtilities.LogType.AutoValues);
        }
    }
}
#endif