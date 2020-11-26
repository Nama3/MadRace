#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [Serializable]
    public abstract class BaseSettings
    {
        protected abstract Type ServicesType { get; }

        public string GetSettingsDirectory() => FileUtilities.GetAbsoluteScriptPathByType(ServicesType, false);
        public string GetSettingsPath() => Path.Combine(GetSettingsDirectory(), $"{GetType().Name}.json");

        public abstract void ResetToDefault();

        public static T Load<T>() where T : BaseSettings, new() => (T) new T().Load();

        public BaseSettings Load()
        {
            var path = GetSettingsPath();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return (BaseSettings) JsonUtility.FromJson(json, GetType());
            }

            ResetToDefault();
            Save();
            return this;
        }

        public void Save()
        {
            var path = GetSettingsPath();
            var json = JsonUtility.ToJson(this, true);

            File.WriteAllText(path, json);
        }
    }
}
#endif