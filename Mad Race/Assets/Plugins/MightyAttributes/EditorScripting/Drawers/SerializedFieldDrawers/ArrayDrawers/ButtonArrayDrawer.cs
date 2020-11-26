#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ButtonArrayDrawer : BaseArrayDrawer<ButtonArrayAttribute>
    {
        protected override void DrawArrayImpl(MightySerializedField serializedField, ButtonArrayAttribute attribute, ArrayOption options,
            BaseArrayDecoratorAttribute[] decoratorAttributes, IArrayElementDrawer drawer, BasePropertyDrawerAttribute drawerAttribute)
        {
            var property = serializedField.Property;

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawArray(serializedField, decoratorAttribute);

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawHeader(serializedField, decoratorAttribute);

            if (!options.Contains(ArrayOption.HideLabel))
            {
                if (options.Contains(ArrayOption.DontFold))
                {
                    EditorGUILayout.LabelField(property.displayName,
                        options.Contains(ArrayOption.BoldLabel) ? EditorStyles.boldLabel : EditorStyles.label);
                    property.isExpanded = true;
                }
                else if (!MightyGUIUtilities.DrawFoldout(property,
                    options.Contains(ArrayOption.BoldLabel) ? MightyStyles.BoldFoldout : null))
                {
                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawHeader(serializedField, decoratorAttribute);

                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawArray(serializedField, decoratorAttribute);

                    return;
                }
            }
            else
                property.isExpanded = true;

            if (!options.Contains(ArrayOption.DontIndent))
                EditorGUI.indentLevel++;

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawHeader(serializedField, decoratorAttribute);


            GUILayout.BeginVertical(MightyStyleUtilities.GetButtonArray(EditorGUI.indentLevel - 1), GUILayout.MinHeight(35));
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (property.arraySize == 0)
            {
                GUILayout.FlexibleSpace();
                if (MightyGUIUtilities.DrawAddButton())
                {
                    property.InsertArrayElementAtIndex(0);
                    property.serializedObject.ApplyModifiedProperties();
                }

                GUILayout.FlexibleSpace();
            }

            MightyGUIUtilities.DrawArrayBody(property, index =>
            {
                var element = property.GetArrayElementAtIndex(index);

                foreach (var decoratorAttribute in decoratorAttributes)
                    ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawElement(serializedField, index,
                        decoratorAttribute);

                GUILayout.BeginHorizontal(GUILayout.MinHeight(33));

                GUILayout.BeginVertical(GUILayout.Width(1));
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();

                if (MightyGUIUtilities.DrawRemoveButton())
                {
                    property.DeleteArrayElementAtIndex(index);
                    property.serializedObject.ApplyModifiedProperties();

                    GUILayout.EndHorizontal();

                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawElement(serializedField, index,
                            decoratorAttribute);
                    return;
                }

                if (MightyGUIUtilities.DrawAddButton())
                {
                    property.InsertArrayElementAtIndex(index);
                    property.serializedObject.ApplyModifiedProperties();
                }

                if (serializedField.IsFoldable())
                    GUILayout.Space(10);

                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();

                if (drawer != null)
                    drawer.DrawElement(serializedField, index, drawerAttribute);
                else if (options.Contains(ArrayOption.HideElementLabel))
                    EditorGUILayout.PropertyField(element, GUIContent.none);
                else
                    EditorGUILayout.PropertyField(element);

                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

                foreach (var decoratorAttribute in decoratorAttributes)
                    ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawElement(serializedField, index, decoratorAttribute);
            });

            EditorGUI.indentLevel = indent;
            GUILayout.EndVertical();

            if (!options.Contains(ArrayOption.DontIndent))
                EditorGUI.indentLevel--;

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawArray(serializedField, decoratorAttribute);
        }
    }
}
#endif