#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MightyDownloadDocWindow : EditorWindow
    {
        public enum WindowState
        {
            FirstTime,
            NewVersion,
        }

        private const float SPACE = 2;
        private const float LARGE_SPACE = 5;

        private const float WIDTH = 385;
        private const float HEIGHT = 70;

        private static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);

        private const string TITLE_TEXT = "Download Mighty Doc";

        private static DarkBoxGrouper m_darkBox;

        private WindowState m_windowState;
        private MightyDocVersion m_docVersion;

        #region Core

        public static MightyDownloadDocWindow Open(WindowState state, MightyDocVersion docVersion)
        {
            var window = GetWindow<MightyDownloadDocWindow>();
            window.m_windowState = state;
            window.m_docVersion = docVersion;
            window.Show();
            return window;
        }

        private void OnEnable()
        {
            m_darkBox = MightyDrawersDatabase.GetDrawer<DarkBoxGrouper>();
            titleContent = new GUIContent(MightyGUIUtilities.DrawIcon(IconName.HELP))
            {
                text = TITLE_TEXT,
            };
        }

        private void OnGUI()
        {
            minSize = maxSize = Size;

            GUILayout.BeginHorizontal();
            LargeSpace();
            GUILayout.BeginVertical();

            LargeSpace();

            BeginBox();

            GUI.color = MightyColorUtilities.Yellow;
            
            switch (m_windowState)
            {
                case WindowState.FirstTime:
                {
                    if (MightySettingsServices.FirstTime) MightySettingsServices.FirstTime = false;
                    GUILayout.Label("It appears to be the first time you're using [Mighty]Attributes.");
                    break;
                }
                case WindowState.NewVersion:
                    GUILayout.Label("A new version of the documentation is available.");
                    break;
            }

            GUILayout.Label("Do you want to download the documentation?", EditorStyles.boldLabel);
            
            MightyDocServices.UpdateDocVersion(m_docVersion.versionNumber);

            GUI.color = Color.white;
            
            GUILayout.BeginHorizontal();
            
            if (Button("Download"))
            {
                MightyDocServices.DownloadDocFromVersion(m_docVersion);
                Close();
            }

            if (Button("Cancel"))
                Close();
            
            GUILayout.EndHorizontal();
            
            EndBox();

            GUILayout.EndVertical();
            Space();
            GUILayout.EndHorizontal();
        }

        #endregion /Core

        #region Utilities

        private static void BeginBox()
        {
            MightyColorUtilities.BeginBackgroundColor(ColorValue.SoftContrast.GetColor());
            m_darkBox.BeginDrawGroup(indentInside: false);

            GUILayout.BeginHorizontal();
            Space();

            GUILayout.BeginVertical();
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

        private static bool Button(string label)
        {
            var previousColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.white;

            var pressed = MightyGUIUtilities.DrawButton(label, 25);

            GUI.backgroundColor = previousColor;

            return pressed;
        }

        private static void Space() => GUILayout.Space(SPACE);
        private static void LargeSpace() => GUILayout.Space(LARGE_SPACE);

        #endregion /Utilities
    }
}
#endif