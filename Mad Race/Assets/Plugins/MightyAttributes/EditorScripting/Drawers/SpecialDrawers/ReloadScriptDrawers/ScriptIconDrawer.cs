#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [Serializable]
    public class ScriptIcon
    {
        public Texture2D icon;
        public int priority;

        public bool ignored;

        [SerializeField] private string _assemblyName;
        [SerializeField] private string _typeName;
        [SerializeField] private string _metaIconGUID;

        private bool m_valid, m_used;

        public bool IsValid => m_valid;
        public void SetValid() => m_valid = true;

        public bool IsUsed => m_used;
        public void SetUsed() => m_used = true;

        private Type m_type;

        public void SetAssembly(Assembly assembly) => _assemblyName = assembly.GetName().Name;

        public string GetAssemblyName() => _assemblyName;

        public void SetType(Type type)
        {
            m_type = type;

            _typeName = type.FullName;
            _metaIconGUID = FileUtilities.GetIconGUIDFromScriptType(type);
        }

        public string GetTypeName() => _typeName;

        public string GetScriptPath() => FileUtilities.GetRelativeScriptPathByType(m_type);

        public bool IsIconValid => icon;

        public bool CompareIcons() => _metaIconGUID != null &&
                                      AssetDatabase.TryGetGUIDAndLocalFileIdentifier(icon, out var iconGUID, out long _) &&
                                      _metaIconGUID == iconGUID;
    }

    [Serializable]
    public class ScriptIconsDatabase
    {
        public ScriptIcon[] scriptIcons;
    }

    public class ScriptIconDrawer : BaseReloadScriptsDrawer<ScriptIconAttribute>
    {
        private readonly MethodInfo SetIconForObject =
            typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);

        private readonly MethodInfo CopyMonoScriptIconToImporters =
            typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static | BindingFlags.NonPublic);

        private bool m_alreadyEnabled;

        private string m_databaseDirectoryPath;
        private string m_databaseFilePath;

        private ScriptIconsDatabase m_database;
        private Dictionary<Type, ScriptIcon> m_scriptIconByType;

        protected override void Enable(BaseMightyMember mightyMember, ScriptIconAttribute mightyAttribute)
        {
            if (!m_alreadyEnabled)
            {
                m_databaseDirectoryPath = Path.Combine(FileUtilities.GetAbsoluteScriptPathByType(typeof(MightyDrawersDatabase), false),
                    "ScriptIcons");
                m_databaseFilePath = Path.Combine(m_databaseDirectoryPath, "ScriptIconsDatabase.json");
            }

            if (!Directory.Exists(m_databaseDirectoryPath))
            {
                m_alreadyEnabled = false;
                Directory.CreateDirectory(m_databaseDirectoryPath);
            }

            if (!File.Exists(m_databaseFilePath))
            {
                m_alreadyEnabled = false;
                File.Create(m_databaseFilePath);
            }

            if (!m_alreadyEnabled)
            {
                m_database = ReadDatabase();
                m_scriptIconByType = ExtractDatabase(m_database);
            }
            
            m_alreadyEnabled = true;
        }

        protected override void ClearCache()
        {
        }

        private ScriptIconsDatabase ReadDatabase()
        {
            var database = new ScriptIconsDatabase();
            EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(m_databaseFilePath), database);

            return database;
        }

        private Dictionary<Type, ScriptIcon> ExtractDatabase(ScriptIconsDatabase database)
        {
            var scriptIconByType = new Dictionary<Type, ScriptIcon>();
            if (database.scriptIcons == null) return scriptIconByType;

            foreach (var icon in database.scriptIcons)
            {
                if (!TypeUtilities.GetTypeInAssembly(icon.GetTypeName(), icon.GetAssemblyName(), out var type)) continue;

                icon.SetType(type);
                scriptIconByType[type] = icon;
            }

            return scriptIconByType;
        }

        private void WriteDatabase() => File.WriteAllText(m_databaseFilePath, EditorJsonUtility.ToJson(m_database, true));

        protected override void BeginReloadScripts(MightyType mightyType, ScriptIconAttribute baseAttribute) =>
            PopulateScriptIcon(mightyType, baseAttribute);

        protected override void EndReloadScripts(MightyType mightyType, ScriptIconAttribute baseAttribute)
        {
            if (!RemoveInvalidScriptIcons()) MightyReloadScripts.BeginReloadScripts();

            m_database.scriptIcons = ApplyIcons(m_scriptIconByType.Values.ToArray());
            WriteDatabase();
        }

        private void PopulateScriptIcon(MightyType mightyType, ScriptIconAttribute attribute)
        {
            var type = mightyType.MemberInfo;
            var ignored = type.GetCustomAttribute(typeof(IgnoreScriptIconAttribute), true) != null;
            var target = attribute.Target;

            var priority = GetPriority(mightyType, target, attribute);
            var icon = GetTexture(mightyType, target, attribute);

            if (m_scriptIconByType.TryGetValue(type, out var scriptIcon) && scriptIcon.IsIconValid && scriptIcon.priority >= priority)
            {
                scriptIcon.ignored = ignored;
                if (scriptIcon.icon == icon) scriptIcon.SetUsed();

                scriptIcon.SetValid();

                return;
            }

            if (scriptIcon == null) scriptIcon = new ScriptIcon();

            scriptIcon.SetAssembly(mightyType.Assembly);
            scriptIcon.SetType(type);

            scriptIcon.icon = icon;
            scriptIcon.priority = priority;

            scriptIcon.ignored = ignored;
            scriptIcon.SetValid();

            m_scriptIconByType[type] = scriptIcon;
        }

        private bool RemoveInvalidScriptIcons()
        {
            var everythingUsed = true;
            foreach (var pair in m_scriptIconByType.ToArray())
            {
                if (!pair.Value.IsValid)
                    m_scriptIconByType.Remove(pair.Key);

                if (pair.Value.IsUsed) continue;
                m_scriptIconByType.Remove(pair.Key);
                everythingUsed = false;
            }

            return everythingUsed;
        }

        private ScriptIcon[] ApplyIcons(ScriptIcon[] icons)
        {
            foreach (var scriptIcon in icons)
            {
                if (scriptIcon.ignored) continue;

                if (!scriptIcon.IsIconValid || scriptIcon.CompareIcons()) continue;

                var monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptIcon.GetScriptPath());

                SetIconForObject.Invoke(null, new object[] {monoScript, scriptIcon.icon});
                CopyMonoScriptIconToImporters.Invoke(null, new object[] {monoScript});
            }

            return icons;
        }

        private static Texture2D GetTexture(BaseMightyMember mightyMember, object target, ScriptIconAttribute attribute)
        {
            var path = attribute.IconPath;
            if (attribute.PathAsCallback && mightyMember.GetValueFromMember(target, attribute.IconPath, out string pathValue))
                path = pathValue;

            return MightyGUIUtilities.GetTexture(path);
        }

        private static int GetPriority(BaseMightyMember mightyMember, object target, ScriptIconAttribute attribute)
        {
            var priority = attribute.Priority;

            if (mightyMember.GetValueFromMember(target, attribute.PriorityCallback, out int priorityValue))
                priority = priorityValue;

            return priority;
        }
    }
}
#endif