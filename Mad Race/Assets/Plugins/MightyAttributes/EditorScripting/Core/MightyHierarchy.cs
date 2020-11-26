#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MightyAttributes.Editor
{
    [InitializeOnLoad]
    public static class MightyHierarchy
    {
        private static readonly MightyMembersCache<Type> MightyTypesCache = new MightyMembersCache<Type>();

        static MightyHierarchy() => InitCallbacks();

        public static void InitCallbacks()
        {
            if (MightySettingsServices.Activated)
            {
                EditorSceneManager.sceneOpened += OnSceneOpened;

                EditorApplication.hierarchyChanged += OnHierarchyChanged;
                EditorApplication.hierarchyWindowItemOnGUI += OnItemGUI;
            }
            else
            {
                EditorSceneManager.sceneOpened -= OnSceneOpened;
                EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            }
        }

        [MenuItem("Tools/[Mighty]Attributes/Refresh Hierarchy", false, 53)]
        public static void RefreshHierarchy()
        {
            InitHierarchy();
            MightyDebugUtilities.MightyDebug("Hierarchy Refreshed");
        }

        [DidReloadScripts]
        public static void InitHierarchy()
        {
            if (!MightySettingsServices.Activated) return;

            CacheScenes();
            EnableHierarchy();
            OnHierarchyChanged();
            EditorApplication.RepaintHierarchyWindow();
        }

        public static void RefreshHierarchyDrawers()
        {
            foreach (var mightyType in MightyTypesCache.Values)
                mightyType.RefreshDrawers();
        }

        private static void CacheScenes()
        {
            MightyTypesCache.ClearCache();

            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var monoBehaviour in ReferencesUtilities.FindAllObjects<MonoBehaviour>(SceneManager.GetSceneAt(i)))
                {
                    if (monoBehaviour == null) continue;
                    var type = monoBehaviour.GetType();
                    if (!type.HasAttributeOfType<BaseHierarchyAttribute>()) continue;

                    var mightyComponent =
                        (MightyComponent) MightyTypesCache.Add(new MightyComponent(type, new MightyComponentContext(monoBehaviour)));
                    var wrappedAttributes = mightyComponent.GetWrappedAttributes<BaseHierarchyAttribute>();
                    mightyComponent.CacheHierarchyForType(type, wrappedAttributes);
                }
        }

        private static void EnableHierarchy()
        {
            foreach (var mightyType in MightyTypesCache.Values)
                mightyType.EnableDrawers();
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            CacheScenes();
            EnableHierarchy();
            OnHierarchyChanged();
        }

        private static void OnHierarchyChanged()
        {
            if (!MightySettingsServices.Activated)
            {
                EditorSceneManager.sceneOpened -= OnSceneOpened;
                EditorApplication.hierarchyChanged -= OnHierarchyChanged;
                return;
            }

            foreach (var mightyType in MightyTypesCache.Values)
                if (mightyType.TryGetAttributes(out BaseHierarchyAttribute[] attributes))
                    foreach (var attribute in attributes)
                        ((IHierarchyDrawer) attribute.Drawer).OnHierarchyChanged((MightyComponent) mightyType, 
                            attribute);
        }

        private static void OnItemGUI(int instanceID, Rect selectionRect)
        {
            if (!MightySettingsServices.Activated)
            {
                EditorSceneManager.sceneOpened -= OnSceneOpened;
                EditorApplication.hierarchyChanged -= OnHierarchyChanged;
                return;
            }

            foreach (var mightyType in MightyTypesCache.Values)
            {
                if (!(mightyType is MightyComponent mightyComponent)) continue;
                if (mightyComponent.ComponentContext.GameObject.gameObject.GetInstanceID() != instanceID) continue;
                if (!mightyComponent.TryGetAttributes(out BaseHierarchyAttribute[] attributes)) continue;
                foreach (var attribute in attributes)
                    ((IHierarchyDrawer) attribute.Drawer).OnGUI(mightyComponent, selectionRect, attribute);
            }
        }
    }
}
#endif