#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace MightyAttributes.Editor
{
    public interface IMethodDrawer : IMightyDrawer
    {
        void OnEnable(MightyMethod mightyMethod, BaseMethodAttribute baseAttribute);
        void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, BaseMethodAttribute baseAttribute);
        void OnModifiedProperties(bool modified, MightyMethod mightyMethod, BaseMethodAttribute baseAttribute);
    }

    public abstract class BaseMethodDrawer<T> : BaseMightyDrawer<T>, IMethodDrawer where T : BaseMethodAttribute
    {
        protected readonly MightyCache<bool> m_methodCache = new MightyCache<bool>();

        public void OnEnable(MightyMethod mightyMethod, BaseMethodAttribute baseAttribute) => OnEnable(mightyMethod, (T) baseAttribute);
        public void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, BaseMethodAttribute baseAttribute) =>
            OnInspectorGUI(canDraw, mightyMethod, (T) baseAttribute);

        public void OnModifiedProperties(bool modified, MightyMethod mightyMethod, BaseMethodAttribute baseAttribute) => 
            OnModifiedProperties(modified, mightyMethod, (T) baseAttribute);

        protected abstract void OnEnable(MightyMethod mightyMethod, T attribute);
        protected abstract void OnModifiedProperties(bool modified, MightyMethod mightyMethod, T attribute);
        protected abstract void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, T attribute);

        protected virtual void InvokeMethod(MightyMethod mightyMethod, T attribute)
        {
            if (!m_methodCache.Contains(mightyMethod)) EnableDrawer(mightyMethod, attribute);
            if (m_methodCache[mightyMethod])
            {
                var methodInfo = mightyMethod.MemberInfo;
                if (attribute.ExecuteInPlayMode || !EditorApplication.isPlaying)
                    methodInfo.Invoke(mightyMethod.Context.Target, null);
            }
            else
                MightyGUIUtilities.DrawHelpBox($"{typeof(T).Name} works only on methods with no parameters");
        }

        protected override void Enable(BaseMightyMember mightyMember, T attribute) =>
            m_methodCache[mightyMember] = mightyMember is MightyMember<MethodInfo> mightyMethod &&
                                          mightyMethod.MemberInfo.GetParameters().Length == 0;

        protected override void ClearCache() => m_methodCache.ClearCache();
    }
}
#endif