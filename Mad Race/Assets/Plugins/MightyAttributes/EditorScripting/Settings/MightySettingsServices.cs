#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class MightySettingsServices
    {
        [Serializable]
        public class MightySettings : BaseSettings
        {
            public bool activated;

            public string mainAssemblyName;
            public string pluginsAssemblyName;

            public bool autoValuesOnReloadScripts;
            public bool autoValuesOnPlay;
            public bool autoValuesOnBuild;
            public bool displayAutoValuesLogs;

            public bool activateReloadScripts;
            public bool displayReloadScriptsLogs;

            public bool firstTime;
            public int docVersion;
            public string docPath;

            protected override Type ServicesType => typeof(MightySettingsServices);

            public override void ResetToDefault()
            {
                activated = true;

                mainAssemblyName = DEFAULT_MAIN_ASSEMBLY_NAME;
                pluginsAssemblyName = DEFAULT_PLUGINS_ASSEMBLY_NAME;

                autoValuesOnReloadScripts = true;
                autoValuesOnPlay = true;
                autoValuesOnBuild = true;
                displayAutoValuesLogs = true;

                activateReloadScripts = true;
                displayReloadScriptsLogs = true;

                firstTime = true;
                docVersion = -1;

                docPath = GetDefaultDocPath();
            }
        }

        public const string DEFAULT_MAIN_ASSEMBLY_NAME = "Assembly-CSharp";
        public const string DEFAULT_PLUGINS_ASSEMBLY_NAME = "Assembly-CSharp-firstpass";

        public static string GetDefaultDocPath() => FileUtilities.GetDirectoryPath(Application.dataPath);

        public static string GetSettingsDirectory() => FileUtilities.GetAbsoluteScriptPathByType(typeof(MightySettingsServices), false);
        public static string GetSettingsPath() => Path.Combine(GetSettingsDirectory(), "MightySettings.json");

        private static MightySettings m_settings;

        #region Properties

        public static bool Activated
        {
            get => m_settings.activated;
            set
            {
                if (value == m_settings.activated) return;

                MightyHierarchy.InitCallbacks();
                m_settings.activated = value;
                SaveSettings();
            }
        }

        public static string MainAssemblyName
        {
            get => m_settings.mainAssemblyName;
            set
            {
                if (value == m_settings.mainAssemblyName) return;

                m_settings.mainAssemblyName = value;
                SaveSettings();
            }
        }

        public static string PluginsAssemblyName
        {
            get => m_settings.pluginsAssemblyName;
            set
            {
                if (value == m_settings.pluginsAssemblyName) return;

                m_settings.pluginsAssemblyName = value;
                SaveSettings();
            }
        }

        public static bool AutoValuesOnReloadScripts
        {
            get => m_settings.autoValuesOnReloadScripts;
            set
            {
                if (value == m_settings.autoValuesOnReloadScripts) return;

                m_settings.autoValuesOnReloadScripts = value;
                SaveSettings();
            }
        }

        public static bool AutoValuesOnPlay
        {
            get => m_settings.autoValuesOnPlay;
            set
            {
                if (value == m_settings.autoValuesOnPlay) return;

                m_settings.autoValuesOnPlay = value;
                SaveSettings();
            }
        }

        public static bool AutoValuesOnBuild
        {
            get => m_settings.autoValuesOnBuild;
            set
            {
                if (value == m_settings.autoValuesOnBuild) return;

                m_settings.autoValuesOnBuild = value;
                SaveSettings();
            }
        }

        public static bool DisplayAutoValuesLogs
        {
            get => m_settings.displayAutoValuesLogs;
            set
            {
                if (value == m_settings.displayAutoValuesLogs) return;

                m_settings.displayAutoValuesLogs = value;
                SaveSettings();
            }
        }

        public static bool ActivateReloadScripts
        {
            get => m_settings.activateReloadScripts;
            set
            {
                if (value == m_settings.activateReloadScripts) return;

                m_settings.activateReloadScripts = value;
                SaveSettings();
            }
        }

        public static bool DisplayReloadScriptsLogs
        {
            get => m_settings.displayReloadScriptsLogs;
            set
            {
                if (value == m_settings.displayReloadScriptsLogs) return;

                m_settings.displayReloadScriptsLogs = value;
                SaveSettings();
            }
        }

        public static bool FirstTime
        {
            get => m_settings.firstTime;
            set
            {
                if (value == m_settings.firstTime) return;

                m_settings.firstTime = value;
                SaveSettings();
            }
        }

        public static int DocVersion
        {
            get => m_settings.docVersion;
            set
            {
                if (value == m_settings.docVersion) return;

                m_settings.docVersion = value;
                SaveSettings();
            }
        }

        public static string DocPath
        {
            get => m_settings.docPath;
            set
            {
                if (value == m_settings.docPath) return;

                m_settings.docPath = value;
                SaveSettings();
            }
        }

        #endregion /Properties

        static MightySettingsServices() => LoadSettings();

        private static void LoadSettings() => m_settings = BaseSettings.Load<MightySettings>();

        private static void SaveSettings() => m_settings.Save();

        public static bool AnyAutoValues => AutoValuesOnReloadScripts || AutoValuesOnPlay || AutoValuesOnBuild;

        public static void ResetAssemblyNamesToDefault()
        {
            MainAssemblyName = DEFAULT_MAIN_ASSEMBLY_NAME;
            PluginsAssemblyName = DEFAULT_PLUGINS_ASSEMBLY_NAME;
        }

        public static bool CanDisplayLog(MightyDebugUtilities.LogType logType)
        {
            switch (logType)
            {
                case MightyDebugUtilities.LogType.AutoValues:
                    return DisplayAutoValuesLogs;
                case MightyDebugUtilities.LogType.ReloadScripts:
                    return DisplayReloadScriptsLogs;
            }

            return true;
        }

        public static void ResetDocVersion()
        {
            m_settings.docVersion = -1;
            SaveSettings();
        }
    }
}
#endif