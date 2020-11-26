#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IButtonDrawer : IMethodDrawer
    {
    }
    
    public abstract class BaseButtonDrawer<T> : BaseMethodDrawer<T>, IButtonDrawer where T : BaseButtonAttribute
    {
        protected override void OnEnable(MightyMethod mightyMethod, T attribute)
        {
        }

        protected override void OnModifiedProperties(bool modified, MightyMethod mightyMethod, T attribute)
        {
        }

        protected override void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, T attribute)
        {
            if (canDraw) InvokeMethod(mightyMethod, attribute);
        }

        protected override void InvokeMethod(MightyMethod mightyMethod, T attribute)
        {
            var methodInfo = mightyMethod.MemberInfo;

            if (!m_methodCache.Contains(mightyMethod)) EnableDrawer(mightyMethod, attribute);

            if (m_methodCache[mightyMethod])
            {
                var label = attribute.Label;
                var buttonText = string.IsNullOrEmpty(label) ? methodInfo.Name.GetPrettyName() : label;

                var enabled = GUI.enabled;

                GUI.enabled = enabled && (attribute.ExecuteInPlayMode || !EditorApplication.isPlaying);

                if (DrawButton(attribute, buttonText, methodInfo.Name))
                {
                    methodInfo.Invoke(mightyMethod.Context.Target, null);
                    OnFunctionHasBeenCalled();
                }

                GUI.enabled = enabled;
            }
            else
                MightyGUIUtilities.DrawHelpBox($"{attribute.GetType().Name} works only on methods with no parameters");
        }

        protected abstract bool DrawButton(T attribute, string label, string methodName);

        protected virtual void OnFunctionHasBeenCalled()
        {
        }
    }
}
#endif