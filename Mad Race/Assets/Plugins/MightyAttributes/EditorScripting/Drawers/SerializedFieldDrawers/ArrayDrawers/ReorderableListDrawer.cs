#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ReorderableListDrawer : BaseArrayDrawer<ReorderableListAttribute>
    {
        private const int WIDTH_OVERFLOW = 19;
        private const int SPACE = 30;
        private const int SIZE_LABEL_WIDTH = 35;
        private const int SIZE_FIELD_WIDTH = 100;

        private readonly MightyCache<int> m_indentCache = new MightyCache<int>();
        private readonly MightyCache<ReorderableList> m_reorderableCache = new MightyCache<ReorderableList>();

        protected override void DrawArrayImpl(MightySerializedField serializedField, ReorderableListAttribute attribute, ArrayOption options,
            BaseArrayDecoratorAttribute[] decoratorAttributes, IArrayElementDrawer drawer, BasePropertyDrawerAttribute drawerAttribute)
        {
            var property = serializedField.Property;

            EditorGUILayout.BeginVertical();

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawArray(serializedField, decoratorAttribute);

            if (!options.Contains(ArrayOption.HideLabel) && !options.Contains(ArrayOption.LabelInHeader))
            {
                foreach (var decoratorAttribute in decoratorAttributes)
                    ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawHeader(serializedField, decoratorAttribute);

                if (options.Contains(ArrayOption.DontFold))
                {
                    EditorGUILayout.LabelField(property.displayName,
                        options.Contains(ArrayOption.BoldLabel) ? EditorStyles.boldLabel : EditorStyles.label);
                    property.isExpanded = true;
                }
                else if (!MightyGUIUtilities.DrawFoldout(property,
                    options.Contains(ArrayOption.BoldLabel) ? MightyStyles.BoldFoldout : null))
                {
                    EditorGUILayout.EndVertical();

                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawHeader(serializedField, decoratorAttribute);

                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawArray(serializedField, decoratorAttribute);

                    return;
                }

                foreach (var decoratorAttribute in decoratorAttributes)
                    ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawHeader(serializedField, decoratorAttribute);
            }
            else 
                property.isExpanded = true;

            if (!options.Contains(ArrayOption.DontIndent))
            {
                m_indentCache[serializedField] = EditorGUI.indentLevel;
                MightyGUIUtilities.BeginLayoutIndent();
                EditorGUI.indentLevel = 0;
                EditorGUILayout.BeginVertical();
            }

            if (!m_reorderableCache.Contains(serializedField))
            {
                ReorderableList reorderableList = new ReorderableList(property.serializedObject, property,
                    attribute.Draggable, options.Contains(ArrayOption.LabelInHeader) || !options.Contains(ArrayOption.HideSizeField),
                    attribute.DrawButtons, attribute.DrawButtons)
                {
                    drawHeaderCallback = position =>
                    {
                        var labelWidth = EditorGUIUtility.labelWidth;

                        if (options.Contains(ArrayOption.LabelInHeader))
                        {
                            var labelSpace = Screen.width - WIDTH_OVERFLOW - SIZE_FIELD_WIDTH - SIZE_LABEL_WIDTH;
                            position.width = labelSpace - SPACE;
                            if (options.Contains(ArrayOption.BoldLabel))
                                EditorGUI.LabelField(position, property.displayName, EditorStyles.boldLabel);
                            else
                                EditorGUI.LabelField(position, property.displayName);

                            position.x = labelSpace;
                            position.width = SIZE_FIELD_WIDTH + SIZE_LABEL_WIDTH;
                            EditorGUIUtility.labelWidth = SIZE_LABEL_WIDTH;
                        }

                        if (!options.Contains(ArrayOption.HideSizeField))
                        {
                            var enabled = GUI.enabled;
                            GUI.enabled = !options.Contains(ArrayOption.DisableSizeField);

                            MightyGUIUtilities.DrawArraySizeField(position, property);
                            GUI.enabled = enabled;
                        }

                        EditorGUIUtility.labelWidth = labelWidth;
                    },

                    drawElementCallback = (position, index, isActive, isFocused) =>
                    {
                        position.y += 2;

                        foreach (var decoratorAttribute in decoratorAttributes)
                            position = ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawElement(position, serializedField, index, decoratorAttribute);

                        if (drawer != null)
                        {
                            var height = drawer.GetElementHeight(serializedField, index, drawerAttribute);
                            position.height = height;
                            drawer.DrawElement(position, serializedField, index, drawerAttribute);
                            position = MightyGUIUtilities.JumpHeight(position, height);
                        }
                        else if (options.Contains(ArrayOption.HideElementLabel))
                            position = MightyGUIUtilities.DrawPropertyField(position, property.GetArrayElementAtIndex(index),
                                GUIContent.none);
                        else
                            position = MightyGUIUtilities.DrawPropertyField(position, property.GetArrayElementAtIndex(index));

                        foreach (var decoratorAttribute in decoratorAttributes)
                            position = ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawElement(position, serializedField, index, decoratorAttribute);
                    },

                    elementHeightCallback = index => GetElementHeight(serializedField, attribute, drawer, drawerAttribute, index),
                    headerHeight = MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING
                };

                m_reorderableCache[serializedField] = reorderableList;
            }

            m_reorderableCache[serializedField].DoLayoutList();

            if (!options.Contains(ArrayOption.DontIndent))
            {
                EditorGUI.indentLevel = m_indentCache[serializedField];
                MightyGUIUtilities.EndLayoutIndent();
                EditorGUILayout.EndVertical();
            }

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawArray(serializedField, decoratorAttribute);

            EditorGUILayout.EndVertical();
        }

        private float GetElementHeight(MightySerializedField serializedField, ReorderableListAttribute attribute, 
            IArrayElementDrawer drawer, BasePropertyDrawerAttribute drawerAttribute, int index)
        {
            var decoratorAttributes = GetDecorationsForMember(serializedField, attribute);

            var height = drawer?.GetElementHeight(serializedField, index, drawerAttribute) ??
                         MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

            foreach (var decoratorAttribute in decoratorAttributes)
                height += ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).GetElementHeight(serializedField, index, decoratorAttribute);

            return height + 2;
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_indentCache.ClearCache();
            m_reorderableCache.ClearCache();
        }
    }
}
#endif