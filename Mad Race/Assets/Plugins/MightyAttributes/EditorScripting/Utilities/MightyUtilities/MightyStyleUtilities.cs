#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static MightyAttributes.Editor.MightyStyles;

namespace MightyAttributes.Editor
{
    public static class MightyStyles
    {
        internal static readonly RectOffset Zero = new RectOffset(0, 0, 0, 0);
        internal static readonly RectOffset LineMargin = new RectOffset(0, 0, 2, 2);
        private static readonly RectOffset BoxSpace = new RectOffset(3, 3, 3, 3);

        private static readonly Vector2 BoxLabelOffset = new Vector2(-14, 0);

        public static readonly GUIStyle White = new GUIStyle
        {
            normal = {background = EditorGUIUtility.whiteTexture}
        };

        public static readonly GUIStyle LineStyle = new GUIStyle(White)
        {
            fixedHeight = 2,
            stretchWidth = true
        };

        public static readonly GUIStyle LightBoxStyle = new GUIStyle(EditorStyles.helpBox)
        {
            margin = BoxSpace,
            padding = BoxSpace
        };

        public static readonly GUIStyle DarkBoxStyle = new GUIStyle(EditorStyles.textField)
        {
            margin = BoxSpace,
            padding = BoxSpace,
        };

        public static readonly GUIStyle BoxGroupLabel = new GUIStyle(EditorStyles.boldLabel)
        {
            contentOffset = BoxLabelOffset
        };

        public static readonly GUIStyle FoldAreaLabel = new GUIStyle(EditorStyles.largeLabel)
        {
            margin = Zero,
            padding = Zero,
            fixedHeight = 40,
        };

        public static readonly GUIStyle FoldBoxHeaderStyle = new GUIStyle(GUI.skin.button)
        {
            margin = BoxSpace,
            padding = Zero
        };

        public static readonly GUIStyle FoldBoxHeaderContent = new GUIStyle
        {
            margin = Zero,
            padding = Zero,
        };

        public static readonly GUIStyle FoldBoxBody = new GUIStyle(EditorStyles.textField)
        {
            margin = Zero,
            padding = BoxSpace
        };

        public static readonly GUIStyle FoldAreaBody = new GUIStyle
        {
            margin = Zero,
            padding = BoxSpace
        };

        public static readonly GUIStyle BoldFoldout = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };

