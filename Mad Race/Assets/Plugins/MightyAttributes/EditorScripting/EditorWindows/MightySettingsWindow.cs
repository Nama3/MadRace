#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MightySettingsWindow : EditorWindow
    {
        private const float SPACE = 2;
        private const float LARGER_SPACE = 3;
        private const float LARGEST_SPACE = 5;

        private const float WIDTH = 370;
        private const float HEIGHT = 465;
        private const int TOGGLE_LABEL_WIDTH = 200;

        private static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);

        private const string TITLE_TEXT = " Mighty Settings";
        private const string TITLE_TOOLTIP = "Settings for [Mighty]Attributes";

        private static DarkBoxGrouper m_darkBox;

        #region Core

        [MenuItem("Tools/[Mighty]Attributes/Settings", false, 11)]
        private static MightySettingsWindow Open()
        {
            var window = GetWindow<MightySettingsWindow>();
            window.Show();
            return window;
        }

        private void OnEnable()
        {
            m_darkBox = MightyDrawersDatabase.GetDrawer<DarkBoxGrouper>();
            titleContent = new GUIContent(MightyGUIUtilities.DrawIcon(IconName.SETTINGS))
            {
                text = TITLE_TEXT,
                tooltip = TITLE_TOOLTIP,
            };
        }

        private void OnGUI()
        {
            minSize = maxSize = Size;

            GUILayout.BeginHorizontal();
            LargestSpace();
            GUILayout.BeginVertical();

            LargerSpace();
            GUI.enabled = DrawActivateButton();

            Space();
            DrawAssemblyNames();

            Space();
            DrawAutoValues();

            Space();
            DrawScriptsReload();

            Space();
            DrawHierarchy();

            GUI.enabled = true;

            GUILayout.EndVertical();
            Space();
            GUILayout.EndHorizontal();
        }

        #region Draws

        private static bool DrawActivateButton()
        {
            Space();
            GUILayout.BeginHorizontal(GUILayout.Height(30));
            GUILayout.FlexibleSpace();

            var activated = MightySettingsServices.Activated;

            var color = GUI.color;
            GUI.enabled = !activated;

            GUI.color = new Color(1, 1, 1, 2);

            activated = MightyGUIUtilities.DrawToggleButton(activated, "ON",
                new Color(0.27f, 0.89f, 0.09f), GUILayout.Width(100), GUILayout.Height(30));

            GUI.enabled = activated;

            activated = !MightyGUIUtilities.DrawToggleButton(!activated, "OFF",
                new Color(0.84f, 0.15f, 0.13f), GUILayout.Width(100), GUILayout.Height(30));

            GUI.color = color;

            MightySettingsServices.Activated = activated;

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUI.enabled = true;

            return activated;
        }

        private static void DrawAssemblyNames()
        {
            BeginBox("Assemblies");

            Space();
            MightySettingsServices.MainAssemblyName =
                EditorGUILayout.TextField("Main Assembly Label", MightySettingsServices.MainAssemblyName);

            Space();
            MightySettingsServices.PluginsAssemblyName =
                EditorGUILayout.TextField("Plugins Assembly Label", MightySettingsServices.PluginsAssemblyName);

            Space();
            if (Button("Reset To Default"))
                MightySettingsServices.ResetAssemblyNamesToDefault();

            EndBox();
        }

        private static void DrawAutoValues()
        {
            BeginBox("Auto Value Attributes");

            DrawLabelWidth(TOGGLE_LABEL_WIDTH, () =>
            {
                Space();
                MightySettingsServices.AutoValuesOnReloadScripts =
                    EditorGUILayout.Toggle("Apply On Reload Scripts", MightySettingsServices.AutoValuesOnReloadScripts);

                EditorGUI.BeginChangeCheck();

                Space();
                MightySettingsServices.AutoValuesOnPlay =
                    EditorGUILayout.Toggle("Apply On Enter Play Mode", MightySettingsServices.AutoValuesOnPlay);

                if (EditorGUI.EndChangeCheck())
                    MightyAutoValues.ManageAutoValuesEvent();

                Space();
                MightySettingsServices.AutoValuesOnBuild =
                    EditorGUILayout.Toggle("Apply On Build", MightySettingsServices.AutoValuesOnBuild);

                EditorGUI.BeginDisabledGroup(!MightySettingsServices.AnyAutoValues);

                Space();
                MightySettingsServices.DisplayAutoValuesLogs =
                    EditorGUILayout.Toggle("Display Logs", MightySettingsServices.DisplayAutoValuesLogs);

                EditorGUI.EndDisabledGroup();

                Space();
                if (Button("Apply Auto Values"))
                    MightyAutoValues.ApplyAutoValuesAsync();
            });

            EndBox();
        }

        private static void DrawScriptsReload()
        {
            BeginBox("Reload Scripts Attributes");

            DrawLabelWidth(TOGGLE_LABEL_WIDTH, () =>
            {
                Space();
                EditorGUI.BeginDisabledGroup(!(MightySettingsServices.ActivateReloadScripts =
                    EditorGUILayout.Toggle("Activate Reload Scripts", MightySettingsServices.ActivateReloadScripts)));

                Space();
                MightySettingsServices.DisplayReloadScriptsLogs =
                    EditorGUILayout.Toggle("Display Logs", MightySettingsServices.DisplayReloadScriptsLogs);

                EditorGUI.EndDisabledGroup();

                Space();
                if (Button("Apply Reload Scripts"))
                    MightyReloadScripts.ApplyScriptReload();
            });

            EndBox();
        }

        private static void DrawHierarchy()
        {
            BeginBox("Hierarchy");

            Space();
            if (Button("Refresh Hierarchy"))
                MightyHierarchy.RefreshHierarchy();

            EndBox();
        }

        #endregion /Draws

        #endregion /Core

        #region Utilities

        private static void BeginBox(string label)
        {
            MightyColorUtilities.BeginBackgroundColor(ColorValue.SoftContrast.GetColor());
            m_darkBox.BeginDrawGroup(indentInside: false);

            GUILayout.BeginHorizontal();
            Space();

            GUILayout.BeginVertical();
            m_darkBox.DrawGroupLabel(label, EditorStyles.boldLabel);
            m_darkBox.DrawLine();
        }

        private static void EndBox()
        {
            Space();
            GUILayout.EndVertical();
            Space();
            GUILayout.EndHorizontal();
            m_darkBox.EndDrawGroup();
            MightyColorUtilities.EndBackgroundColor();
        }

        private static void DrawLabelWidth(float labelWidth, Action drawAction)
        {
            var previousWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            drawAction?.Invoke();
            EditorGUIUtility.labelWidth = previousWidth;
        }

        private static bool Button(string label)
        {
            var previousColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.white;

            var pressed = MightyGUIUtilities.DrawButton(label, 25);

            GUI.backgroundColor = previousColor;

            return pressed;
        }

        private static void Space() => GUILayout.Space(SPACE);
        private static void LargerSpace() => GUILayout.Space(LARGER_SPACE);
        private static void LargestSpace() => GUILayout.Space(LARGEST_SPACE);

        #endregion /Utilities
    }
}
#endif