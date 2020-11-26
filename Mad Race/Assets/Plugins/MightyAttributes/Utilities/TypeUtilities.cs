using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using MightyAttributes.Editor;
#endif

namespace MightyAttributes.Utilities
{
    public static class TypeUtilities
    {
        public static Assembly GetAssemblyByName(string name) => AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == name);
        
#if UNITY_EDITOR
        public static Assembly GetMainAssembly() => GetAssemblyByName(MightySettingsServices.MainAssemblyName);
        public static Assembly GetPluginsAssembly() => GetAssemblyByName(MightySettingsServices.PluginsAssemblyName);

        public static Type[] GetChildrenTypes(Type baseType, bool ignoreAbstract = true, bool recursive = true,
            Assembly assembly = null)
        {
            if (assembly == null) assembly = baseType.Assembly;
            var types = assembly.GetTypes().Where(t =>
                (!ignoreAbstract || !t.IsAbstract) && (recursive && baseType.IsAssignableFrom(t) || t.BaseType == baseType)).ToList();

            return types.ToArray();
        }

        public static Type[] GetChildrenTypesInAssemblies(Type baseType, bool ignoreAbstract = true, bool recursive = true,
            params Assembly[] assemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
                if (assembly != null)
                    types.AddRange(GetChildrenTypes(baseType, ignoreAbstract, recursive, assembly));

            return types.ToArray();
        }
#endif

        public static bool GetTypeInAssembly(string typeName, string assemblyName, out Type type)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                var assembly = GetAssemblyByName(assemblyName);
                if (assembly != null && (type = assembly.GetType(typeName)) != null)
                    return true;
            }

            type = null;
            return false;
        }

        public static bool GetTypeInAssemblies(string typeName, out Type type, params Assembly[] assemblies)
        {
            if (!string.IsNullOrEmpty(typeName))
                foreach (var assembly in assemblies)
                    if (assembly != null && (type = assembly.GetType(typeName)) != null)
                        return true;

            type = null;
            return false;
        }
        
        public static IEnumerable<Type> GetSubInterfaces(Type baseType, bool includeInherited = true)
        {
            foreach (var t in baseType.Assembly.GetTypes())
                if (t.IsInterface && GetInterfaces(t, includeInherited).Contains(baseType))
                    yield return t;
        }

        public static IEnumerable<Type> GetAllSubTypes(Type baseType, bool excludeAbstracts = true)
        {
            foreach (var t in baseType.Assembly.GetTypes())
                if (t.IsClass && (!excludeAbstracts || !t.IsAbstract) && t.IsSubclassOf(baseType))
                    yield return t;
        }

        public static Type GetSubType(Type baseType, bool excludeAbstracts = true)
        {
            foreach (var t in baseType.Assembly.GetTypes())
                if (t.IsClass && (!excludeAbstracts || !t.IsAbstract) && t.IsSubclassOf(baseType))
                    return t;
            return null;
        }

        public static IEnumerable<Type> GetSubTypesWithGenericParameter(Type baseType, Type genericParameterType,
            bool excludeAbstracts = true)
        {
            foreach (var t in Assembly.GetAssembly(baseType).GetTypes())
                if (t.IsClass && (!excludeAbstracts || !t.IsAbstract) && t.IsSubclassOf(baseType) &&
                    t.BaseType.GetGenericArguments().Contains(genericParameterType))
                    yield return t;
        }

        public static Type GetSubTypeWithGenericParameter(Type baseType, Type genericParameterType,
            bool excludeAbstracts = true)
        {
            foreach (var t in Assembly.GetAssembly(baseType).GetTypes())
                if (t.IsClass && (!excludeAbstracts || !t.IsAbstract) && t.IsSubclassOf(baseType) &&
                    t.BaseType.GetGenericArguments().Contains(genericParameterType))
                    return t;
            return null;
        }

        public static IEnumerable<Type> GetInterfaces(Type type, bool includeInherited)
        {
            return includeInherited || type.BaseType == null
                ? type.GetInterfaces()
                : type.GetInterfaces().Except(type.BaseType.GetInterfaces());
        }

        public static IEnumerable<Type> GetAllChildren<T>(params Type[] excludedTypes)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes(),
                    (domainAssembly, assemblyType) => new {domainAssembly, assemblyType})
                .Where(t => typeof(T).IsAssignableFrom(t.assemblyType))
                .Select(t => t.assemblyType).Where(t => !excludedTypes.Contains(t));
        }

        public static IEnumerable<Type> GetAllNonAbstractChildren<T>(params Type[] excludedTypes) =>
            GetAllChildren<T>(excludedTypes).Where(t => !t.IsAbstract);

        public static string GetUnityTypeName(Type type) => Regex.Replace(type.Name, "(.+)`[0-9]", "$1");

        public static bool TryConvertStringToType<T>(string str, out T value)
        {
            try
            {
                if (typeof(T).IsEnum && typeof(T).GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0)
                {
                    value = (T) Enum.Parse(typeof(T), str);
                }
                else
                {
                    value = (T) Convert.ChangeType(str, typeof(T));
                }
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
        
        public static bool IsSerializableClassOrStruct(this Type type)
        {
            if (type == typeof(bool)) return false;
            if (type.IsEnum || type == typeof(byte) || type == typeof(short) || type == typeof(int)) return false;
            if (type == typeof(long)) return false;
            if (type == typeof(float)) return false;
            if (type == typeof(double)) return false;
            if (type == typeof(char)) return false;
            if (type == typeof(string)) return false;
            if (type == typeof(Vector2)) return false;
            if (type == typeof(Vector3)) return false;
            if (type == typeof(Vector4)) return false;
            if (type == typeof(Rect)) return false;
            if (type == typeof(Bounds)) return false;
            if (type == typeof(Vector2Int)) return false;
            if (type == typeof(Vector3Int)) return false;
            if (type == typeof(RectInt)) return false;
            if (type == typeof(BoundsInt)) return false;
            if (type == typeof(Quaternion)) return false;
            if (type == typeof(LayerMask)) return false;
            if (type == typeof(Color)) return false;
            if (type == typeof(AnimationCurve)) return false;
            if (type == typeof(Gradient)) return false;
            if (typeof(Object).IsAssignableFrom(type)) return false;

            return type.GetCustomAttribute(typeof(SerializableAttribute), true) != null;
        }
    }
}