        public static readonly GUIStyle BigLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 25
        };

        public static readonly GUIStyle BigBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 25
        };

        public static readonly GUIStyle InfoLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 15
        };

        public static readonly GUIStyle SimpleDarkBox = new GUIStyle(EditorStyles.textField)
        {
            margin = new RectOffset(3, 3, 3, 3)
        };
        
        public static readonly GUIStyle ButtonArrayStyle = new GUIStyle
        {
            margin = BoxSpace
        };
    }

    public static class MightyStyleUtilities
    {
        private static bool m_lightToDarkBegan;

        private static MethodInfo m_boldFontMethodInfo;
        private static GUIStyle[] m_styles;

        public static float GetLineHeight(bool drawSpace) => GetLine(drawSpace).GetStyleHeight();

        public static GUIStyle GetLine(bool drawSpace)
        {
            LineStyle.margin = drawSpace ? LineMargin : Zero;
            return LineStyle;
        }

        public static GUIStyle GetBox(int indentLevel) => LightBoxStyle.IndentStyle(indentLevel);

        public static GUIStyle GetDarkBox(int indentLevel) =>  DarkBoxStyle.IndentStyle(indentLevel);

        public static GUIStyle GetFoldAreaHeader(int indentLevel) => DarkBoxStyle.IndentStyle(indentLevel);

        public static GUIStyle GetFoldBoxHeader(int indentLevel) => FoldBoxHeaderStyle.IndentStyle(indentLevel);

        public static GUIStyle GetButtonArray(int indentLevel) => ButtonArrayStyle.IndentStyle(indentLevel);

        public static GUIStyle IndentStyle(this GUIStyle style, int indentLevel)
        {
            style.margin.left = MightyGUIUtilities.GetIndentWidth(indentLevel);
            return style;
        }

        public static float GetStyleHeight(this GUIStyle style) => style.margin.vertical + style.fixedHeight;

        public static void SetBoldDefaultFont(bool value)
        {
            if (m_boldFontMethodInfo == null)
                m_boldFontMethodInfo =
                    typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
            if (m_boldFontMethodInfo == null) return;

            m_boldFontMethodInfo.Invoke(null, new object[] {value});
        }

        #region GetStyle

        public static GUIStyle GetStyle(this BaseMightyMember mightyMember, object target, string styleName, out bool exception)
        {
            var ex = false;

            if (string.IsNullOrEmpty(styleName) || !mightyMember.GetValueFromMember(target, styleName, out var guiStyle,
                (string name, out GUIStyle value) => GetStyle(name, out value, out ex)))
            {
                exception = ex;
                return null;
            }

            exception = ex;
            return guiStyle;
        }

        public static GUIStyle GetStyle(this BaseMightyMember mightyMember, object target, string styleName, out bool exception,
            bool editorStyle)
        {
            var ex = false;

            if (string.IsNullOrEmpty(styleName) || !mightyMember.GetValueFromMember(target, styleName, out var guiStyle,
                (string name, out GUIStyle value) => GetStyle(name, out value, out ex, editorStyle)))
            {
                exception = ex;
                return null;
            }

            exception = ex;
            return guiStyle;
        }

        public static MightyInfo<GUIStyle> GetStyleInfo(this BaseMightyMember mightyMember, object target, string styleName,
            out bool exception)
        {
            var ex = false;

            if (string.IsNullOrEmpty(styleName) || !mightyMember.GetInfoFromMember(target, styleName, out var styleInfo,
                (string name, out GUIStyle value) => GetStyle(name, out value, out ex)))
            {
                exception = ex;
                return null;
            }

            exception = ex;
            return styleInfo;
        }

        public static MightyInfo<GUIStyle> GetStyleInfo(this BaseMightyMember mightyMember, object target, string styleName,
            out bool exception,
            bool editorStyle)
        {
            var ex = false;

            if (string.IsNullOrEmpty(styleName) || !mightyMember.GetInfoFromMember(target, styleName, out var styleInfo,
                (string name, out GUIStyle value) => GetStyle(name, out value, out ex, editorStyle)))
            {
                exception = ex;
                return null;
            }

            exception = ex;
            return styleInfo;
        }

        private static bool GetStyle(string styleName, out GUIStyle guiStyle, out bool exception)
        {
            if (string.IsNullOrWhiteSpace(styleName))
            {
                guiStyle = null;
                exception = false;
                return false;
            }

            var namePath = styleName.Split('.');
            if (namePath.Length > 1)
            {
                switch (namePath[0])
                {
                    case "EditorStyles":
                        return GetStyle(styleName, out guiStyle, out exception, true);
                    case "GUI":
                    case "GUISkin":
                        return GetStyle(styleName, out guiStyle, out exception, false);
                }
            }

            return GetEditorStyle(styleName, out guiStyle, out exception) ||
                   GetGUISkinStyle(styleName, out guiStyle, out exception) ||
                   GetCustomStyle(styleName, out guiStyle, out exception);
        }

        private static bool GetStyle(string styleName, out GUIStyle guiStyle, out bool exception, bool editorStyle)
        {
            if (string.IsNullOrWhiteSpace(styleName))
            {
                guiStyle = null;
                exception = false;
                return false;
            }

            return editorStyle && GetEditorStyle(styleName, out guiStyle, out exception) ||
                   GetGUISkinStyle(styleName, out guiStyle, out exception) ||
                   GetCustomStyle(styleName, out guiStyle, out exception);
        }

        private static bool GetEditorStyle(string styleName, out GUIStyle guiStyle, out bool exception)
        {
            try
            {
                exception = false;
                foreach (var styleProperty in GetAllStylesProperties<EditorStyles>())
                {
                    if (styleProperty.Name != styleName && $"EditorStyles.{styleProperty.Name}" != styleName) continue;
                    guiStyle = styleProperty.GetValue(null) as GUIStyle;
                    return true;
                }
            }
            catch
            {
                exception = true;
            }

            guiStyle = null;
            return false;
        }

        private static bool GetGUISkinStyle(string styleName, out GUIStyle guiStyle, out bool exception)
        {
            try
            {
                exception = false;
                foreach (var styleProperty in GetAllStylesProperties<GUISkin>())
                {
                    if (styleProperty.Name != styleName && $"GUI.skin.{styleProperty.Name}" != styleName &&
                        $"GUISkin.{styleProperty.Name}" != styleName) continue;

                    guiStyle = styleProperty.GetValue(GUI.skin) as GUIStyle;
                    return true;
                }
            }
            catch
            {
                exception = true;
            }

            guiStyle = null;
            return false;
        }

        private static bool GetCustomStyle(string styleName, out GUIStyle guiStyle, out bool exception)
        {
            try
            {
                exception = false;
                foreach (var style in GUI.skin.customStyles)
                {
                    if (style.name != styleName) continue;
                    guiStyle = style;
                    return true;
                }
            }
            catch
            {
                exception = true;
            }

            guiStyle = null;
            return false;
        }

        public static PropertyInfo[] GetAllStylesProperties<T>() =>
            typeof(T).GetProperties().Where(x => x.PropertyType == typeof(GUIStyle)).ToArray();

        #endregion /GetStyle
    }
}
#endif