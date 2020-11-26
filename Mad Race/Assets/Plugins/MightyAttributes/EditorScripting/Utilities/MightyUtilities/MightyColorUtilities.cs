#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class MightyColorUtilities
    {
        public static readonly Color Red = new Color(1, 0, .1f);
        public static readonly Color Orange = new Color(1, .5f, 0);
        public static readonly Color Yellow = new Color(1, .8f, 0);
        public static readonly Color Green = new Color(.2f, 1, .2f);
        public static readonly Color Blue = new Color(0, .5f, .8f);
        public static readonly Color Purple = new Color(.5f, 0, 1);
        public static readonly Color Pink = new Color(1, .4f, .85f);

        public static readonly Color DarkGray = new Color(.4f, .4f, .4f);
        
        public static readonly Color Brightest = new Color(.82f, .82f, .82f);
        public static readonly Color Brighter = new Color(.7f, .7f, .7f);
        public static readonly Color Bright = new Color(.57f, .57f, .57f);

        public static readonly Color Dark = new Color(.27f, .27f, .27f);
        public static readonly Color Darker = new Color(.16f, .16f, .16f);
        public static readonly Color Darkest = new Color(.07f, .07f, .07f);

        public static readonly Color DarkSkinTextColor = new Color(.769f, .769f, .769f);
        public static readonly Color LightSkinTextColor = new Color(.024f, .024f, .024f);

        private static readonly Stack<Color?> ColorStack = new Stack<Color?>();
        private static readonly Stack<Color?> ContentColorStack = new Stack<Color?>();
        private static readonly Stack<Color?> BackgroundColorStack = new Stack<Color?>();

        public static Color BrightenColor(Color color, float brightFactor) => Color.Lerp(color, Color.white, brightFactor);

        public static Color DarkenColor(Color color, float darkFactor) => Color.Lerp(color, Color.black, darkFactor);

        public static void BeginColor(Color? color)
        {
            if (color == null)
            {
                ColorStack.Push(null);
                return;
            }

            ColorStack.Push(GUI.color);
            GUI.color = (Color) color;
        }

        public static void EndColor()
        {
            if (ColorStack.Count == 0)
            {
                Debug.LogError("EndColor: BeginColor must be called first.");
                return;
            }

            var color = ColorStack.Pop();
            if (color != null)
                GUI.color = (Color) color;
        }

        public static void BeginContentColor(Color? color)
        {
            if (color == null)
            {
                ContentColorStack.Push(null);
                return;
            }

            ContentColorStack.Push(GUI.contentColor);
            GUI.contentColor = (Color) color;
        }

        public static void EndContentColor()
        {
            if (ContentColorStack.Count == 0)
            {
                Debug.LogError("EndContentColor: BeginContentColor must be called first.");
                return;
            }

            var color = ContentColorStack.Pop();
            if (color != null)
                GUI.contentColor = (Color) color;
        }

        public static void BeginBackgroundColor(Color? color)
        {
            if (color == null)
            {
                BackgroundColorStack.Push(null);
                return;
            }

            BackgroundColorStack.Push(GUI.backgroundColor);
            GUI.backgroundColor = (Color) color;
        }

        public static void EndBackgroundColor()
        {
            if (BackgroundColorStack.Count == 0)
            {
                Debug.LogError("EndBackgroundColor: BeginBackgroundColor must be called first.");
                return;
            }

            var color = BackgroundColorStack.Pop();
            if (color != null)
                GUI.backgroundColor = (Color) color;
        }

        #region ColorValue

        public static Color GetColor(this BaseMightyMember mightyMember, object target, string colorName, ColorValue colorValue,
            Color defaultColor) => GetColor(mightyMember, target, colorName, GetColor(colorValue, defaultColor));

        public static Color? GetColor(this BaseMightyMember mightyMember, object target, string colorName, ColorValue colorValue) =>
            GetColor(mightyMember, target, colorName, GetColor(colorValue, null));

        public static MightyInfo<Color?> GetColorInfo(this BaseMightyMember mightyMember, object target, string colorName,
            ColorValue colorValue) => GetColorInfo(mightyMember, target, colorName, GetColor(colorValue, null));

        public static Color GetColor(this BaseMightyMember mightyMember, object target, string colorName, Color defaultColor) =>
            mightyMember.GetValueFromMember(target, colorName, out Color color, ColorUtility.TryParseHtmlString) ? color : defaultColor;

        public static Color? GetColor(this BaseMightyMember mightyMember, object target, string colorName, Color? defaultColor) =>
            mightyMember.GetValueFromMember(target, colorName, out Color color, ColorUtility.TryParseHtmlString) ? color : defaultColor;

        public static MightyInfo<Color?> GetColorInfo(this BaseMightyMember mightyMember, object target, string colorName,
            Color? defaultColor) => mightyMember.GetInfoFromMember(target, colorName, out var memberInfo, (string s, out Color? value) =>
            (value = ColorUtility.TryParseHtmlString(s, out var colorValue) ? colorValue : (Color?) null) != null)
            ? memberInfo
            : new MightyInfo<Color?>(null, null, defaultColor);

        public static Color GetColor(this ColorValue colorValue) => GetColor(colorValue, Color.white);

        public static Color GetColor(this ColorValue colorValue, Color defaultColor) => InternalGetColor(colorValue) ?? defaultColor;

        public static Color? GetColor(ColorValue colorValue, Color? defaultColor) => InternalGetColor(colorValue) ?? defaultColor;

        private static Color? InternalGetColor(ColorValue colorValue)
        {
            switch (colorValue)
            {
                case ColorValue.Red:
                    return Red;
                case ColorValue.Orange:
                    return Orange;
                case ColorValue.Yellow:
                    return Yellow;
                case ColorValue.Green:
                    return Green;
                case ColorValue.Blue:
                    return Blue;
                case ColorValue.Purple:
                    return Purple;
                case ColorValue.Pink:
                    return Pink;

                case ColorValue.White:
                    return Color.white;
                case ColorValue.Gray:
                    return Color.gray;
                case ColorValue.DarkGray:
                    return DarkGray;
                case ColorValue.Black:
                    return Color.black;

                case ColorValue.Brightest:
                    return Brightest;
                case ColorValue.Brighter:
                    return Brighter;
                case ColorValue.Bright:
                    return Bright;
                case ColorValue.Dark:
                    return Dark;
                case ColorValue.Darker:
                    return Darker;
                case ColorValue.Darkest:
                    return Darkest;

                case ColorValue.SofterContrast:
                    return EditorGUIUtility.isProSkin ? Brightest : Color.gray;
                case ColorValue.SoftContrast:
                    return EditorGUIUtility.isProSkin ? Brighter : DarkGray;
                case ColorValue.Contrast:
                    return EditorGUIUtility.isProSkin ? Bright : Dark;
                case ColorValue.HardContrast:
                    return EditorGUIUtility.isProSkin ? Bright : Darker;
            }

            return null;
        }

        #endregion /ColorValue
    }
}
#endif