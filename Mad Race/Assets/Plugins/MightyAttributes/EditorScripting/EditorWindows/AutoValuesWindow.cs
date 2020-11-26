#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class AutoValuesWindowUtilities
    {
        private static AutoValuesWindow m_autoValuesWindow;

        private static AutoValuesWindow AutoValuesWindow
        {
            get
            {
                if (m_autoValuesWindow == null) m_autoValuesWindow = Resources.FindObjectsOfTypeAll<AutoValuesWindow>().FirstOrDefault();
                if (m_autoValuesWindow == null) m_autoValuesWindow = ScriptableObject.CreateInstance<AutoValuesWindow>();
                return m_autoValuesWindow;
            }
        }

        public static AutoValuesWindow Open() => AutoValuesWindow.OpenWindow();

        public static void Close()
        {
            if (m_autoValuesWindow) m_autoValuesWindow.Close();
            m_autoValuesWindow = null;
        }

        public static void DisplayCount(int count) => AutoValuesWindow.DisplayCount(count);
        public static void SetIndex(int index) => AutoValuesWindow.SetIndex(index);
    }

    public class AutoValuesWindow : EditorWindow
    {
        private const string APPLYING_VALUES = "Applying Auto Values...";
        private const string SEARCHING_OBJECTS = "Searching Mighty Objects";
        private const string PROCESSING_OBJECT = "Processing Object";
        private const string SLASH = "/";

        private int m_iconIndex;
        private string m_infoLabel;
        private bool m_displayProgression;
        private string m_indexLabel;
        private string m_countLabel;

        public AutoValuesWindow OpenWindow()
        {
            ShowPopup();
            m_infoLabel = SEARCHING_OBJECTS;
            m_displayProgression = false;
            return this;
        }

        public void DisplayCount(int count)
        {
            m_displayProgression = true;
            m_infoLabel = PROCESSING_OBJECT;
            m_countLabel = count.ToString();
            Refresh();
        }

        public void SetIndex(int index)
        {
            m_indexLabel = index.ToString();
            Refresh();
        }

        private void Refresh()
        {
            IconName.NextLoadingIndex(ref m_iconIndex);
            Repaint();
        }

        private void OnGUI()
        {
            minSize = maxSize = new Vector2(400, 90);
            position = new Rect(new Vector2((float) Screen.currentResolution.width / 2 - minSize.x / 2,
                (float) Screen.currentResolution.height / 2 - minSize.y / 2), minSize);

            GUI.color = MightyColorUtilities.Brighter;

            GUILayout.BeginVertical(MightyStyles.White);
            GUILayout.BeginVertical(MightyStyles.SimpleDarkBox);
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUI.color = Color.white;
            GUILayout.Box(MightyGUIUtilities.DrawIcon(IconName.Loading(m_iconIndex)));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUI.color = MightyColorUtilities.Yellow;
            GUILayout.Label(APPLYING_VALUES, MightyStyles.BigBoldLabelStyle);
            GUI.color = Color.white;
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.white;
            GUILayout.Label(m_infoLabel, MightyStyles.InfoLabelStyle);
            GUILayout.Label(m_indexLabel, MightyStyles.InfoLabelStyle);
            if (m_displayProgression)
            {
                GUILayout.Label(SLASH, MightyStyles.InfoLabelStyle);
                GUILayout.Label(m_countLabel, MightyStyles.InfoLabelStyle);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }
}
#endif