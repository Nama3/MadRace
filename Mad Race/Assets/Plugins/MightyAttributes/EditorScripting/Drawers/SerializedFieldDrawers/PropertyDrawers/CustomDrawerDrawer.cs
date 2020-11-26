#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class CustomDrawerDrawer : BasePropertyDrawer<CustomDrawerAttribute>, IArrayElementDrawer
    {
        #region Signatures

        private readonly CallbackSignature m_fieldCallback = new CallbackSignature(typeof(object), typeof(string), typeof(object));

        private readonly CallbackSignature m_propertyCallback = new CallbackSignature(typeof(void), typeof(MightySerializedField));

        private readonly CallbackSignature m_elementCallback = new CallbackSignature(typeof(void), typeof(MightySerializedField), typeof(int));

        private readonly CallbackSignature m_labeledElementCallback =
            new CallbackSignature(typeof(void), typeof(GUIContent), typeof(MightySerializedField), typeof(int));

        private readonly CallbackSignature m_rectElementCallback =
            new CallbackSignature(typeof(void), typeof(Rect), typeof(MightySerializedField), typeof(int));

        private readonly CallbackSignature m_elementHeightCallback =
            new CallbackSignature(typeof(float), typeof(MightySerializedField), typeof(int));

        #endregion /Signatures

        private readonly MightyCache<CallbackSignature, MightyMethod<object>> m_customDrawerCache =
            new MightyCache<CallbackSignature, MightyMethod<object>>();

        public object DrawField(FieldInfo fieldInfo, Object context, object value, CustomDrawerAttribute attribute)
        {
            if (!GetDrawerForMember(ReferencesUtilities.GetUniqueID(fieldInfo), m_fieldCallback, out var drawerMethod))
                m_customDrawerCache[ReferencesUtilities.GetUniqueID(fieldInfo), m_fieldCallback] =
                    MemberUtilities.GetMightyMethod<object>(context, attribute.DrawerCallback, m_fieldCallback);

            return InvokeDrawer(drawerMethod, $"object {attribute.DrawerCallback}(string label, object value)",
                fieldInfo.Name.GetPrettyName(), value);
        }

        protected override void DrawProperty(MightySerializedField serializedField, SerializedProperty property,
            CustomDrawerAttribute attribute)
        {
            GetDrawerForMember(serializedField, m_propertyCallback, out var drawerMethod, attribute);

            InvokeDrawer(drawerMethod,
                $"void {attribute.DrawerCallback}(MightySerializedField serializedField)",
                serializedField);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var attribute = (CustomDrawerAttribute) baseAttribute;
            GetDrawerForMember(serializedField, m_elementCallback, out var drawerMethod, attribute);

            InvokeDrawer(drawerMethod,
                $"void {attribute.DrawerCallback}(MightySerializedField serializedField, int index)",
                serializedField, index);
        }

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var attribute = (CustomDrawerAttribute) baseAttribute;
            GetDrawerForMember(serializedField, m_labeledElementCallback, out var drawerMethod, attribute);

            InvokeDrawer(drawerMethod,
                $"void {attribute.DrawerCallback}(GUIContent label, MightySerializedField serializedField, int index)",
                label, serializedField, index);
        }

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var attribute = (CustomDrawerAttribute) baseAttribute;
            GetDrawerForMember(serializedField, m_rectElementCallback, out var drawerMethod, attribute);

            InvokeDrawer(position, drawerMethod,
                $"Rect {attribute.DrawerCallback}(Rect position, MightySerializedField serializedField, int index)",
                position, serializedField, index);
        }

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var attribute = (CustomDrawerAttribute) baseAttribute;
            var elementHeightName = attribute.ElementHeightCallback;
            if (string.IsNullOrWhiteSpace(elementHeightName)) return 0;

            if (GetDrawerForMember(serializedField, m_elementHeightCallback, out var callback, attribute))
                return (float) callback.Invoke(serializedField, index);

            MightyGUIUtilities.DrawHelpBox(
                $"Element height is invalid, it should be like this: \"float {elementHeightName}(MightySerializedField serializedField, int index)\"");
            return 0;
        }

        private object InvokeDrawer(MightyMethod<object> drawerMethod, string signature, params object[] parameters)
        {
            if (drawerMethod != null) return drawerMethod.Invoke(parameters);
            MightyGUIUtilities.DrawHelpBox($"The drawer callback is invalid, it should be like this: \"{signature}\"");
            return null;
        }

        private object InvokeDrawer(Rect position, MightyMethod<object> drawerMethod, string signature, params object[] parameters)
        {
            if (drawerMethod != null) return drawerMethod.Invoke(parameters);
            MightyGUIUtilities.DrawHelpBox(position, $"The drawer callback is invalid, it should be like this: \"{signature}\"");
            return null;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, CustomDrawerAttribute attribute)
        {
            var target = attribute.Target;

            if (!serializedField.Property.IsCollection())
                InitCallback(serializedField, target, attribute.DrawerCallback, m_propertyCallback);
            else
            {
                InitCallback(serializedField, target, attribute.DrawerCallback, m_elementCallback);
                InitCallback(serializedField, target, attribute.DrawerCallback, m_labeledElementCallback);
                InitCallback(serializedField, target, attribute.DrawerCallback, m_rectElementCallback);
                InitCallback(serializedField, target, attribute.ElementHeightCallback, m_elementHeightCallback);
            }
        }

        private void InitCallback(BaseMightyMember mightyMember, object target, string callbackName, CallbackSignature signature)
        {
            mightyMember.GetMightyMethod<object>(target, callbackName, signature, out var mightyMethod);
            if (mightyMethod != null) m_customDrawerCache[mightyMember, signature] = mightyMethod;
        }

        private bool GetDrawerForMember(BaseMightyMember member, CallbackSignature signature, out MightyMethod<object> drawerMethod,
            BasePropertyDrawerAttribute attribute)
        {
            if (GetDrawerForMember(member.ID, signature, out drawerMethod)) return true;
            EnableDrawer(member, attribute);
            return GetDrawerForMember(member.ID, signature, out drawerMethod);
        }

        private bool GetDrawerForMember(long id, CallbackSignature signature, out MightyMethod<object> drawerMethod) =>
            m_customDrawerCache.TryGetValue(id, signature, out drawerMethod);

        protected override void ClearCache() => m_customDrawerCache.ClearCache();
    }
}
#endif