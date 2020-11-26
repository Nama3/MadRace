#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public static class MightyGUIUtilities
    {
        public const float LARGE_CHAR_SIZE = 7.5f;
        public const float CHAR_WIDTH = 6.5f;
        public const float CHAR_HEIGHT = 13f;
        public const float END_SPACE = 5;

        public const int TAB_SIZE = 15;

        public const float FIELD_HEIGHT = 18f;
        public const float FIELD_SPACING = 4;
        public const float WARNING_HEIGHT = 64;
        public const float VERTICAL_FIELD_HEIGHT = FIELD_HEIGHT * 2 + FIELD_SPACING;

        private static bool m_guiIndented;

        private static readonly Stack<float?> LabelWidthStack = new Stack<float?>();
        private static readonly Stack<float?> FieldWidthStack = new Stack<float?>();

        private static readonly Dictionary<string, bool> FoldoutByName;

        static MightyGUIUtilities() => FoldoutByName = new Dictionary<string, bool>();

        #region Indent & Space

        public static int GetCurrentIndentWidth() => GetIndentWidth(EditorGUI.indentLevel);

        public static int GetIndentWidth(int indentLevel) => TAB_SIZE * indentLevel;

        public static void IndentOnce()
        {
            GUILayout.BeginHorizontal();
            GetIndentWidth(1);
            GUILayout.EndHorizontal();
        }

        public static Rect JumpLine(Rect position, bool reduceHeight = true)
        {
            position.y += FIELD_HEIGHT + FIELD_SPACING;
            if (reduceHeight) position.height -= FIELD_HEIGHT + FIELD_SPACING;
            return position;
        }

        public static Rect JumpHeight(Rect position, float height)
        {
            position.y += height;
            position.height -= height;
            return position;
        }

        public static bool BeginLabelWidth(float? width)
        {
            if (width == null)
            {
                LabelWidthStack.Push(null);
                return false;
            }

            LabelWidthStack.Push(EditorGUIUtility.labelWidth);
            EditorGUIUtility.labelWidth = (float) width;

            return true;
        }

        public static bool EndLabelWidth()
        {
            if (LabelWidthStack.Count == 0)
            {
                Debug.LogError("EndLabelWidth: BeginLabelWidth must be called first.");
                return false;
            }

            var width = LabelWidthStack.Pop();
            if (width == null) return false;

            EditorGUIUtility.labelWidth = (float) width;
            return true;
        }

        public static bool BeginFieldWidth(float? width)
        {
            if (width == null)
            {
                FieldWidthStack.Push(null);
                return false;
            }

            FieldWidthStack.Push(EditorGUIUtility.fieldWidth);
            EditorGUIUtility.fieldWidth = (float) width;

            return true;
        }

        public static bool EndFieldWidth()
        {
            if (FieldWidthStack.Count == 0)
            {
                Debug.LogError("EndFieldWidth: BeginFieldWidth must be called first.");
                return false;
            }

            var width = FieldWidthStack.Pop();
            if (width == null) return false;

            EditorGUIUtility.fieldWidth = (float) width;
            return true;
        }

        #endregion /Indent & Space

        #region ScrollView

        public static Vector2 DrawScrollView(Vector2 scrollPosition, Action drawAction, params GUILayoutOption[] options)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, options);
            drawAction();
            EditorGUILayout.EndScrollView();
            return scrollPosition;
        }

        public static Vector2 BeginDrawScrollView(Vector2 scrollPosition, params GUILayoutOption[] options) =>
            EditorGUILayout.BeginScrollView(scrollPosition, options);

        public static void EndDrawScrollView() => EditorGUILayout.EndScrollView();

        #endregion /ScrollView

        #region Foldout

        public static bool DrawFoldout(SerializedProperty property, GUIStyle style = null) =>
            property.isExpanded = DrawFoldout(property.isExpanded, property.displayName, style);

        public static bool DrawFoldout(SerializedProperty property, GUIContent label, GUIStyle style = null) =>
            property.isExpanded = DrawFoldout(property.isExpanded, label, style);

        public static bool DrawFoldout(bool foldout, string label, GUIStyle style = null)
        {
            var changed = GUI.changed;
            foldout = EditorGUILayout.Foldout(foldout, label, true, style ?? EditorStyles.foldout);
            GUI.changed = changed;
            return foldout;
        }

        public static bool DrawFoldout(bool foldout, GUIContent label, GUIStyle style = null)
        {
            var changed = GUI.changed;
            foldout = EditorGUILayout.Foldout(foldout, label, true, style ?? EditorStyles.foldout);
            GUI.changed = changed;
            return foldout;
        }

        public static bool DrawFoldout(Rect position, SerializedProperty property, GUIStyle style = null) =>
            property.isExpanded = DrawFoldout(position, property.isExpanded, property.displayName, style);

        public static bool DrawFoldout(Rect position, SerializedProperty property, GUIContent label, GUIStyle style = null) =>
            property.isExpanded = DrawFoldout(position, property.isExpanded, label, style);

        public static bool DrawFoldout(Rect position, bool foldout, string label, GUIStyle style = null)
        {
            var changed = GUI.changed;
            foldout = EditorGUI.Foldout(position, foldout, label, true, style ?? EditorStyles.foldout);
            GUI.changed = changed;
            return foldout;
        }

        public static bool DrawFoldout(Rect position, bool foldout, GUIContent label, GUIStyle style = null)
        {
            var changed = GUI.changed;
            foldout = EditorGUI.Foldout(position, foldout, label, true, style ?? EditorStyles.foldout);
            GUI.changed = changed;
            return foldout;
        }


        public static void SetFoldout(string name, bool foldout)
        {
            if (!FoldoutByName.ContainsKey(name)) FoldoutByName.Add(name, foldout);
            else FoldoutByName[name] = foldout;
        }

        public static bool GetFoldout(string name)
        {
            if (!FoldoutByName.ContainsKey(name)) FoldoutByName.Add(name, false);
            return FoldoutByName[name];
        }

        #endregion /Foldout

        #region Box

        private static bool DrawFoldArea(string label, bool foldout, Indent indent, GUIStyle labelStyle = null,
            bool otherFoldoutCondition = false)
        {
            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;

            GUI.backgroundColor = foldout ? backgroundColor : MightyColorUtilities.DarkenColor(backgroundColor, .3f);
            GUI.contentColor = foldout ? contentColor : MightyColorUtilities.DarkenColor(contentColor, .25f);

            GUILayout.BeginVertical(MightyStyleUtilities.GetFoldAreaHeader(EditorGUI.indentLevel));

            if (indent.HasFlag(Indent.Label))
                EditorGUI.indentLevel++;

            foldout = EditorGUILayout.Foldout(foldout, label, true, labelStyle ?? MightyStyles.FoldAreaLabel) ||
                      otherFoldoutCondition;

            switch (indent)
            {
                case Indent.Label:
                    EditorGUI.indentLevel--;
                    break;
                case Indent.Content:
                    EditorGUI.indentLevel++;
                    break;
            }

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;

            return foldout;
        }

        public static bool DrawFoldArea(string label, bool foldout, Action drawAction, Indent indent = Indent.Nothing,
            GUIStyle labelStyle = null) => DrawFoldArea(label, foldout, true, drawAction, indent, labelStyle);

        public static bool DrawFoldArea(string label, bool foldout, bool hasContent, Action drawAction, Indent indent = Indent.Nothing,
            GUIStyle labelStyle = null)
        {
            if (foldout = BeginDrawFoldArea(label, foldout, hasContent, indent, labelStyle)) drawAction();
            EndDrawFoldArea(indent);
            return foldout;
        }

        public static bool BeginDrawFoldArea(string label, bool foldout, bool hasContent = true, Indent indent = Indent.Nothing,
            GUIStyle labelStyle = null)
        {
            if (!DrawFoldArea(label, foldout, indent, labelStyle, hasContent))
                return false;

            if (hasContent)
                DrawLine();
            return true;
        }

        public static void EndDrawFoldArea(Indent indent = Indent.Nothing) => EndDrawVertical(indent);

        #endregion /Box

        #region Reorderable

        public static void DrawReorderableList(ReorderableList list, SerializedProperty property, string label, ref bool foldout,
            float? height = null, ReorderableList.HeaderCallbackDelegate drawHeaderCallback = null,
            ReorderableList.ElementCallbackDelegate drawElementCallback = null)
        {
            if (foldout = DrawFoldout(foldout, label)) DrawReorderableList(list, property, height, drawHeaderCallback, drawElementCallback);
        }

        public static void DrawReorderableList(ReorderableList list, SerializedProperty property, float? height = null,
            ReorderableList.HeaderCallbackDelegate drawHeaderCallback = null,
            ReorderableList.ElementCallbackDelegate drawElementCallback = null)
        {
            list.drawHeaderCallback = drawHeaderCallback ?? (rect =>
            {
                EditorGUI.BeginChangeCheck();
                property.arraySize = Mathf.Max(0, EditorGUI.DelayedIntField(rect, "Size", property.arraySize));
                if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
            });

            list.drawElementCallback = drawElementCallback ?? ((rect, index, isActive, isFocused) =>
            {
                var element = property.GetArrayElementAtIndex(index);
                rect.y += 2f;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, height ?? EditorGUIUtility.singleLineHeight), element);
            });

            list.DoLayoutList();
            if (property.serializedObject.hasModifiedProperties)
                property.serializedObject.ApplyModifiedProperties();
        }

        #endregion /Reorderable

        #region Label

        public static float TextWidth(string text)
        {
            var font = GUI.skin.font;
            var width = 0f;
            foreach (var ch in text)
                if (font.GetCharacterInfo(ch, out var info))
                    width += info.glyphWidth;

            return width;
        }

        public static float TextHeight(string text, int minimumLinesCount)
        {
            var linesCount = 1;
            foreach (var ch in text)
                if (ch == '\n')
                    linesCount++;

            if (linesCount < 3) linesCount = 3;

            return linesCount * CHAR_HEIGHT + END_SPACE;
        }

        public static string GetPrettyName(this string name, bool removeInterfacePrefix = false)
        {
            name = name.Replace("m_", "").Replace("_", "");
            name = Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
            name = Regex.Replace(name, "([a-z])([0-9])", "$1 $2");

            if (removeInterfacePrefix && name[0] == 'I')
                name = name.Remove(0, 1);

            return name.UpperFirstChar();
        }

        public static string UpperFirstChar(this string value)
        {
            var array = value.ToCharArray();
            array[0] = char.ToUpper(value[0]);
            return new string(array);
        }

        #endregion /Label

        #region Title

        public static void DrawTitle(string header)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        }

        public static void DrawTitle(Rect position, string header)
        {
            EditorGUI.IndentedRect(position);
            EditorGUI.LabelField(position, header, EditorStyles.boldLabel);
        }

        #endregion /Title

        #region Line

        public static void DrawLine(Color? color = null, bool drawSpace = true)
        {
            var previousColor = GUI.backgroundColor;
            var colorNotNull = color != null;
            if (colorNotNull)
                GUI.backgroundColor = (Color) color;

            GUILayout.Box(GUIContent.none, MightyStyleUtilities.GetLine(drawSpace));

            if (colorNotNull)
                GUI.backgroundColor = previousColor;
        }

        public static Rect DrawLine(Rect rect, Color? color = null, bool drawSpace = true)
        {
            var previousColor = GUI.color;
            GUI.color = color ?? previousColor;

            var lineStyle = MightyStyleUtilities.GetLine(drawSpace);

            rect.y += 1;
            GUI.Box(rect, GUIContent.none, lineStyle);
            rect.y += lineStyle.GetStyleHeight();

            GUI.color = previousColor;
            return rect;
        }

        #endregion /Line

        #region HelpBox

        public static void DrawHelpBox(string infoText, HelpBoxType infoBoxType)
        {
            switch (infoBoxType)
            {
                case HelpBoxType.Info:
                    DrawHelpBox(infoText, MessageType.Info);
                    break;

                case HelpBoxType.Warning:
                    DrawHelpBox(infoText);
                    break;

                case HelpBoxType.Error:
                    DrawHelpBox(infoText, MessageType.Error);
                    break;
            }
        }

        public static void DrawHelpBox(string message, MessageType type = MessageType.Warning, Object context = null,
            bool logToConsole = false)
        {
            EditorGUILayout.HelpBox(message, type);

            message = $"[Mighty]Attributes - {message}";

            if (logToConsole) MightyDebugUtilities.MightyDebug(message, type, context);
        }

        public static void DrawHelpBox(Rect position, string message, MessageType type = MessageType.Warning, Object context = null,
            bool logToConsole = false)
        {
            position.height -= 3;
            EditorGUI.HelpBox(position, message, type);

            if (logToConsole) MightyDebugUtilities.MightyDebug(message, type, context);
        }

        #endregion /HelpBox

        #region Align

        public static void BeginDrawAlign(Align align)
        {
            GUILayout.BeginHorizontal();
            switch (align)
            {
                case Align.Left:
                    GUILayout.Space(EditorGUI.indentLevel * 20);
                    break;
                case Align.Center:
                    GUILayout.FlexibleSpace();
                    break;
                case Align.Right:
                    GUILayout.FlexibleSpace();
                    break;
            }
        }

        public static void EndDrawAlign(Align align)
        {
            switch (align)
            {
                case Align.Center:
                    GUILayout.FlexibleSpace();
                    break;
                case Align.Left:
                    GUILayout.FlexibleSpace();
                    break;
            }

            GUILayout.EndHorizontal();
        }

        public static Rect GetAlignPosition(Rect currentPosition, float contentWidth, float contentHeight, Align align)
        {
            switch (align)
            {
                case Align.Left:
                    return new Rect(currentPosition.x, currentPosition.y, contentWidth, contentHeight);
                case Align.Center:
                    return new Rect(Screen.width / 2f - contentWidth / 2f + TAB_SIZE, currentPosition.y, contentWidth, contentHeight);
                case Align.Right:
                    return new Rect(Screen.width - contentWidth - TAB_SIZE, currentPosition.y, contentWidth, contentHeight);
            }

            return currentPosition;
        }

        #endregion /Align

        #region Layout

        public static void LayoutIndent(int indent = 1) => GUILayoutUtility.GetRect(TAB_SIZE * indent, 0);

        public static void DrawLayoutIndent() => GUILayout.Space(GetCurrentIndentWidth());

        public static void BeginLayoutIndent()
        {
            EditorGUILayout.BeginHorizontal();
            DrawLayoutIndent();
        }

        public static void EndLayoutIndent() => EditorGUILayout.EndHorizontal();

        public static void DrawVertical(Action drawAction, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            drawAction();
            GUILayout.EndVertical();
        }

        public static void DrawVertical(Action drawAction, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            drawAction();
            GUILayout.EndVertical();
        }

        public static void DrawHorizontal(Action drawAction, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            drawAction();
            GUILayout.EndHorizontal();
        }

        public static void DrawHorizontal(Action drawAction, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            drawAction();
            GUILayout.EndHorizontal();
        }

        private static void EndDrawVertical(Indent indent)
        {
            if (indent.HasFlag(Indent.Content))
                EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }

        public static Rect GetInspectorWidthRect(float? height = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            var rect = GUILayoutUtility.GetLastRect();

            if (height != null) rect.height = (float) height;

            return rect;
        }

        #endregion /Layout

        #region Array

        public delegate void ElementDrawer(int index);

        public static void DrawArraySizeField(SerializedProperty property)
        {
            if (!property.IsCollection() || !property.isExpanded) return;

            EditorGUI.BeginChangeCheck();
            property.arraySize = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size", property.arraySize));
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
        }

        public static Rect DrawArraySizeField(Rect position, SerializedProperty property)
        {
            if (!property.IsCollection() || !property.isExpanded) return position;

            EditorGUI.BeginChangeCheck();
            property.arraySize = Mathf.Max(0, EditorGUI.DelayedIntField(position, "Size", property.arraySize));
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

            return JumpLine(position);
        }

        public static void DrawArrayBody(SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.IsCollection() || !property.isExpanded) return;

            for (var i = 0; i < property.arraySize; i++)
                elementDrawer(i);
        }

        public static void DrawArrayBody(SerializedProperty property)
        {
            if (!property.IsCollection() || !property.isExpanded) return;

            for (var i = 0; i < property.arraySize; i++)
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
        }

        public static void DrawArrayBody(Rect position, SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.IsCollection() || !property.isExpanded) return;

            for (var i = 0; i < property.arraySize; i++)
                elementDrawer(i);
        }

        public static void DrawArray(SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.IsCollection()) return;

            if (!DrawFoldout(property)) return;

            EditorGUI.indentLevel++;
            DrawArraySizeField(property);
            DrawArrayBody(property, elementDrawer);
            EditorGUI.indentLevel--;
        }

        public static void DrawArray(SerializedProperty property)
        {
            if (!property.IsCollection()) return;

            if (!DrawFoldout(property)) return;

            EditorGUI.indentLevel++;
            DrawArraySizeField(property);
            DrawArrayBody(property);
            EditorGUI.indentLevel--;
        }

        public static void DrawArray(Rect position, SerializedProperty property, ElementDrawer elementDrawer, bool fromBox)
        {
            if (!property.IsCollection()) return;

            if (!DrawFoldout(position, property)) return;

            position.x += 15;
            position.width -= 15;
            DrawArraySizeField(position, property);
            DrawArrayBody(position, property, elementDrawer);
        }

        #endregion /Array

        #region Buttons

        public static bool DrawAddButton() => GUILayout.Button(DrawIcon(IconName.PLUS), GUILayout.Height(25), GUILayout.Width(25));
        public static bool DrawRemoveButton() => GUILayout.Button(DrawIcon(IconName.MINUS), GUILayout.Height(25), GUILayout.Width(25));

        public static void DrawComponentFromSelf<T>(MonoBehaviour source, ref T destination, params GUILayoutOption[] options)
            where T : Component => DrawComponentFromSelf($"{typeof(T).Name} From Self", source, ref destination, options);

        public static void DrawComponentFromSelf<T>(string label, MonoBehaviour source, ref T destination, params GUILayoutOption[] options)
            where T : Component
        {
            var value = destination;
            DrawButton(label, () => value = source.GetComponent<T>(), options.Length > 0 ? options : new[] {GUILayout.Height(20)});
            destination = value;
        }

        public static bool DrawToggleButton(bool value, string offIconName, string onIconName, string label, Color offColor, Color onColor,
            params GUILayoutOption[] options) =>
            DrawToggleButton(value, DrawIconLabel(value ? onIconName : offIconName, label), offColor, onColor, options);

        public static bool DrawToggleButton(bool value, string iconName, string label, Color offColor, Color onColor,
            params GUILayoutOption[] options) => DrawToggleButton(value, DrawIconLabel(iconName, label), offColor, onColor, options);

        public static bool DrawToggleButton(bool value, GUIContent label, Color offColor, Color onColor, params GUILayoutOption[] options)
        {
            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;
            GUI.backgroundColor = value ? onColor : offColor;
            GUI.contentColor = value ? Color.white : Color.gray;

            value = GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;
            return value;
        }

        public static bool DrawToggleButton(bool value, string label, Color offColor, Color onColor, params GUILayoutOption[] options)
        {
            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;
            GUI.backgroundColor = value ? onColor : offColor;
            GUI.contentColor = value ? Color.white : Color.gray;

            value = GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;
            return value;
        }

        public static bool DrawToggleButton(bool value, GUIContent label, Color color, params GUILayoutOption[] options)
        {
            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;
            GUI.backgroundColor = value ? color : MightyColorUtilities.DarkenColor(color, 0.5f);
            GUI.contentColor = value ? Color.white : Color.gray;

            value = GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;
            return value;
        }

        public static bool DrawToggleButton(bool value, string label, Color color, params GUILayoutOption[] options)
        {
            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;
            GUI.backgroundColor = value ? color : MightyColorUtilities.DarkenColor(color, 0.5f);
            GUI.contentColor = value ? Color.white : Color.gray;

            value = GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;
            return value;
        }

        public static bool DrawToggleButton(bool value, string iconName, string label, params GUILayoutOption[] options) =>
            DrawToggleButton(value, DrawIconLabel(iconName, label), options);

        public static bool DrawToggleButton(bool value, GUIContent label, params GUILayoutOption[] options) =>
            GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

        public static bool DrawToggleButton(bool value, string label, params GUILayoutOption[] options) =>
            GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

        public static void DrawButton(Action action, GUIStyle style, params GUILayoutOption[] options) =>
            DrawButton(action.Method.Name, action, style, options);

        public static void DrawButton(Action action, bool addSpace = true, params GUILayoutOption[] options) =>
            DrawButton(addSpace ? GetPrettyName(action.Method.Name) : action.Method.Name, action, options);

        public static void DrawButton(string iconName, string label, Action action, float height) =>
            DrawButton(iconName, label, action, GUILayout.Height(height));

        public static void DrawButton(string iconName, string label, Action action, params GUILayoutOption[] options) =>
            DrawButton(DrawIconLabel(iconName, label), action, options);

        public static void DrawButton(GUIContent content, Action action, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(content, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
                action();
        }

        public static void DrawButton(string iconName, string label, Action action, GUIStyle style, params GUILayoutOption[] options) =>
            DrawButton(DrawIconLabel(iconName, label), action, style, options);

        public static void DrawButton(GUIContent content, Action action, GUIStyle style, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(content, style, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
                action();
        }

        public static void DrawButton(string label, Action action, float height) => DrawButton(label, action, GUILayout.Height(height));

        public static void DrawButton(string label, Action action, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
                action();
        }

        public static void DrawButton(string label, Action action, GUIStyle style, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, style, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
                action();
        }

        public static bool DrawButton(string iconName, string label, float height) =>
            DrawButton(DrawIconLabel(iconName, label), GUILayout.Height(height));

        public static bool DrawButton(string iconName, string label, params GUILayoutOption[] options) =>
            DrawButton(DrawIconLabel(iconName, label), options);

        public static bool DrawButton(string iconName, string label, GUIStyle style, params GUILayoutOption[] options) =>
            DrawButton(DrawIconLabel(iconName, label), style, options);

        public static bool DrawButton(string label, float height) => DrawButton(label, GUILayout.Height(height));

        public static bool DrawButton(string label, params GUILayoutOption[] options) =>
            GUILayout.Button(label, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

        public static bool DrawButton(string label, GUIStyle style, params GUILayoutOption[] options) =>
            GUILayout.Button(label, style, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

        public static bool DrawButton(GUIContent content, params GUILayoutOption[] options) =>
            GUILayout.Button(content, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

        public static bool DrawButton(GUIContent content, GUIStyle style, params GUILayoutOption[] options) =>
            GUILayout.Button(content, style, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

        #endregion /Buttons

        #region Dropdown

        public static object DrawDropdown(GUIContent label, int selectedValueIndex, object[] values, string[] displayOptions,
            FieldOption option = FieldOption.Nothing)
        {
            EditorGUI.BeginChangeCheck();

            int index;
            if (option.Contains(FieldOption.HideLabel))
                index = EditorGUILayout.Popup(selectedValueIndex, displayOptions);
            else if (option.Contains(FieldOption.BoldLabel))
                index = EditorGUILayout.Popup(label.text, selectedValueIndex, displayOptions, EditorStyles.boldLabel);
            else
                index = EditorGUILayout.Popup(label, selectedValueIndex, displayOptions);

            return !EditorGUI.EndChangeCheck() ? null : values[index];
        }

        public static object DrawDropdown(Rect position, string label, int selectedValueIndex, object[] values, string[] displayOptions,
            FieldOption option = FieldOption.Nothing)
        {
            EditorGUI.BeginChangeCheck();

            int index;
            if (option.Contains(FieldOption.HideLabel))
                index = EditorGUI.Popup(position, selectedValueIndex, displayOptions);
            else if (option.Contains(FieldOption.BoldLabel))
                index = EditorGUI.Popup(position, label, selectedValueIndex, displayOptions, EditorStyles.boldLabel);
            else
                index = EditorGUI.Popup(position, label, selectedValueIndex, displayOptions);

            return !EditorGUI.EndChangeCheck() ? null : values[index];
        }

        #endregion /Dropdown

        #region Icon

        public static Texture2D GetTexture(string texturePath)
        {
            if (!texturePath.StartsWith("Assets/")) texturePath = Path.Combine("Assets", texturePath);
            return AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
        }

        public static GUIContent DrawIconLabel(string iconName, string label) => new GUIContent(DrawIcon(iconName)) {text = $" {label}"};

        public static GUIContent DrawIcon(string iconName) => EditorGUIUtility.IconContent(iconName);

        #endregion /Icon

        #region Dialog

        public static bool ConfirmDialog(string title, string content) => EditorUtility.DisplayDialog(title, content, "Confirm", "Cancel");

        #endregion /Dialog

        #region Fields

        public static void FieldWidth(Action fieldDrawer, float labelWidth, float? fieldWidth = null)
        {
            var previousLabelWidth = EditorGUIUtility.labelWidth;
            var previousFieldWidth = EditorGUIUtility.fieldWidth;

            EditorGUIUtility.labelWidth = labelWidth;
            if (fieldWidth != null) EditorGUIUtility.fieldWidth = (float) fieldWidth;

            fieldDrawer();

            EditorGUIUtility.labelWidth = previousLabelWidth;
            if (fieldWidth != null) EditorGUIUtility.fieldWidth = previousFieldWidth;
        }

        public static void DrawPrefabSensitiveField(SerializedProperty property, Rect position, Action<GUIContent> fieldDrawer)
        {
            var label = EditorGUI.BeginProperty(position, new GUIContent(property.displayName), property);
            fieldDrawer.Invoke(label);
            EditorGUI.EndProperty();
        }

        public static void DrawPropertyField(SerializedProperty property, GUIContent label = null, bool includeChildren = true)
        {
            if (label != null)
                EditorGUILayout.PropertyField(property, label, includeChildren);
            else
                EditorGUILayout.PropertyField(property, includeChildren);
        }

        public static Rect DrawPropertyField(Rect position, SerializedProperty property, GUIContent label = null,
            bool includeChildren = true)
        {
            if (label != null)
                EditorGUI.PropertyField(position, property, label, includeChildren);
            else
                EditorGUI.PropertyField(position, property, includeChildren);

            return JumpLine(position);
        }

        public static bool DrawLayoutField(string label, object value, Type valueType, bool enabled)
        {
            GUI.enabled = enabled;

            var isDrawn = true;

            if (valueType.IsArray)
                return DrawArrayLayoutField(label, value, valueType.GetElementType(), enabled);

            if (valueType.IsEnum)
            {
                if (valueType.GetCustomAttribute<FlagsAttribute>() != null)
                    EditorGUILayout.MaskField(label, Convert.ToInt32(value), Enum.GetNames(valueType));
                else
                    EditorGUILayout.EnumPopup(label, (Enum) value);
            }
            else if (valueType == typeof(bool))
                EditorGUILayout.Toggle(label, (bool) value);
            else if (valueType == typeof(int))
                EditorGUILayout.IntField(label, (int) value);
            else if (valueType == typeof(short))
                EditorGUILayout.IntField(label, (short) value);
            else if (valueType == typeof(byte))
                EditorGUILayout.IntField(label, (byte) value);
            else if (valueType == typeof(uint))
                EditorGUILayout.IntField(label, (int) (uint) value);
            else if (valueType == typeof(ushort))
                EditorGUILayout.IntField(label, (ushort) value);
            else if (valueType == typeof(long))
                EditorGUILayout.LongField(label, (long) value);
            else if (valueType == typeof(ulong))
                EditorGUILayout.LongField(label, (long) (ulong) value);
            else if (valueType == typeof(float))
                EditorGUILayout.FloatField(label, (float) value);
            else if (valueType == typeof(double))
                EditorGUILayout.DoubleField(label, (double) value);
            else if (valueType == typeof(string))
                EditorGUILayout.TextField(label, (string) value);
            else if (valueType == typeof(Vector2))
                EditorGUILayout.Vector2Field(label, (Vector2) value);
            else if (valueType == typeof(Vector3))
                EditorGUILayout.Vector3Field(label, (Vector3) value);
            else if (valueType == typeof(Vector4))
                EditorGUILayout.Vector4Field(label, (Vector4) value);
            else if (valueType == typeof(Vector2Int))
                EditorGUILayout.Vector2IntField(label, (Vector2Int) value);
            else if (valueType == typeof(Vector3Int))
                EditorGUILayout.Vector3IntField(label, (Vector3Int) value);
            else if (valueType == typeof(Rect))
                EditorGUILayout.RectField(label, (Rect) value);
            else if (valueType == typeof(Bounds))
                EditorGUILayout.BoundsField(label, (Bounds) value);
            else if (valueType == typeof(RectInt))
                EditorGUILayout.RectIntField(label, (RectInt) value);
            else if (valueType == typeof(BoundsInt))
                EditorGUILayout.BoundsIntField(label, (BoundsInt) value);
            else if (valueType == typeof(Quaternion))
                Quaternion.Euler(EditorGUILayout.Vector3Field(label, ((Quaternion) value).eulerAngles));
            else if (valueType == typeof(LayerMask))
                EditorGUILayout.MaskField(label, InternalEditorUtility.LayerMaskToConcatenatedLayersMask((LayerMask) value),
                    InternalEditorUtility.layers);
            else if (valueType == typeof(Color))
                EditorGUILayout.ColorField(label, (Color) value);
            else if (valueType == typeof(AnimationCurve))
                EditorGUILayout.CurveField(label, (AnimationCurve) value);
            else if (valueType == typeof(Gradient))
                EditorGUILayout.GradientField(label, (Gradient) value);
            else if (typeof(Object).IsAssignableFrom(valueType))
                EditorGUILayout.ObjectField(label, (Object) value, valueType, true);
            else if (valueType.GetCustomAttribute(typeof(SerializableAttribute), true) != null)
            {
                if (!EditorGUILayout.Foldout(GetFoldout(label), label, true))
                {
                    SetFoldout(label, false);
                    return true;
                }

                SetFoldout(label, true);
                EditorGUI.indentLevel++;
                foreach (var field in valueType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.IsPublic || f.GetCustomAttribute(typeof(SerializeField)) != null))
                {
                    DrawLayoutField(field.Name.GetPrettyName(), field.GetValue(value), field.FieldType, enabled);
                }

                EditorGUI.indentLevel--;
            }
            else
                isDrawn = false;

            GUI.enabled = true;

            return isDrawn;
        }

        public static object DrawLayoutField(string fieldName, Object context, object target, bool foldProperty = true)
            => DrawLayoutField(ReflectionUtilities.GetField(target.GetType(), fieldName), context, target, foldProperty);

        public static object DrawLayoutField(FieldInfo field, Object context, object target, bool foldProperty = true)
            => field == null ? null : DrawLayoutField(field, context, target, field.GetValue(target), foldProperty);

        public static object DrawLayoutField(FieldInfo field, Object context, object target, object value,
            bool foldProperty = true, bool isAsset = false)
        {
            value = DrawLayoutField(context, field.Name, field.FieldType, field.Name.GetPrettyName(), value, foldProperty,
                isAsset);
            field.SetValue(target, value);

            return value;
        }

        public static T DrawGenericLayoutField<T>(string label, T value, bool foldProperty = true, bool isAsset = false) =>
            (T) DrawLayoutField(null, label, typeof(T), label, value, foldProperty, isAsset);

        public static object DrawLayoutField(Object context, string fieldName, Type type, string label, object value,
            bool foldProperty = true, bool isAsset = false)
        {
            if (typeof(IList).IsAssignableFrom(type))
                return DrawArrayLayoutField(context, fieldName, type.GetElementType(), label, value);

            if (type.IsEnum || type == typeof(Enum))
            {
                if (type.GetCustomAttribute<FlagsAttribute>() != null)
                    return EditorGUILayout.MaskField(label, Convert.ToInt32(value), Enum.GetNames(type));
                return EditorGUILayout.EnumPopup(label, (Enum) Enum.ToObject(type, (int) value));
            }

            if (type == typeof(bool))
                return EditorGUILayout.Toggle(label, (bool) value);
            if (type == typeof(int))
                return EditorGUILayout.IntField(label, (int) value);
            if (type == typeof(uint))
                return EditorGUILayout.IntField(label, (int) (uint) value);
            if (type == typeof(short))
                return EditorGUILayout.IntField(label, (short) value);
            if (type == typeof(ushort))
                return EditorGUILayout.IntField(label, (ushort) value);
            if (type == typeof(byte))
                return EditorGUILayout.IntField(label, (byte) value);
            if (type == typeof(long))
                return EditorGUILayout.LongField(label, (long) value);
            if (type == typeof(ulong))
                return EditorGUILayout.LongField(label, (long) (ulong) value);
            if (type == typeof(float))
                return EditorGUILayout.FloatField(label, (float) value);
            if (type == typeof(double))
                return EditorGUILayout.DoubleField(label, (double) value);
            if (type == typeof(string))
                return EditorGUILayout.TextField(label, (string) value);
            if (type == typeof(Vector2))
                return EditorGUILayout.Vector2Field(label, (Vector2) value);
            if (type == typeof(Vector3))
                return EditorGUILayout.Vector3Field(label, (Vector3) value);
            if (type == typeof(Vector4))
                return EditorGUILayout.Vector4Field(label, (Vector4) value);
            if (type == typeof(Vector2Int))
                return EditorGUILayout.Vector2IntField(label, (Vector2Int) value);
            if (type == typeof(Vector3Int))
                return EditorGUILayout.Vector3IntField(label, (Vector3Int) value);
            if (type == typeof(Rect))
                return EditorGUILayout.RectField(label, (Rect) value);
            if (type == typeof(Bounds))
                return EditorGUILayout.BoundsField(label, (Bounds) value);
            if (type == typeof(RectInt))
                return EditorGUILayout.RectIntField(label, (RectInt) value);
            if (type == typeof(BoundsInt))
                return EditorGUILayout.BoundsIntField(label, (BoundsInt) value);
            if (type == typeof(Quaternion))
                return Quaternion.Euler(EditorGUILayout.Vector3Field(label, ((Quaternion) value).eulerAngles));
            if (type == typeof(LayerMask))
                return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(EditorGUILayout.MaskField(label,
                    InternalEditorUtility.LayerMaskToConcatenatedLayersMask((LayerMask) value),
                    InternalEditorUtility.layers));
            if (type == typeof(Color))
                return EditorGUILayout.ColorField(label, (Color) value);
            if (type == typeof(AnimationCurve))
                return EditorGUILayout.CurveField(label, (AnimationCurve) value);
            if (type == typeof(Gradient))
                return EditorGUILayout.GradientField(label, (Gradient) value);
            if (typeof(Component).IsAssignableFrom(type))
                return EditorGUILayout.ObjectField(label, (Object) value, type, !isAsset);
            if (typeof(Object).IsAssignableFrom(type))
                return EditorGUILayout.ObjectField(label, (Object) value, type, false);

            if (type.GetCustomAttribute(typeof(SerializableAttribute), true) == null) return value;

            if (foldProperty)
            {
                var pathName = EditorSerializedFieldUtilities.GenerateFileName(context, fieldName);
                if (!EditorGUILayout.Foldout(GetFoldout(pathName), label, true))
                {
                    SetFoldout(pathName, false);
                    return value;
                }

                SetFoldout(pathName, true);
                EditorGUI.indentLevel++;
            }

            foreach (var field in type.GetSerializableFields(true, true))
            {
                var fieldValue = DrawLayoutField(context, $"{fieldName}.{field.Name}", field.FieldType,
                    field.Name.GetPrettyName(), field.GetValue(value));

                if (field.FieldType.IsEnum)
                    fieldValue = Enum.ToObject(field.FieldType, fieldValue);

                field.SetValue(value, fieldValue);
            }

            if (!foldProperty) return value;

            EditorGUI.indentLevel--;

            return value;
        }

        public static bool DrawArrayLayoutField(string label, object arrayValue, Type elementType, bool enabled)
        {
            if (elementType.IsEnum)
            {
                if (elementType.GetCustomAttribute<FlagsAttribute>() != null)
                    return DrawArray(label, (IList<int>) arrayValue, enabled);
                return DrawArray(label, (IList<Enum>) arrayValue, enabled);
            }

            if (elementType == typeof(bool))
                return DrawArray(label, (IList<bool>) arrayValue, enabled);
            
            if (elementType == typeof(int))
                return DrawArray(label, (IList<int>) arrayValue, enabled);
            if (elementType == typeof(uint))
                return DrawArray(label, (IList<uint>) arrayValue, enabled);
            if (elementType == typeof(short))
                return DrawArray(label, (IList<short>) arrayValue, enabled);
            if (elementType == typeof(ushort))
                return DrawArray(label, (IList<ushort>) arrayValue, enabled);
            if (elementType == typeof(byte))
                return DrawArray(label, (IList<byte>) arrayValue, enabled);

            if (elementType == typeof(long))
                return DrawArray(label, (IList<long>) arrayValue, enabled);
            if (elementType == typeof(ulong))
                return DrawArray(label, (IList<ulong>) arrayValue, enabled);

            if (elementType == typeof(float))
                return DrawArray(label, (IList<float>) arrayValue, enabled);

            if (elementType == typeof(double))
                return DrawArray(label, (IList<double>) arrayValue, enabled);

            if (elementType == typeof(string))
                return DrawArray(label, (IList<string>) arrayValue, enabled);

            if (elementType == typeof(Vector2))
                return DrawArray(label, (IList<Vector2>) arrayValue, enabled);

            if (elementType == typeof(Vector3))
                return DrawArray(label, (IList<Vector3>) arrayValue, enabled);

            if (elementType == typeof(Vector4))
                return DrawArray(label, (IList<Vector4>) arrayValue, enabled);

            if (elementType == typeof(Vector2Int))
                return DrawArray(label, (IList<Vector2Int>) arrayValue, enabled);

            if (elementType == typeof(Vector3Int))
                return DrawArray(label, (IList<Vector3Int>) arrayValue, enabled);

            if (elementType == typeof(Rect))
                return DrawArray(label, (IList<Rect>) arrayValue, enabled);

            if (elementType == typeof(Bounds))
                return DrawArray(label, (IList<Bounds>) arrayValue, enabled);

            if (elementType == typeof(RectInt))
                return DrawArray(label, (IList<RectInt>) arrayValue, enabled);

            if (elementType == typeof(BoundsInt))
                return DrawArray(label, (IList<BoundsInt>) arrayValue, enabled);

            if (elementType == typeof(Quaternion))
                return DrawArray(label, (IList<Quaternion>) arrayValue, enabled);

            if (elementType == typeof(LayerMask))
                return DrawArray(label, (IList<LayerMask>) arrayValue, enabled);

            if (elementType == typeof(Color))
                return DrawArray(label, (IList<Color>) arrayValue, enabled);

            if (elementType == typeof(AnimationCurve))
                return DrawArray(label, (IList<AnimationCurve>) arrayValue, enabled);

            if (elementType == typeof(Gradient))
                return DrawArray(label, (IList<Gradient>) arrayValue, enabled);

            if (typeof(Object).IsAssignableFrom(elementType))
                return DrawArray(label, (IList<Object>) arrayValue, enabled);

            return false;
        }

        public static object DrawArrayLayoutField(Object context, string fieldName, Type elementType, string label,
            object arrayValue)
        {
            if (elementType.IsEnum)
            {
                if (elementType.GetCustomAttribute<FlagsAttribute>() != null)
                    return DrawArrayField(context, fieldName, label, (IList<int>) arrayValue);
                return DrawArrayField(context, fieldName, label, (IList<int>) arrayValue, elementType);
            }

            if (elementType == typeof(bool))
                return DrawArrayField(context, fieldName, label, (IList<bool>) arrayValue);

            if (elementType == typeof(int))
                return DrawArrayField(context, fieldName, label, (IList<int>) arrayValue);
            if (elementType == typeof(uint))
                return DrawArrayField(context, fieldName, label, (IList<uint>) arrayValue);
            if (elementType == typeof(short))
                return DrawArrayField(context, fieldName, label, (IList<short>) arrayValue);
            if (elementType == typeof(ushort))
                return DrawArrayField(context, fieldName, label, (IList<ushort>) arrayValue);
            if (elementType == typeof(byte))
                return DrawArrayField(context, fieldName, label, (IList<byte>) arrayValue);

            if (elementType == typeof(long))
                return DrawArrayField(context, fieldName, label, (IList<long>) arrayValue);
            if (elementType == typeof(ulong))
                return DrawArrayField(context, fieldName, label, (IList<ulong>) arrayValue);

            if (elementType == typeof(float))
                return DrawArrayField(context, fieldName, label, (IList<float>) arrayValue);

            if (elementType == typeof(double))
                return DrawArrayField(context, fieldName, label, (IList<double>) arrayValue);

            if (elementType == typeof(string))
                return DrawArrayField(context, fieldName, label, (IList<string>) arrayValue);

            if (elementType == typeof(Vector2))
                return DrawArrayField(context, fieldName, label, (IList<Vector2>) arrayValue);

            if (elementType == typeof(Vector3))
                return DrawArrayField(context, fieldName, label, (IList<Vector3>) arrayValue);

            if (elementType == typeof(Vector4))
                return DrawArrayField(context, fieldName, label, (IList<Vector4>) arrayValue);

            if (elementType == typeof(Vector2Int))
                return DrawArrayField(context, fieldName, label, (IList<Vector2Int>) arrayValue);

            if (elementType == typeof(Vector3Int))
                return DrawArrayField(context, fieldName, label, (IList<Vector3Int>) arrayValue);

            if (elementType == typeof(Rect))
                return DrawArrayField(context, fieldName, label, (IList<Rect>) arrayValue);

            if (elementType == typeof(Bounds))
                return DrawArrayField(context, fieldName, label, (IList<Bounds>) arrayValue);

            if (elementType == typeof(RectInt))
                return DrawArrayField(context, fieldName, label, (IList<RectInt>) arrayValue);

            if (elementType == typeof(BoundsInt))
                return DrawArrayField(context, fieldName, label, (IList<BoundsInt>) arrayValue);

            if (elementType == typeof(Quaternion))
                return DrawArrayField(context, fieldName, label, (IList<Quaternion>) arrayValue);

            if (elementType == typeof(LayerMask))
                return DrawArrayField(context, fieldName, label, (IList<LayerMask>) arrayValue);

            if (elementType == typeof(Color))
                return DrawArrayField(context, fieldName, label, (IList<Color>) arrayValue);

            if (elementType == typeof(AnimationCurve))
                return DrawArrayField(context, fieldName, label, (IList<AnimationCurve>) arrayValue);

            if (elementType == typeof(Gradient))
                return DrawArrayField(context, fieldName, label, (IList<Gradient>) arrayValue);

            if (typeof(Object).IsAssignableFrom(elementType))
                return DrawArrayField(context, fieldName, label, (IList<Object>) arrayValue);

            return arrayValue;
        }

        public static IList<T> DrawArrayField<T>(Object context, string fieldName, string label, IList<T> array,
            Type drawerType = null)
        {
            var pathName = EditorSerializedFieldUtilities.GenerateFileName(context, fieldName);
            if (!EditorGUILayout.Foldout(GetFoldout(pathName), label, true))
            {
                SetFoldout(pathName, false);
                return array;
            }

            SetFoldout(pathName, true);

            EditorGUI.indentLevel++;
            var size = EditorGUILayout.DelayedIntField("Size", array.Count);
            ArrayUtilities.ResizeIList(size, ref array);

            for (var i = 0; i < size; i++)
                array[i] = (T) DrawLayoutField(context, fieldName, drawerType ?? typeof(T), $"Element {i}", array[i]);
            EditorGUI.indentLevel--;

            return array;
        }

        public static bool DrawArray<T>(string label, IList<T> array, bool enabled)
        {
            GUI.enabled = true;
            if (!EditorGUILayout.Foldout(GetFoldout(label), label, true))
            {
                GUI.enabled = enabled;
                SetFoldout(label, false);
                return false;
            }

            GUI.enabled = enabled;
            SetFoldout(label, true);

            var isDrawn = true;
            var size = EditorGUILayout.DelayedIntField(label, array.Count);
            EditorGUI.indentLevel++;
            for (var i = 0; i < size; i++)
                isDrawn = DrawLayoutField($"Element {i}", array[i], typeof(T), enabled) && isDrawn;
            EditorGUI.indentLevel--;

            return isDrawn;
        }

        private static bool LabelHasContent(GUIContent label) => label == null || label.text != string.Empty || label.image != null;

        private static Rect MultiFieldPrefixLabel(Rect position, int id, GUIContent label, int columns, GUIStyle style)
        {
            if (!LabelHasContent(label))
                return EditorGUI.IndentedRect(position);

            var indentWidth = GetCurrentIndentWidth();
            var newPosition = position;

            Rect labelPosition;

            if (EditorGUIUtility.wideMode)
            {
                var labelWidth = EditorGUIUtility.labelWidth;

                labelPosition = new Rect(position.x + indentWidth, position.y, labelWidth - indentWidth, FIELD_HEIGHT);

                newPosition.x += labelWidth;
                newPosition.width -= labelWidth + FIELD_SPACING * (columns - 1);

                EditorGUI.HandlePrefixLabel(position, labelPosition, label, id, style);
            }
            else
            {
                labelPosition = new Rect(position.x + indentWidth, position.y, position.width - indentWidth, FIELD_HEIGHT);

                newPosition.x += TAB_SIZE;
                newPosition.width -= TAB_SIZE + FIELD_SPACING * (columns - 1);
                newPosition.y += FIELD_HEIGHT;

                EditorGUI.HandlePrefixLabel(position, labelPosition, label, id, style);
            }

            return newPosition;
        }

        public static Rect MultiFieldPrefixLabel(Rect position, GUIContent label, int columns, GUIStyle style = null)
        {
            var controlId = GUIUtility.GetControlID("Foldout".GetHashCode(), FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, columns, style ?? EditorStyles.label);
            position.height = FIELD_HEIGHT;
            return position;
        }

        public static void MultiFloatField(Rect position, GUIContent[] subLabels, float[] values, float[] labelWidths,
            Orientation orientation = Orientation.Vertical)
        {
            var length = values.Length;
            var singleFieldWidth = position.width / length;
            var currentPosition = new Rect(position)
            {
                width = singleFieldWidth,
                height = FIELD_HEIGHT
            };

            var labelWidth = EditorGUIUtility.labelWidth;
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            for (var i = 0; i < values.Length; ++i)
            {
                EditorGUIUtility.labelWidth = labelWidths[i];
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        values[i] = EditorGUI.FloatField(currentPosition, subLabels[i], values[i]);
                        break;
                    case Orientation.Vertical:
                        EditorGUI.LabelField(currentPosition, subLabels[i]);
                        currentPosition.y += FIELD_HEIGHT + 1;
                        values[i] = EditorGUI.FloatField(currentPosition, values[i]);
                        currentPosition.y -= FIELD_HEIGHT + 1;
                        break;
                }

                currentPosition.x += singleFieldWidth + FIELD_SPACING;
            }

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indentLevel;
        }

        public static void MultiFloatField(Rect position, GUIContent label, GUIContent[] subLabels, float[] values, float subLblWidth,
            Orientation orientation = Orientation.Vertical, GUIStyle labelStyle = null) =>
            MultiFloatField(MultiFieldPrefixLabel(position, label, subLabels.Length, labelStyle), subLabels, values,
                Enumerable.Repeat(subLblWidth, values.Length).ToArray(), orientation);

        public static void MultiFloatField(Rect position, GUIContent label, GUIContent[] subLabels, float[] values, float[] subLblWidths,
            Orientation orientation = Orientation.Vertical, GUIStyle labelStyle = null) =>
            MultiFloatField(MultiFieldPrefixLabel(position, label, subLabels.Length, labelStyle), subLabels, values, subLblWidths,
                orientation);

        public static void MultiIntField(Rect position, GUIContent[] subLabels, int[] values, float[] labelWidths,
            Orientation orientation = Orientation.Vertical)
        {
            var length = values.Length;
            var singleFieldWidth = position.width / length;
            var currentPosition = new Rect(position)
            {
                width = singleFieldWidth
            };

            var labelWidth = EditorGUIUtility.labelWidth;
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            for (var index = 0; index < values.Length; ++index)
            {
                EditorGUIUtility.labelWidth = labelWidths[index];
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        values[index] = EditorGUI.IntField(currentPosition, subLabels[index], values[index]);
                        break;
                    case Orientation.Vertical:
                        EditorGUI.LabelField(currentPosition, subLabels[index]);
                        currentPosition.y += FIELD_HEIGHT + 1;
                        values[index] = EditorGUI.IntField(currentPosition, values[index]);
                        currentPosition.y -= FIELD_HEIGHT + 1;
                        break;
                }

                currentPosition.x += singleFieldWidth + FIELD_SPACING;
            }

            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indentLevel;
        }

        public static void MultiIntField(Rect position, GUIContent label, GUIContent[] subLabels, int[] values, float subLblWidth,
            Orientation orientation = Orientation.Vertical, GUIStyle labelStyle = null) =>
            MultiIntField(MultiFieldPrefixLabel(position, label, subLabels.Length, labelStyle), subLabels, values,
                Enumerable.Repeat(subLblWidth, values.Length).ToArray(), orientation);

        public static void MultiIntField(Rect position, GUIContent label, GUIContent[] subLabels, int[] values, float[] subLblWidths,
            Orientation orientation = Orientation.Vertical, GUIStyle labelStyle = null) =>
            MultiIntField(MultiFieldPrefixLabel(position, label, subLabels.Length, labelStyle), subLabels, values, subLblWidths,
                orientation);

        #endregion /Fields

        #region Obsolete

        [Obsolete("Suffers from Gimbal Lock.")]
        public static Quaternion DrawRotationEuler(Rect position, string label, Quaternion rotation) =>
            Quaternion.Euler(MathUtilities.Round(EditorGUI.Vector3Field(position, label, rotation.eulerAngles), 2));

        [Obsolete("Suffers from Gimbal Lock.")]
        public static Quaternion DrawRotationEuler(Rect position, GUIContent label, Quaternion rotation) =>
            Quaternion.Euler(MathUtilities.Round(EditorGUI.Vector3Field(position, label, rotation.eulerAngles), 2));

        [Obsolete("Suffers from Gimbal Lock.")]
        public static void DrawRotationEuler(Rect position, string label, SerializedProperty property) =>
            property.quaternionValue = DrawRotationEuler(position, label, property.quaternionValue);

        [Obsolete("Suffers from Gimbal Lock.")]
        public static void DrawRotationEuler(Rect position, GUIContent label, SerializedProperty property) =>
            property.quaternionValue = DrawRotationEuler(position, label, property.quaternionValue);

        [Obsolete("Suffers from Gimbal Lock.")]
        public static void DrawRotationEuler(Rect position, SerializedProperty property) =>
            property.quaternionValue = DrawRotationEuler(position, property.displayName, property.quaternionValue);

        [Obsolete("Suffers from Gimbal Lock.")]
        public static Quaternion DrawRotationEuler(string label, Quaternion rotation) =>
            Quaternion.Euler(MathUtilities.Round(EditorGUILayout.Vector3Field(label, rotation.eulerAngles), 2));

        [Obsolete("Suffers from Gimbal Lock.")]
        public static Quaternion DrawRotationEuler(GUIContent label, Quaternion rotation) =>
            Quaternion.Euler(MathUtilities.Round(EditorGUILayout.Vector3Field(label, rotation.eulerAngles), 2));

        [Obsolete("Suffers from Gimbal Lock.")]
        public static void DrawRotationEuler(string label, SerializedProperty property) =>
            property.quaternionValue = DrawRotationEuler(label, property.quaternionValue);

        [Obsolete("Suffers from Gimbal Lock.")]
        public static void DrawRotationEuler(GUIContent label, SerializedProperty property) =>
            property.quaternionValue = DrawRotationEuler(label, property.quaternionValue);

        [Obsolete("Suffers from Gimbal Lock.")]
        public static void DrawRotationEuler(SerializedProperty property) =>
            property.quaternionValue = DrawRotationEuler(property.displayName, property.quaternionValue);

        #endregion /Obsolete
    }
}
#endif