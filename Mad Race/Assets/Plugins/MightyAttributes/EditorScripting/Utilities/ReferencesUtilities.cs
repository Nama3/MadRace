#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MightyAttributes.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class ReferencesUtilities
{
    public static readonly ObjectIDGenerator IDGenerator = new ObjectIDGenerator();

    public static long GetUniqueID(object obj) => IDGenerator.GetId(obj, out _);
    
    public static GameObject[] GetRootGameObjects(Scene? scene = null)
    {
        if (scene == null)
        {
            var objects = new List<GameObject>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var currentScene = SceneManager.GetSceneAt(i);
                if (currentScene.IsValid() && currentScene.isLoaded)
                    objects.AddRange(currentScene.GetRootGameObjects());
            }

            return objects.ToArray();
        }

        var notNullScene = (Scene) scene;
        return notNullScene.IsValid() && notNullScene.isLoaded ? notNullScene.GetRootGameObjects() : new GameObject[0];
    }

    #region FindObject

    public static T FindFirstObject<T>(Scene? scene = null, bool includeInactive = true) where T : Component
    {
        foreach (var rootObject in GetRootGameObjects(scene))
            if (rootObject.GetComponentsInChildren<T>(includeInactive).FirstOrDefault() is T t)
                return t;

        return null;
    }

    public static T[] FindAllObjects<T>(Scene? scene = null, bool includeInactive = true) where T : Component
    {
        var list = new List<T>();
        foreach (var o in GetRootGameObjects(scene)) list.AddRange(o.GetComponentsInChildren<T>(includeInactive));
        return list.ToArray();
    }    
    
    public static object[] FindAllObjects(this Type type, Scene? scene = null, bool includeInactive = true)
    {
        var list = new List<object>();
        foreach (var o in GetRootGameObjects(scene)) list.AddRange(o.GetComponentsInChildren(type, includeInactive));
        return list.ToArray();
    }

    public static Object FindObject(this Type type, Scene scene, string str, bool includeInactive,
        Func<GameObject, Type, bool, string, bool> comparer)
    {
        return GetRootGameObjects(scene).SelectMany(o => o.GetComponentsInChildren(type, includeInactive))
            .FirstOrDefault(c => comparer(c.gameObject, type, includeInactive, str));
    }

    public static Object FindObjectButSelf(this Type type, Object self, Scene scene, string str, bool includeInactive,
        Func<GameObject, Type, bool, string, bool> comparer)
    {
        return GetRootGameObjects(scene).SelectMany(o => o.GetComponentsInChildren(type, includeInactive))
            .FirstOrDefault(c => self.GetInstanceID() != c.GetInstanceID() && comparer(c.gameObject, type, includeInactive, str));
    }

    public static Object[] FindObjects(this Type type, Scene scene, string str, bool includeInactive,
        Func<GameObject, Type, bool, string, bool> comparer)
    {
        var list = new List<Object>();
        foreach (var o in GetRootGameObjects(scene))
            list.AddRange(o.GetComponentsInChildren(type, includeInactive)
                .Where(c => comparer(c.gameObject, type, includeInactive, str)));
        return list.ToArray();
    }

    public static Object[] FindObjectsButSelf(this Type type, Object self, Scene scene, string str, bool includeInactive,
        Func<GameObject, Type, bool, string, bool> comparer)
    {
        var list = new List<Object>();
        foreach (var o in GetRootGameObjects(scene))
            list.AddRange(o.GetComponentsInChildren(type, includeInactive)
                .Where(c => self.GetInstanceID() != c.GetInstanceID() && comparer(c.gameObject, type, includeInactive, str)));
        return list.ToArray();
    }

    public static Object FindObject(this SerializedProperty property, string str, bool includeInactive, bool ignoreSelf,
        Func<GameObject, Type, bool, string, bool> comparer)
    {
        var propertySystemType = property.GetSystemType();
        if (ignoreSelf)
        {
            var self = property.GetTargetObject();
            if (propertySystemType.IsInstanceOfType(self))
                return propertySystemType.FindObjectButSelf(self, property.GetGameObject().scene, str, includeInactive, comparer);
        }

        return propertySystemType.FindObject(property.GetGameObject().scene, str, includeInactive, comparer);
    }

    public static Object[] FindObjects(this SerializedProperty property, string str, bool includeInactive, bool ignoreSelf,
        Func<GameObject, Type, bool, string, bool> comparer)
    {
        var propertySystemType = property.GetSystemType();
        if (ignoreSelf)
        {
            var self = property.GetTargetObject();
            if (propertySystemType.IsInstanceOfType(self))
                return propertySystemType.FindObjectsButSelf(self, property.GetGameObject().scene, str, includeInactive, comparer);
        }

        return propertySystemType.FindObjects(property.GetGameObject().scene, str, includeInactive, comparer);
    }

    public static Object FindObjectWithTag(this SerializedProperty property, string tag = null, bool includeInactive = false,
        bool ignoreSelf = false) =>
        property.FindObject(tag, includeInactive, ignoreSelf, CompareTypeAndTag);

    public static Object[] FindObjectsWithTag(this SerializedProperty property, string tag = null, bool includeInactive = false,
        bool ignoreSelf = false) =>
        property.FindObjects(tag, includeInactive, ignoreSelf, CompareTypeAndTag).ToArray();

    public static Object FindObjectWithName(this SerializedProperty property, string name = null, bool includeInactive = false,
        bool ignoreSelf = false) =>
        property.FindObject(name, includeInactive, ignoreSelf, CompareTypeAndName);

    public static Object[] FindObjectsWithName(this SerializedProperty property, string name = null, bool includeInactive = false,
        bool ignoreSelf = false) =>
        property.FindObjects(name, includeInactive, ignoreSelf, CompareTypeAndName).ToArray();

    public static Object FindObjectWithLayer(this SerializedProperty property, string layer = null, bool includeInactive = false,
        bool ignoreSelf = false) =>
        property.FindObject(layer, includeInactive, ignoreSelf, CompareTypeAndLayer);

    public static Object[] FindObjectsWithLayer(this SerializedProperty property, string layer = null, bool includeInactive = false,
        bool ignoreSelf = false) =>
        property.FindObjects(layer, includeInactive, ignoreSelf, CompareTypeAndLayer).ToArray();

    #endregion /FindObject

    #region FindAsset

    public static T FindAssetOfType<T>(string name = null, string[] folders = null) where T : Object => 
        (T) FindAssetOfType(typeof(T), name, folders);
    
    public static T[] FindAssetsOfType<T>(string name = null, string[] folders = null) where T : Object => 
        FindAssetsOfType(typeof(T), name, folders).Cast<T>().ToArray();

    public static Object FindAssetOfType(this Type type, string name = null, string[] folders = null)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (folders != null)
            {
                return AssetDatabase.FindAssets($"{name} t:{type}".Replace("UnityEngine.", ""), folders)
                    .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                    .FirstOrDefault(asset => asset != null);
            }

            return AssetDatabase.FindAssets($"{name} t:{type}".Replace("UnityEngine.", ""))
                .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                .FirstOrDefault(asset => asset != null);
        }

        if (folders != null)
        {
            return AssetDatabase.FindAssets($"t:{type}".Replace("UnityEngine.", ""), folders)
                .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                .FirstOrDefault(asset => asset != null);
        }

        return AssetDatabase.FindAssets($"t:{type}".Replace("UnityEngine.", "")).Select(AssetDatabase.GUIDToAssetPath)
            .Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type)).FirstOrDefault(asset => asset != null);
    }

    public static Object[] FindAssetsOfType(this Type type, string name = null, string[] folders = null)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (folders != null)
            {
                return AssetDatabase.FindAssets($"{name} t:{type}".Replace("UnityEngine.", ""), folders)
                    .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                    .Where(asset => asset != null).ToArray();
            }

            return AssetDatabase.FindAssets($"{name} t:{type}".Replace("UnityEngine.", ""))
                .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                .Where(asset => asset != null).ToArray();
        }

        if (folders != null)
        {
            return AssetDatabase.FindAssets($"t:{type}".Replace("UnityEngine.", ""), folders)
                .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                .Where(asset => asset != null).ToArray();
        }

        return AssetDatabase.FindAssets($"t:{type}".Replace("UnityEngine.", "")).Select(AssetDatabase.GUIDToAssetPath)
            .Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type)).Where(asset => asset != null).ToArray();
    }

    public static Object FindAssetWithName(this SerializedProperty property, string name) => property.GetSystemType().FindAssetOfType(name);

    public static Object[] FindAssetsWithName(this SerializedProperty property, string name) => 
        property.GetSystemType().FindAssetsOfType(name);

    public static Object FindAssetInFolders(this SerializedProperty property, string name, string[] folders) => 
        property.GetSystemType().FindAssetOfType(name, folders);

    public static Object[] FindAssetsInFolders(this SerializedProperty property, string name, string[] folders) => 
        property.GetSystemType().FindAssetsOfType(name, folders);

    #endregion

    #region GetInChildren

    public static Component GetComponentInChildrenOnly(this GameObject source, Type type, string str, bool includeInactive,
        Func<GameObject, string, bool> comparer) =>
        string.IsNullOrEmpty(str)
            ? source.GetComponentsInChildren(type, includeInactive)
                .FirstOrDefault(c => c.gameObject.GetInstanceID() != source.GetInstanceID())
            : source.GetComponentsInChildren(type, includeInactive).FirstOrDefault(c =>
                c.gameObject.GetInstanceID() != source.GetInstanceID() && comparer(c.gameObject, str));

    public static Component GetComponentInChildren(this GameObject source, Type type, string str, bool includeInactive,
        Func<GameObject, string, bool> comparer) =>
        string.IsNullOrEmpty(str)
            ? source.GetComponentInChildren(type, includeInactive)
            : source.GetComponentsInChildren(type, includeInactive).FirstOrDefault(item => comparer(item.gameObject, str));

    public static Component[] GetComponentsInChildrenOnly(this GameObject source, Type type, string str, bool includeInactive,
        Func<GameObject, string, bool> comparer) =>
        string.IsNullOrEmpty(str)
            ? source.GetComponentsInChildren(type, includeInactive)
                .Where(c => c.gameObject.GetInstanceID() != source.GetInstanceID()).ToArray()
            : source.GetComponentsInChildren(type, includeInactive).Where(c =>
                c.gameObject.GetInstanceID() != source.GetInstanceID() && comparer(c.gameObject, str)).ToArray();

    public static IEnumerable<Component> GetComponentsInChildren(this GameObject source, Type type, string str, bool includeInactive,
        Func<GameObject, string, bool> comparer) =>
        string.IsNullOrEmpty(str)
            ? source.GetComponentsInChildren(type, includeInactive)
            : source.GetComponentsInChildren(type, includeInactive).Where(item => comparer(item.gameObject, str));

    public static Component GetComponentInChildren(this SerializedProperty property, string str, bool includeInactive, bool ignoreSelf,
        Func<GameObject, string, bool> comparer) =>
        ignoreSelf
            ? property.GetGameObject().GetComponentInChildrenOnly(property.GetSystemType(), str, includeInactive, comparer)
            : property.GetGameObject().GetComponentInChildren(property.GetSystemType(), str, includeInactive, comparer);

    public static Component[] GetComponentsInChildren(this SerializedProperty property, string str, bool includeInactive,
        bool ignoreSelf, Func<GameObject, string, bool> comparer) =>
        ignoreSelf
            ? property.GetGameObject().GetComponentsInChildrenOnly(property.GetSystemType(), str, includeInactive, comparer).ToArray()
            : property.GetGameObject().GetComponentsInChildren(property.GetSystemType(), str, includeInactive, comparer).ToArray();

    public static Component GetComponentInChildrenWithTag(this SerializedProperty property, string tag = null,
        bool includeInactive = false, bool ignoreSelf = false) =>
        property.GetComponentInChildren(tag, includeInactive, ignoreSelf, TagComparer);

    public static Component[] GetComponentsInChildrenWithTag(this SerializedProperty property, string tag = null,
        bool includeInactive = false, bool ignoreSelf = false) =>
        property.GetComponentsInChildren(tag, includeInactive, ignoreSelf, TagComparer);

    public static Component GetComponentInChildrenWithName(this SerializedProperty property, string name = null,
        bool includeInactive = false, bool ignoreSelf = false) =>
        property.GetComponentInChildren(name, includeInactive, ignoreSelf, NameComparer);

    public static Component[] GetComponentsInChildrenWithName(this SerializedProperty property, string layer = null,
        bool includeInactive = false, bool ignoreSelf = false) =>
        property.GetComponentsInChildren(layer, includeInactive, ignoreSelf, NameComparer);

    public static Component GetComponentInChildrenWithLayer(this SerializedProperty property, string name = null,
        bool includeInactive = false, bool ignoreSelf = false) =>
        property.GetComponentInChildren(name, includeInactive, ignoreSelf, LayerComparer);

    public static Component[] GetComponentsInChildrenWithLayer(this SerializedProperty property, string layer = null,
        bool includeInactive = false, bool ignoreSelf = false) =>
        property.GetComponentsInChildren(layer, includeInactive, ignoreSelf, LayerComparer);

    #endregion

    #region GetInParent

    public static Component GetComponentInParent(this GameObject gameObject, Type type, string str,
        Func<GameObject, string, bool> comparer) =>
        string.IsNullOrEmpty(str)
            ? gameObject.GetComponentInParent(type)
            : gameObject.GetComponentsInParent(type).FirstOrDefault(item => comparer(item.gameObject, str));

    public static Component[] GetComponentsInParent(this GameObject gameObject, Type type, string str,
        Func<GameObject, string, bool> comparer) =>
        string.IsNullOrEmpty(str)
            ? gameObject.GetComponentsInParent(type)
            : gameObject.GetComponentsInParent(type).Where(item => comparer(item.gameObject, str)).ToArray();

    public static Component GetComponentInParent(this SerializedProperty property, string str, Func<GameObject, string, bool> comparer) =>
        property.GetGameObject().GetComponentInParent(property.GetSystemType(), str, comparer);

    public static Component[] GetComponentsInParent(this SerializedProperty property, string str,
        Func<GameObject, string, bool> comparer) =>
        property.GetGameObject().GetComponentsInParent(property.GetSystemType(), str, comparer);

    public static Component GetComponentInParentWithTag(this SerializedProperty property, string tag = null) =>
        property.GetComponentInParent(tag, TagComparer);

    public static Component[] GetComponentsInParentWithTag(this SerializedProperty property, string tag = null) =>
        property.GetComponentsInParent(tag, TagComparer);

    public static Component GetComponentInParentWithName(this SerializedProperty property, string name = null) =>
        property.GetComponentInParent(name, NameComparer);

    public static Component[] GetComponentsInParentWithName(this SerializedProperty property, string layer = null) =>
        property.GetComponentsInParent(layer, NameComparer);

    public static Component GetComponentInParentWithLayer(this SerializedProperty property, string name = null) =>
        property.GetComponentInParent(name, LayerComparer);

    public static Component[] GetComponentsInParentWithLayer(this SerializedProperty property, string layer = null) =>
        property.GetComponentsInParent(layer, LayerComparer);

    #endregion

    #region Comparer

    public static bool CompareArrays(this SerializedProperty property, Object[] array)
    {
        if (!property.IsCollection()) return false;
        if (property.arraySize != array.Length) return false;

        var propertyArray = new Object[property.arraySize];
        for (var i = 0; i < propertyArray.Length; i++)
            propertyArray[i] = property.GetArrayElementAtIndex(i).objectReferenceValue;

        return new HashSet<Object>(propertyArray).SetEquals(array);
    }

    public static bool CompareArrays(this SerializedProperty property, object[] array, object target)
    {
        if (!property.IsCollection()) return false;
        if (property.arraySize != array.Length) return false;
        if (property.arraySize < 1) return true;
        var propertyType = property.GetArrayElementAtIndex(0).propertyType;
        var propertyArray = PropertyUtilities.SetArrayGenericValue(property, propertyType, target);

        if (propertyArray == null) return false;
        switch (propertyType)
        {
            case SerializedPropertyType.Enum:
            {
                var comparer = new EnumComparer<object>();
                return new HashSet<object>(propertyArray, comparer).SetEquals(new HashSet<object>(array, comparer));
            }
            case SerializedPropertyType.Generic:
            {
                var comparer = new GenericComparer<object>();
                return new HashSet<object>(propertyArray, comparer).SetEquals(new HashSet<object>(array, comparer));
            }
            default:
                return new HashSet<object>(propertyArray).SetEquals(array);
        }
    }

    public static bool CompareArrays(this SerializedProperty property, SerializedProperty other, object target)
    {
        if (!property.IsCollection() || !other.IsCollection()) return false;
        if (property.arraySize != other.arraySize) return false;
        if (property.propertyType != other.propertyType) return false;
        if (property.arraySize < 1) return true;

        var propertyType = property.GetArrayElementAtIndex(0).propertyType;
        var propertyArray = PropertyUtilities.SetArrayGenericValue(property, propertyType, target);
        if (propertyArray == null) return false;
        var otherArray = PropertyUtilities.SetArrayGenericValue(other, propertyType, target);
        if (otherArray == null) return false;

        return propertyType == SerializedPropertyType.Generic
            ? new HashSet<object>(propertyArray, new GenericComparer<object>()).SetEquals(
                new HashSet<object>(propertyArray, new GenericComparer<object>()))
            : new HashSet<object>(propertyArray).SetEquals(otherArray);
    }

    private class GenericComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y) => x == null && y == null || x != null && y != null && x.Equals(y);

        public int GetHashCode(T obj) => obj.GetHashCode();
    }

    private class EnumComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y) => Convert.ToInt32(x) == Convert.ToInt32(y);

        public int GetHashCode(T obj) => obj.GetHashCode();
    }

    public static bool CompareTypeAndTag(this GameObject go, Type type, bool includeInactive, string tag)
    {
        if (string.IsNullOrEmpty(tag))
            return go.GetComponentInChildren(type, includeInactive) != null;
        var t = go.GetComponentInChildren(type, includeInactive);
        return t != null && t.gameObject.TagComparer(tag);
    }

    public static bool CompareTypeAndName(this GameObject go, Type type, bool includeInactive, string name)
    {
        if (string.IsNullOrEmpty(name))
            return go.GetComponentInChildren(type, includeInactive) != null;
        var t = go.GetComponentInChildren(type, includeInactive);
        return t != null && t.gameObject.NameComparer(name);
    }

    public static bool CompareTypeAndLayer(this GameObject go, Type type, bool includeInactive, string layer)
    {
        if (string.IsNullOrEmpty(layer))
            return go.GetComponentInChildren(type, includeInactive) != null;
        var t = go.GetComponentInChildren(type, includeInactive);
        return t != null && t.gameObject.LayerComparer(layer);
    }

    public static bool TagComparer(this GameObject gameObject, string tag) => gameObject.CompareTag(tag);

    public static bool NameComparer(this GameObject gameObject, string name) => gameObject.name == name;

    public static bool LayerComparer(this GameObject gameObject, string layer) => gameObject.layer == LayerMask.NameToLayer(layer);

    #endregion
}
#endif