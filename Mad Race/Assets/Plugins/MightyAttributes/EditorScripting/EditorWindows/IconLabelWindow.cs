#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class IconLabelWindow : EditorWindow
    {
        private static IconLabelWindow m_instance;

        private string m_label;
        private int m_fontSize;
        private Color m_contentColor;
        private Action<int> m_onCloseEvent;
        private Action m_extraDrawAction;

        public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2? position = null) =>
            Init(iconName, label, fontSize, contentColor, new Vector2(35 + label.Length * 5, 35), position);

        public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Action<int> onCloseEvent) =>
            Init(iconName, label, fontSize, contentColor, new Vector2(35 + label.Length * 5, 35), onCloseEvent);

        public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2? position,
            Action<int> onCloseEvent) => Init(iconName, label, fontSize, contentColor, new Vector2(35 + label.Length * 5, 35), position,
            onCloseEvent);

        public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2 size,
            Vector2? position = null) => BaseInit(iconName, label, fontSize, contentColor, size, position);

        public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2 size,
            Action<int> onCloseEvent) => BaseInit(iconName, label, fontSize, contentColor, size, null, onCloseEvent);

        public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2 size, Vector2? position,
            Action<int> onCloseEvent) => BaseInit(iconName, label, fontSize, contentColor, size, position, onCloseEvent);

        private static IconLabelWindow BaseInit(string iconName, string label, int fontSize, Color contentColor, Vector2 size,
            Vector2? position = null, Action<int> onCloseEvent = null)
        {
            m_instance = GetWindow<IconLabelWindow>();
            m_instance.m_label = label;
            m_instance.m_fontSize = fontSize;
            m_instance.m_contentColor = contentColor;
            m_instance.titleContent = MightyGUIUtilities.DrawIcon(iconName);
            m_instance.maxSize = m_instance.minSize = size;
            m_instance.maximized = false;
            if (position != null)
                m_instance.position = new Rect((Vector2) position, size);
            m_instance.m_onCloseEvent = onCloseEvent;
            m_instance.Show();
            return m_instance;
        }

        public static void AddExtraDraw(IconLabelWindow instance, Action extraDrawAction) =>
            AddExtraDraw(instance.GetInstanceID(), extraDrawAction);

        public static void AddExtraDraw(int instanceID, Action extraDrawAction)
        {
            if (m_instance != null && m_instance.GetInstanceID() == instanceID)
                m_instance.m_extraDrawAction = extraDrawAction;
        }

        public static void CloseWindow(IconLabelWindow instance) => CloseWindow(instance.GetInstanceID());

        public static void CloseWindow(int instanceID)
        {
            if (m_instance != null && m_instance.GetInstanceID() == instanceID)
                m_instance.Close();
        }

        private void OnDestroy()
        {
            m_extraDrawAction = null;
            m_onCloseEvent?.Invoke(m_instance.GetInstanceID());
        }

        private void OnGUI()
        {
            GUILayout.FlexibleSpace();
            MightyGUIUtilities.DrawHorizontal(() =>
            {
                GUILayout.FlexibleSpace();
                var style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = m_fontSize,
                    fontStyle = FontStyle.Bold,
                };
                style.normal.textColor = style.active.textColor = style.focused.textColor = style.hover.textColor = m_contentColor;

                GUILayout.Label(m_label, style);
                GUILayout.FlexibleSpace();
            });
            GUILayout.FlexibleSpace();
            m_extraDrawAction?.Invoke();
        }
    }
}
#endif