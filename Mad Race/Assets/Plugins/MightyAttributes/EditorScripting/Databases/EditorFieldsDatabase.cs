#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [InitializeOnLoad]
    public static class EditorFieldsDatabase
    {
        private static readonly Dictionary<string, EditorSerializedField> EditorFieldByFileName;

        static EditorFieldsDatabase()
        {
            var mainAssembly = TypeUtilities.GetMainAssembly();
            if (mainAssembly == null) return;

            var guidAndFieldNames = new List<(string, string)>();

            foreach (var type in mainAssembly.GetTypes())
            {
                var typePath = FileUtilities.GetRelativeScriptPathByType(type);
                var typeGUID = AssetDatabase.AssetPathToGUID(typePath);
                PopulateFieldNames(typeGUID, guidAndFieldNames,
                    ReflectionUtilities.GetAllFields(type, f => f.GetCustomAttribute(typeof(EditorSerializeAttribute)) != null));
            }

            if (guidAndFieldNames.Count == 0) return;

            EditorFieldByFileName = new Dictionary<string, EditorSerializedField>();

            if (!Directory.Exists(EditorSerializedFieldUtilities.DirectoryPath))
                Directory.CreateDirectory(EditorSerializedFieldUtilities.DirectoryPath);

            foreach (var filePath in Directory.GetFiles(EditorSerializedFieldUtilities.DirectoryPath))
            {
                if (Path.GetExtension(filePath) == ".meta") continue;
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                if (!Regex.IsMatch(fileName, EditorSerializedFieldUtilities.FILE_NAME_REGEX))
                {
                    File.Delete(filePath);
                    continue;
                }
                
                var (guid, localID, fieldName) = EditorSerializedFieldUtilities.GetGUIDAndLocalIDAndFieldName(fileName);
                
                if (EditorFieldByFileName.ContainsKey(fileName)) continue;

                EditorFieldByFileName[fileName] = new EditorSerializedField(guid, localID, fieldName);
            }

            foreach (var key in EditorFieldByFileName.Keys.ToArray())
            {
                var (guid, _, fieldName) = EditorSerializedFieldUtilities.GetGUIDAndLocalIDAndFieldName(key);
                if (guidAndFieldNames.Contains((guid, fieldName))) continue;
                
                EditorFieldByFileName[key].Delete();
                EditorFieldByFileName.Remove(key);
            }
        }

        private static void PopulateFieldNames(string typeGUID, List<(string, string)> guidAndFieldNames, IEnumerable<FieldInfo> fields,
            string namePrefix = "")
        {
            foreach (var field in fields)
            {
                guidAndFieldNames.Add((typeGUID, $"{namePrefix}{field.Name}"));
                if (field.FieldType.IsSerializableClassOrStruct())
                    PopulateFieldNames(typeGUID, guidAndFieldNames, field.FieldType.GetSerializableFields(), $"{namePrefix}{field.Name}.");

                if (!(field.GetCustomAttribute(typeof(EditorSerializeAttribute)) is EditorSerializeAttribute attribute)) continue;

                if (attribute.PreviousName == null || attribute.PreviousName == field.Name) continue;

                guidAndFieldNames.Add((typeGUID, $"{namePrefix}{attribute.PreviousName}"));
                if (field.FieldType.IsSerializableClassOrStruct())
                    PopulateFieldNames(typeGUID, guidAndFieldNames, field.FieldType.GetSerializableFields(),
                        $"{namePrefix}{attribute.PreviousName}.");
            }
        }

        public static void RenameField(Object context, string previousFieldName, string newFieldName)
        {
            var previousFileName = EditorSerializedFieldUtilities.GenerateFileName(context, previousFieldName);
            if (!EditorFieldByFileName.ContainsKey(previousFileName)) return;

            foreach (var key in EditorFieldByFileName.Keys.Where(k => k.Contains(previousFileName)).ToArray())
            {
                var newFileName = EditorSerializedFieldUtilities.GenerateFileName(context, newFieldName);

                var editorField = EditorFieldByFileName[key];

                editorField.Rename(newFieldName);
                EditorFieldByFileName.Remove(key);
                EditorFieldByFileName[newFileName] = editorField;
            }
        }

        public static EditorSerializedField GetEditorField(Object context,  string fieldName) =>
            GetEditorField(EditorSerializedFieldUtilities.GenerateFileName(context, fieldName));

        public static EditorSerializedField GetEditorField(string fileName)
        {
            if (fileName == null) return null;
            if (EditorFieldByFileName.ContainsKey(fileName)) return EditorFieldByFileName[fileName];
            
            var (guid, localID, fieldName) = EditorSerializedFieldUtilities.GetGUIDAndLocalIDAndFieldName(fileName);
            EditorFieldByFileName[fileName] = new EditorSerializedField(guid, localID, fieldName);
            return EditorFieldByFileName[fileName];
        }
    }
}
#endif