#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class DownloadingWindowUtilities
    {
        private static DownloadingWindow m_downloadingWindow;

        private static DownloadingWindow DownloadingWindow
        {
            get
            {
                if (m_downloadingWindow == null) m_downloadingWindow = Resources.FindObjectsOfTypeAll<DownloadingWindow>().FirstOrDefault();
                if (m_downloadingWindow == null) m_downloadingWindow = ScriptableObject.CreateInstance<DownloadingWindow>();
                return m_downloadingWindow;
            }
        }

        public static DownloadingWindow Open() => DownloadingWindow.OpenWindow();

        public static void Close()
        {
            if (m_downloadingWindow) m_downloadingWindow.Close();
            m_downloadingWindow = null;
        }

        public static void SetPercent(float percent) => DownloadingWindow.SetPercent(percent);
    }

    public class DownloadingWindow : EditorWindow
    {
        private const float SPACE = 3;

        private const float WIDTH = 385;
        private const float HEIGHT = 35;

        private static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);

        private float m_fillPercent;

        #region Core

        public DownloadingWindow OpenWindow()
        {
            ShowPopup();
            m_fillPercent = 0;
            return this;
        }

        public void SetPercent(float percent)
        {
            m_fillPercent = percent;
            Repaint();
        }

        private void OnGUI()
        {
            minSize = maxSize = Size;

            position = new Rect(new Vector2((float) Screen.currentResolution.width / 2 - minSize.x / 2,
                (float) Screen.currentResolution.height / 2 - minSize.y / 2), minSize);

            GUI.color = MightyColorUtilities.Brighter;

            GUILayout.BeginVertical(MightyStyles.White);
            Space();
            GUILayout.BeginHorizontal();
            Space();

            GUILayout.BeginVertical();

            ProgressBarDrawer.DrawBar(HEIGHT - SPACE * 2, m_fillPercent, "Downloading...", MightyColorUtilities.Blue, Color.white);

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            Space();
            GUILayout.EndVertical();
        }

        #endregion /Core

        private static void Space() => GUILayout.Space(SPACE);
    }
}
#endif