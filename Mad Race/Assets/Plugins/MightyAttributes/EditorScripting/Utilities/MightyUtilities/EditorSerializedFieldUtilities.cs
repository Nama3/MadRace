#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public static class EditorSerializedFieldUtilities
    {
        public static readonly string DirectoryPath = Path.Combine(Application.dataPath, "EditorSerializedFields").Replace("\\", "/");

        private static PropertyInfo m_inspectorModeInfo;

        public const string FILE_NAME_REGEX = @"^([^.]+)\.([0-9]+)\.([^.]+)$";

        public static string GenerateFileName(Object context, string fieldName) =>
            TryGetGUIDAndLocalID(context, out var guid, out var localID) ? GenerateFileName(guid, localID, fieldName) : null;

        public static string GenerateFileName(string guid, long localID, string fieldName) => $"{guid}.{localID}.{fieldName}";

        public static bool TryGetGUIDAndLocalID(Object context, out string guid, out long localID)
        {
            if (m_inspectorModeInfo == null)
                m_inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            if (m_inspectorModeInfo == null)
            {
                guid = null;
                localID = 0;
                return false;
            }

            var serializedObject = new SerializedObject(context);
            m_inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            var localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile"); //note the misspelling!

            
            var typePath = FileUtilities.GetRelativeScriptPathByType(context.GetType());
            guid = AssetDatabase.AssetPathToGUID(typePath);
            localID = localIdProp.longValue;

            return true;
        }

        public static (string, long, string) GetGUIDAndLocalIDAndFieldName(string fileName)
        {
            var match = Regex.Match(fileName, FILE_NAME_REGEX);

            var groups = match.Groups;
            if (match.Success && groups.Count == 4 && long.TryParse(groups[2].Value, out var localID))
                return (groups[1].Value, localID, groups[3].Value);

            return default;
        }

        public static string CreateEditorFieldPath(string fileName) => Path.Combine(DirectoryPath, $"{fileName}").Replace("\\", "/");

        public static bool TryGetEditorFieldValue(string fieldName, Object context, out object value, string previousName = null) => 
            TryGetEditorFieldValue(fieldName, context, context, out value, previousName);

        public static bool TryGetEditorFieldValue(string fieldName, Object context, object target, out object value,
            string previousName = null)
        {
            var field = target.GetField(fieldName);
            if (field != null)
            {
                GetEditorFieldValue(field, context, out value, previousName);
                return true;
            }
            value = null;
            return false;
        }

        public static void GetEditorFieldValue(FieldInfo field, Object context, out object value, string previousName = null)
        {
            if (previousName != null)
                EditorFieldsDatabase.RenameField(context, previousName, field.Name);

            var editorField = EditorFieldsDatabase.GetEditorField(context, field.Name);
            editorField.DeserializeOverwrite(field.FieldType, out value);
        }        
        
        public static void SetEditorFieldValue(string fieldName, Object context, object value, string previousName = null) => 
            SetEditorFieldValue(fieldName, context, context, value, previousName);

        public static void SetEditorFieldValue(string fieldName, Object context, object target, object value, string previousName = null)
        {
            var field = target.GetField(fieldName);
            if (field != null) SetEditorFieldValue(field, context, value, previousName);
        }

        public static void SetEditorFieldValue(FieldInfo field, Object context, object value, string previousName = null)
        {
            if (previousName != null)
                EditorFieldsDatabase.RenameField(context, previousName, field.Name);

            var editorField = EditorFieldsDatabase.GetEditorField(context, field.Name);
            editorField.Serialize(value, field.FieldType);
        }

        public static void ApplyEditorFieldChanges(string fieldName, Object context, string previousName = null) =>
            ApplyEditorFieldChanges(fieldName, context, context, previousName);

        public static void ApplyEditorFieldChanges(string fieldName, Object context, object target, string previousName = null)
        {
            var field = target.GetField(fieldName);
            if (field != null) SetEditorFieldValue(field, context, field.GetValue(target), previousName);
        }

        public static BaseEditorFieldWrapper GetWrapperForType(Type type) =>
            type.IsArray
                ? EditorFieldWrappersDatabase.GetWrapperForType(type.GetElementType(), true)
                : EditorFieldWrappersDatabase.GetWrapperForType(type, false);
    }
}
#endif