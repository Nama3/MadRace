#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public class EditorSerializeDrawer : BaseNonSerializedFieldDrawer<EditorSerializeAttribute>
    {
        public delegate object DrawCallback(EditorSerializeAttribute attribute, Object context, object target, object value);

        public void DrawField(string fieldName, Object context) => DrawField(fieldName, context, context);

        public void DrawField(string fieldName, Object context, object target)
        {
            var field = target.GetField(fieldName);
            var attribute = field.GetCustomAttribute<EditorSerializeAttribute>();

            if (attribute.PreviousName != null)
                EditorFieldsDatabase.RenameField(context, attribute.PreviousName, field.Name);

            var editorField = EditorFieldsDatabase.GetEditorField(context, field.Name);
            if (editorField == null) return;
            
            var value = field.GetValue(target);

            Deserialize(editorField, target, field, ref value);

            EditorGUI.BeginChangeCheck();

            value = MightyGUIUtilities.DrawLayoutField(field, context, target, value,
                !attribute.Options.Contains(EditorFieldOption.DontFold), attribute.Options.Contains(EditorFieldOption.Asset));

            if (EditorGUI.EndChangeCheck()) Serialize(attribute, editorField, value, field.FieldType);
        }

        public void DrawField(string fieldName, Object context, DrawCallback drawCallback) =>
            DrawField(fieldName, context, context, drawCallback);

        public void DrawField(string fieldName, Object context, object target, DrawCallback drawCallback)
        {
            var field = target.GetField(fieldName);
            var attribute = field.GetCustomAttribute<EditorSerializeAttribute>();

            if (attribute.PreviousName != null)
                EditorFieldsDatabase.RenameField(context, attribute.PreviousName, field.Name);

            var editorField = EditorFieldsDatabase.GetEditorField(context, field.Name);
            if (editorField == null) return;
            
            var value = field.GetValue(target);

            Deserialize(editorField, target, field, ref value);

            EditorGUI.BeginChangeCheck();

            value = drawCallback(attribute, context, target, value);

            if (EditorGUI.EndChangeCheck()) Serialize(attribute, editorField, value, field.FieldType);
        }

        protected override void DrawField(MightyNonSerializedField nonSerializedField, EditorSerializeAttribute attribute)
        {
            var context = nonSerializedField.Context.Object;
            var field = nonSerializedField.MemberInfo;
            var target = nonSerializedField.Context.Target;

            if (attribute.PreviousName != null)
                EditorFieldsDatabase.RenameField(context, attribute.PreviousName, field.Name);

            if (attribute.Options == EditorFieldOption.Hide) return;

            var editorField = EditorFieldsDatabase.GetEditorField(context, field.Name);
            if (editorField == null) return;
            
            var value = field.GetValue(target);

            if (attribute.Options.Contains(EditorFieldOption.Deserialize)) Deserialize(editorField, target, field, ref value);

            if (attribute.Options.Contains(EditorFieldOption.Hide) &&
                field.GetCustomAttribute(typeof(HideAttribute), true) is HideAttribute)
            {
                if (attribute.Options.Contains(EditorFieldOption.Serialize)) Serialize(attribute, editorField, value, field.FieldType);
                return;
            }

            if (attribute.Options.Contains(EditorFieldOption.Hide)) return;

            EditorGUI.BeginChangeCheck();

            if (field.GetCustomAttribute(typeof(CustomDrawerAttribute), true) is CustomDrawerAttribute drawerAttribute &&
                MightyDrawersDatabase.GetDrawerForAttribute<CustomDrawerDrawer>(typeof(CustomDrawerAttribute)) is var drawer)
                value = drawer.DrawField(field, context, value, drawerAttribute);
            else
                value = MightyGUIUtilities.DrawLayoutField(field, context, target, value,
                    !attribute.Options.Contains(EditorFieldOption.DontFold), attribute.Options.Contains(EditorFieldOption.Asset));

            if (EditorGUI.EndChangeCheck() && attribute.Options.Contains(EditorFieldOption.Serialize))
                Serialize(attribute, editorField, value, field.FieldType);
        }

        public void Serialize(EditorSerializeAttribute attribute, EditorSerializedField editorField, object value, Type fieldType)
        {
            if (attribute.ExecuteInPlayMode || !EditorApplication.isPlaying)
                editorField.Serialize(value, fieldType);
        }

        public void Deserialize(EditorSerializedField editorField, object target, FieldInfo field,
            ref object value)
        {
            var fieldType = field.FieldType;
            if (!editorField.DeserializeOverwrite(fieldType, out var jsonValue)) return;

            if (fieldType.IsEnum)
                jsonValue = Enum.ToObject(fieldType, jsonValue);
            if (typeof(Object).IsAssignableFrom(fieldType) && !(jsonValue as Object) || value == jsonValue) return;

            value = jsonValue;
            field.SetValue(target, value);
        }

        protected override void Enable(BaseMightyMember mightyMember, EditorSerializeAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif