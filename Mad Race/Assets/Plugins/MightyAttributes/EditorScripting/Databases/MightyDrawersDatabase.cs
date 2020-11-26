#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;

namespace MightyAttributes.Editor
{
    [InitializeOnLoad]
    public static class MightyDrawersDatabase
    {
        private static Dictionary<Type, IMightyDrawer> m_drawersByAttributeType;

        private static bool m_populatedInConstructor;

        static MightyDrawersDatabase()
        {
            PopulateDatabase();
            m_populatedInConstructor = true;
        }

        [DidReloadScripts]
        private static void PopulateDatabase()
        {
            if (!m_populatedInConstructor)
            {
                m_drawersByAttributeType = new Dictionary<Type, IMightyDrawer>();

                var mightyAssembly = typeof(IMightyDrawer).Assembly;
                var pluginsAssembly = TypeUtilities.GetPluginsAssembly();

                ExtractAssembly(mightyAssembly);
                if (mightyAssembly != pluginsAssembly)
                    ExtractAssembly(pluginsAssembly);

                ExtractAssembly(TypeUtilities.GetMainAssembly());
            }
            else
                m_populatedInConstructor = false;
        }

        private static void ExtractAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || !typeof(IMightyDrawer).IsAssignableFrom(type)) continue;

                var baseType = type.BaseType;
                if (baseType == null) continue;

                foreach (var genericType in baseType.GetGenericArguments())
                    if (!m_drawersByAttributeType.ContainsKey(genericType))
                        m_drawersByAttributeType.Add(genericType, (IMightyDrawer) Activator.CreateInstance(type));
            }
        }

        public static T GetDrawerForAttribute<T>(Type attributeType) where T : IMightyDrawer
        {
            if (m_drawersByAttributeType.TryGetValue(attributeType, out var drawer)) return (T) drawer;

            if (!typeof(IInheritDrawer).IsAssignableFrom(attributeType)) return default;

            var baseType = attributeType.BaseType;
            return baseType == null ? default : GetDrawerForAttribute<T>(attributeType.BaseType);
        }

        public static T GetDrawer<T>() where T : IMightyDrawer
        {
            foreach (var pair in m_drawersByAttributeType)
                if (pair.Value is T drawer)
                    return drawer;
            return default;
        }

        public static void ClearAllCaches()
        {
            foreach (var mightyDrawer in m_drawersByAttributeType.Values) mightyDrawer.ClearDrawerCache();
        }

        public static void ClearCachesOfTypes(params Type[] attributeTypes)
        {
            foreach (var attributeAndDrawer in m_drawersByAttributeType)
                if (attributeTypes.Any(attributeType => attributeType.IsAssignableFrom(attributeAndDrawer.Key)))
                    attributeAndDrawer.Value.ClearDrawerCache();
        }

        public static void ClearCachesNotOfTypes(params Type[] attributeTypes)
        {
            foreach (var attributeAndDrawer in m_drawersByAttributeType)
                if (attributeTypes.All(attributeType => !attributeType.IsAssignableFrom(attributeAndDrawer.Key)))
                    attributeAndDrawer.Value.ClearDrawerCache();
        }
    }
}
#endif