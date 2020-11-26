#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class FileUtilities
    {
        private const string META_ICON_REGEX = @"\sicon: {fileID: \d+, guid: (.+), type: \d+}";

        public static string GetIconGUIDFromScriptType(Type type) => GetIconGUIDFromMetaFile(GetAbsoluteMetaPathByType(type));

        public static string GetIconGUIDFromMetaFile(string metaFilePath)
        {
            foreach (var line in File.ReadAllLines(metaFilePath))
            {
                var match = Regex.Match(line, META_ICON_REGEX);
                if (match.Success && match.Groups.Count == 2)
                    return match.Groups[1].Value;
            }

            return null;
        }

        public static string GetRelativeMetaPathByType(Type type) => $"{GetRelativeScriptPathByType(type)}.meta";

        public static string GetAbsoluteMetaPathByType(Type type) => $"{GetAbsoluteScriptPathByType(type)}.meta";

        public static string GetRelativeScriptPathByType(Type type, bool pathWithName = true)
        {
            var typeName = TypeUtilities.GetUnityTypeName(type);
            var assets = AssetDatabase.FindAssets($"{typeName} t:script");
            if (assets == null || assets.Length == 0) 
                return null;

            string relativePath = null;
            foreach (var asset in assets)
            {
                var currentPath = AssetDatabase.GUIDToAssetPath(asset);
                if (Path.GetFileNameWithoutExtension(currentPath) != typeName) continue;
                relativePath = currentPath;
            }

            if (relativePath == null)
                return null;

            return pathWithName ? relativePath.Replace("/", "\\") : relativePath.Replace($"{typeName}.cs", string.Empty).Replace("/", "\\");
        }

        public static string GetAbsoluteScriptPathByType(Type type, bool pathWithName = true)
        {
            var relativePath = GetRelativeScriptPathByType(type, pathWithName);
            if (relativePath == null) return null;

            var absolutePath = $"{Application.dataPath.Replace("Assets", string.Empty)}{relativePath}";

            return pathWithName
                ? absolutePath.Replace("/", "\\")
                : absolutePath.Replace($"{type.Name}.cs", string.Empty).Replace("/", "\\");
        }

        public static string ParentDirectoryPath(string path) => Directory.GetParent(path)?.Parent?.FullName;
        public static string GetDirectoryPath(string path) => Directory.GetParent(path)?.FullName;
        
        public static void OpenExplorerAndSelectFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }        
        
        public static void OpenExplorerAtFolder(string folderPath)
        {
            folderPath = Path.GetFullPath(folderPath);
            Process.Start("explorer.exe", $"\"{folderPath}\"");
        }

        public static void OpenAtPath(string path)
        {
            path = Path.GetFullPath(path);
            Process.Start(path);
        }
    }
}
#endif