#if UNITY_EDITOR
using System;
using System.Reflection;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    [InitializeOnLoad]
    public static class MightyReloadScripts
    {
        private static readonly MightyMembersCache<Type> MightyTypesCache = new MightyMembersCache<Type>();

        private static readonly Assembly MainAssembly, PluginsAssembly;

        static MightyReloadScripts()
        {
            if (!MightySettingsServices.Activated) return;

            MainAssembly = TypeUtilities.GetMainAssembly();
            PluginsAssembly = TypeUtilities.GetPluginsAssembly();

            CacheAssemblies(typeof(BaseReloadScriptsAttribute));
        }

        #region Cache

        private static void CacheAssemblies(Type attributeType)
        {
            CacheAssembly(MainAssembly, attributeType);
            CacheAssembly(PluginsAssembly, attributeType);
        }

        private static void CacheAssembly(Assembly assembly, Type attributeType)
        {
            if (assembly == null) return;

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(BaseWrapperAttribute)) || !type.HasAttributeOfType(attributeType)) continue;

                if (type.IsSubclassOf(typeof(Component)))
                    CacheComponents(assembly, type);
                else if (type.IsSubclassOf(typeof(Object)))
                    CacheAssets(assembly, type);
                else
                    CacheScriptInstance(assembly, type);
            }
        }

        private static void CacheComponents(Assembly assembly, Type type)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (Object obj in type.FindAllObjects(SceneManager.GetSceneAt(i)))
                {
                    var mightyType = (MightyType) MightyTypesCache.Add(new MightyType(type, assembly, obj));
                    var wrappedAttributes = mightyType.GetWrappedAttributes<BaseReloadScriptsAttribute>();
                    mightyType.CacheReloadScriptForType(type, wrappedAttributes);
                }
        }

        private static void CacheAssets(Assembly assembly, Type type)
        {
            foreach (var asset in type.FindAssetsOfType())
            {
                var mightyType = (MightyType) MightyTypesCache.Add(new MightyType(type, assembly, asset));
                var wrappedAttributes = mightyType.GetWrappedAttributes<BaseReloadScriptsAttribute>();
                mightyType.CacheReloadScriptForType(type, wrappedAttributes);
            }
        }

        private static void CacheScriptInstance(Assembly assembly, Type type)
        {
            var instance = Activator.CreateInstance(type);

            var mightyType = (MightyType) MightyTypesCache.Add(new MightyType(type, assembly, instance));
            var wrappedAttributes = mightyType.GetWrappedAttributes<BaseReloadScriptsAttribute>();
            mightyType.CacheReloadScriptForType(type, wrappedAttributes);
        }

        #endregion /Cache

        [DidReloadScripts]
        public static void OnReloadScripts()
        {
            if (!MightySettingsServices.ActivateReloadScripts) return;
            if (!MightySettingsServices.Activated) return;

            EnableReloadScripts();

            BeginReloadScripts();
            EndReloadScripts();

            MightyDebugUtilities.MightyDebug("Reload Scripts Applied", MightyDebugUtilities.LogType.ReloadScripts);
        }

        [MenuItem("Tools/[Mighty]Attributes/Apply Reload Scripts", false, 52)]
        public static void ApplyScriptReload()
        {
            if (!MightySettingsServices.Activated) return;

            EnableReloadScripts();

            BeginReloadScripts();
            EndReloadScripts();

            DisableReloadScripts();

            MightyDebugUtilities.MightyDebug("Reload Scripts Applied");
        }
        
        private static void EnableReloadScripts()
        {
            foreach (var mightyType in MightyTypesCache.Values)
                mightyType.EnableDrawers();
        }

        private static void DisableReloadScripts() => MightyDrawersDatabase.ClearCachesOfTypes(typeof(BaseReloadScriptsAttribute));

        public static void BeginReloadScripts()
        {
            foreach (var mightyType in MightyTypesCache.Values)
            {
                if (!mightyType.TryGetAttributes(out BaseReloadScriptsAttribute[] attributes)) continue;

                foreach (var attribute in attributes)
                    ((IReloadScriptsDrawer) attribute.Drawer).BeginReloadScripts((MightyType) mightyType, attribute);
            }
        }

        public static void EndReloadScripts()
        {
            foreach (var mightyType in MightyTypesCache.Values)
            {
                if (!mightyType.TryGetAttributes(out BaseReloadScriptsAttribute[] attributes)) continue;

                foreach (var attribute in attributes)
                    ((IReloadScriptsDrawer) attribute.Drawer).EndReloadScripts((MightyType) mightyType, attribute);
            }
        }
    }
}
#endif