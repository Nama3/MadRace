#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class EditorSerializedField
    {
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();

        private readonly string m_guid;
        private readonly long m_localId;
        private string m_fileName;
        private string m_path;

        public EditorSerializedField(string guid, long localID, string fieldName)
        {
            m_guid = guid;
            m_localId = localID;
            m_path = SetFileName(fieldName);
        }

        public void Serialize(object value, Type type)
        {
            var wrapper = EditorSerializedFieldUtilities.GetWrapperForType(type);
            if (wrapper != null)
            {
//                if (type.IsEnum)
//                    value = Enum.ToObject(type, value);
                Serialize(wrapper, value);
                return;
            }

            if (type.GetCustomAttribute(typeof(SerializableAttribute), true) == null) return;

            wrapper = EditorSerializedFieldUtilities.GetWrapperForType(typeof(bool));
            wrapper.SetValue(MightyGUIUtilities.GetFoldout(m_fileName));

            WriteFile(m_path, wrapper);

            foreach (var field in type.GetSerializableFields())
                EditorFieldsDatabase.GetEditorField(m_fileName).Serialize(field.GetValue(value), field.FieldType);
        }

        public bool DeserializeOverwrite(Type type, out object value)
        {
            var wrapper = EditorSerializedFieldUtilities.GetWrapperForType(type);

            if (wrapper != null) return Deserialize(wrapper, out value);

            value = null;

            if (type.GetCustomAttribute(typeof(SerializableAttribute), true) == null) return false;

            if (!Exists()) return false;

            wrapper = EditorSerializedFieldUtilities.GetWrapperForType(typeof(bool));
            ReadFile(m_path, wrapper);
            wrapper.GetValue(out var foldout);
            MightyGUIUtilities.SetFoldout(m_fileName, (bool) foldout);

            value = Activator.CreateInstance(type);
            foreach (var field in type.GetSerializableFields())
            {
                EditorFieldsDatabase.GetEditorField(m_fileName).DeserializeOverwrite(field.FieldType, out var fieldValue);
                if (field.FieldType.IsEnum)
                    fieldValue = Enum.ToObject(field.FieldType, fieldValue);
                field.SetValue(value, fieldValue);
            }

            return true;
        }

        private void Serialize(BaseEditorFieldWrapper wrapper, object value)
        {
            wrapper.SetValue(value);
            if (!(wrapper is BaseArrayFieldWrapper arrayWrapper))
            {
                WriteFile(m_path, wrapper);
                return;
            }

            arrayWrapper.foldout = MightyGUIUtilities.GetFoldout(m_fileName);
            WriteFile(m_path, arrayWrapper);
        }

        private bool Deserialize(BaseEditorFieldWrapper wrapper, out object value)
        {
            value = null;
            if (!Exists()) return false;

            wrapper.ResetValue();
            ReadFile(m_path, wrapper);

            if (wrapper is BaseArrayFieldWrapper arrayWrapper)
                MightyGUIUtilities.SetFoldout(m_fileName, arrayWrapper.foldout);

            wrapper.GetValue(out value);
            return value != null;
        }

        public void Rename(string newFieldName)
        {
            var newPath = SetFileName(newFieldName);
            Delete(newPath);

            File.Move($"{m_path}.json", $"{newPath}.json");
            m_path = newPath;
        }
        
        public void Delete() => Delete(m_path);

        private static void Delete(string path)
        {
            path = $"{path}.json";
            if (File.Exists(path)) 
                File.Delete(path);
        }

        public bool Exists() => File.Exists($"{m_path}.json");

        private string SetFileName(string fieldName)
        {
            m_fileName = $"{m_guid}.{m_localId}.{fieldName}";
            return EditorSerializedFieldUtilities.CreateEditorFieldPath(m_fileName);
        }

        private static void WriteFile(string filePath, BaseEditorFieldWrapper wrapper)
        {
            var json = EditorJsonUtility.ToJson(wrapper);
            var bytes = Encoding.GetBytes(json);
            try
            {
                using (var stream = File.Open($"{filePath}.json", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var length = bytes.Length;
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Write(bytes, 0, length);
                    stream.SetLength(length);
                }
            }
            catch
            {
                // ignored
            }
        }

        private static void ReadFile(string filePath, BaseEditorFieldWrapper wrapper)
        {
            try
            {
                byte[] bytes;
                using (var stream = File.OpenRead($"{filePath}.json"))
                {
                    var length = (int) stream.Length;
                    bytes = new byte[length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(bytes, 0, length);
                }

                EditorJsonUtility.FromJsonOverwrite(new UTF8Encoding().GetString(bytes), wrapper);
            }
            catch
            {
                // ignored
            }
        }
    }
}
#endif