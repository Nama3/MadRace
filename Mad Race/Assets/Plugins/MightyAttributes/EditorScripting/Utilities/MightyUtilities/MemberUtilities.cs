#if UNITY_EDITOR
using System;
using System.Collections;
using System.Reflection;
using MightyAttributes.Utilities;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public static class MemberUtilities
    {
        public delegate bool GetValuePredicate<T>(string memberName, out T value);

        #region Target

        public static object GetAttributeTarget<T>(this MemberInfo memberInfo, object context) where T : Attribute
        {
            foreach (var attribute in memberInfo.GetCustomAttributes<BaseWrapperAttribute>(true))
            {
                var wrappingObject = MightyWrapperUtilities.GetWrapperReference<T>(attribute);
                if (wrappingObject != null) return wrappingObject;
            }

            return memberInfo.TryGetAttributeTargetInSerializableClass(typeof(T), context, out var target) ? target : context;
        }

        public static object GetAttributeTarget(this MemberInfo memberInfo, Type attributeType, object context)
        {
            foreach (var attribute in memberInfo.GetCustomAttributes<BaseWrapperAttribute>(true))
            {
                var wrappingObject = MightyWrapperUtilities.GetWrapperReference(attribute, attributeType);
                if (wrappingObject != null) return wrappingObject;
            }

            return memberInfo.TryGetAttributeTargetInSerializableClass(attributeType, context, out var target) ? target : context;
        }

        public static bool TryGetAttributeTargetInSerializableClass(this MemberInfo memberInfo, Type attributeType, object context,
            out object target)
        {
            foreach (var serializableField in context.GetType().GetSerializableFields())
            {
                if (serializableField.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length <= 0) continue;

                var nestedMember = serializableField.GetMember(memberInfo.Name);
                if (nestedMember == null) continue;
                if (nestedMember.GetCustomAttribute(attributeType) == null)
                    return serializableField.TryGetAttributeTargetInSerializableClass(attributeType, serializableField.GetValue(context),
                        out target);

                target = serializableField.GetValue(context);
                return true;
            }

            target = null;
            return false;
        }

        #endregion /Target

        #region Callback

        public static bool GetCallbackName(object target, string memberName, out string callbackName)
        {
            var fieldInfo = ReflectionUtilities.GetField(target.GetType(), memberName);
            if (fieldInfo.IsCallbackName())
            {
                callbackName = (string) fieldInfo.GetValue(target);
                return true;
            }

            var propertyInfo = ReflectionUtilities.GetProperty(target.GetType(), memberName);
            if (propertyInfo.IsCallbackName())
            {
                callbackName = (string) propertyInfo.GetValue(target);
                return true;
            }

            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (methodInfo.IsCallbackName())
            {
                callbackName = (string) methodInfo.Invoke(target, null);
                return true;
            }

            callbackName = null;
            return false;
        }

        public static object GetCallbackTarget(BaseMightyMember mightyMember, object target, string callbackName,
            CallbackSignature callbackSignature, out MemberInfo outMember)
        {
            outMember = null;
            if (string.IsNullOrWhiteSpace(callbackName)) return null;
            if (target.GetType().GetMemberInfo(callbackName, callbackSignature, out outMember)) return target;

            if (target is BaseWrapperAttribute wrapper && GetCallbackName(target, callbackName, out var callbackNameValue))
                return GetCallbackTarget(mightyMember, mightyMember.GetWrapperTarget(wrapper), callbackNameValue,
                    callbackSignature, out outMember);

            return null;
        }

        #endregion /Callback

        #region Method

        public static MightyMethod<T> GetMightyMethod<T>(Object context, string memberName, CallbackSignature callbackSignature) =>
            string.IsNullOrWhiteSpace(memberName) ? null :
            InternalGetMightyMethod<T>(context, memberName, callbackSignature, out var method) ? method : null;

        public static bool GetMightyMethod<T>(this BaseMightyMember mightyMember, object target, string memberName,
            CallbackSignature callbackSignature, out MightyMethod<T> method)
        {
            if (string.IsNullOrWhiteSpace(memberName))
            {
                method = null;
                return false;
            }

            if (target is BaseWrapperAttribute wrapper && GetCallbackName(target, memberName, out var callbackName))
                return mightyMember.GetMightyMethod(mightyMember.GetWrapperTarget(wrapper), callbackName, callbackSignature, out method);

            if (InternalGetMightyMethod(target, memberName, callbackSignature, out method))
                return true;

            if (!(mightyMember is MightySerializedField serializedField)) return false;

            target = serializedField.Property.GetPropertyTargetReference();
            return target.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 &&
                   InternalGetMightyMethod(target, memberName, callbackSignature, out method);
        }

        public static bool GetMightyVoidMethod(this BaseMightyMember mightyMember, object target, string memberName,
            CallbackSignature callbackSignature, out MightyVoidMethod method)
        {
            if (string.IsNullOrWhiteSpace(memberName))
            {
                method = null;
                return false;
            }

            if (target is BaseWrapperAttribute wrapper && GetCallbackName(target, memberName, out var callbackName))
                return mightyMember.GetMightyVoidMethod(mightyMember.GetWrapperTarget(wrapper), callbackName, callbackSignature,
                    out method);

            if (InternalGetMightyVoidMethod(target, memberName, callbackSignature, out method))
                return true;

            if (!(mightyMember is MightySerializedField serializedField)) return false;

            target = serializedField.Property.GetPropertyTargetReference();
            return target.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 &&
                   InternalGetMightyVoidMethod(target, memberName, callbackSignature, out method);
        }

        private static bool InternalGetMightyMethod<T>(object target, string memberName, CallbackSignature callbackSignature,
            out MightyMethod<T> method)
        {
            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (callbackSignature.IsMethodValid(methodInfo))
            {
                method = new MightyMethod<T>(target, methodInfo);
                return true;
            }

            method = null;
            return false;
        }

        private static bool InternalGetMightyVoidMethod(object target, string memberName, CallbackSignature callbackSignature,
            out MightyVoidMethod method)
        {
            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (callbackSignature.IsMethodValid(methodInfo))
            {
                method = new MightyVoidMethod(target, methodInfo);
                return true;
            }

            method = null;
            return false;
        }

        #endregion /Method

        #region Get Value or Info

        public static bool GetBoolValue(this BaseMightyMember mightyMember, object target, string memberName, out bool value) =>
            mightyMember.GetValueFromMember(target, memberName, out value, bool.TryParse);

        public static bool GetBoolInfo(this BaseMightyMember mightyMember, object target, string memberName, out MightyInfo<bool> info)
            => mightyMember.GetInfoFromMember(target, memberName, out info,
                (string name, out bool value) => bool.TryParse(name, out value));


        public static bool GetValueFromMember<T>(this BaseMightyMember mightyMember, object target, string memberName, out T value,
            GetValuePredicate<T> predicate = null)
        {
            if (string.IsNullOrWhiteSpace(memberName))
            {
                value = default;
                return false;
            }

            if (predicate != null && predicate(memberName, out value)) return true;

            if (target is BaseWrapperAttribute wrapper && GetCallbackName(target, memberName, out var callbackName))
                return mightyMember.GetValueFromMember(mightyMember.GetWrapperTarget(wrapper), callbackName, out value, predicate);

            if (InternalGetValueFromMember(target, memberName, true, out value))
                return true;

            if (!(mightyMember is MightySerializedField serializedField)) return false;

            target = serializedField.Property.GetPropertyTargetReference();
            if (target.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 &&
                InternalGetValueFromMember(target, memberName, true, out value)) return true;

            return TypeUtilities.TryConvertStringToType(memberName, out value);
        }

        public static bool GetInfoFromMember<T>(this BaseMightyMember mightyMember, object target, string memberName,
            out MightyInfo<T> info, GetValuePredicate<T> predicate = null, bool neverInWrapper = false)
        {
            if (string.IsNullOrWhiteSpace(memberName))
            {
                info = null;
                return false;
            }

            if (predicate != null && predicate(memberName, out var outValue))
            {
                info = new MightyInfo<T>(null, null, outValue);
                return true;
            }

            if (target is BaseWrapperAttribute wrapper)
            {
                if (neverInWrapper)
                    return mightyMember.GetInfoFromMember(mightyMember.GetWrapperTarget(wrapper), memberName, out info, predicate);
                if (GetCallbackName(target, memberName, out var callbackName))
                    return mightyMember.GetInfoFromMember(mightyMember.GetWrapperTarget(wrapper), callbackName, out info, predicate);
            }

            if (InternalGetInfoFromMember(target, memberName, true, out info))
                return true;

            if (!(mightyMember is MightySerializedField serializedField)) return false;

            target = serializedField.Property.GetPropertyTargetReference();
            if (target.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 &&
                InternalGetInfoFromMember(target, memberName, true, out info)) return true;


            if (!TypeUtilities.TryConvertStringToType(memberName, out T value)) return false;
            
            info = new MightyInfo<T>(target, null, value);
            return true;

        }

        internal static bool InternalGetValueFromMember<T>(object target, string memberName, bool checkElementType, out T value)
        {
            var callbackSignature = new CallbackSignature(typeof(T), checkElementType);

            var fieldInfo = ReflectionUtilities.GetField(target.GetType(), memberName);
            if (callbackSignature.IsFieldValid(fieldInfo))
            {
                value = (T) fieldInfo.GetValue(target);
                return true;
            }

            var propertyInfo = ReflectionUtilities.GetProperty(target.GetType(), memberName);
            if (callbackSignature.IsPropertyValid(propertyInfo))
            {
                value = (T) propertyInfo.GetValue(target, null);
                return true;
            }

            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (callbackSignature.IsMethodValid(methodInfo))
            {
                value = (T) methodInfo.Invoke(target, null);
                return true;
            }

            value = default;
            return false;
        }

        internal static bool InternalGetInfoFromMember<T>(object target, string memberName, bool checkElementType, out MightyInfo<T> info)
        {
            var callbackSignature = new CallbackSignature(typeof(T), checkElementType);

            var fieldInfo = ReflectionUtilities.GetField(target.GetType(), memberName);
            if (callbackSignature.IsFieldValid(fieldInfo))
            {
                info = new MightyInfo<T>(target, fieldInfo, (T) fieldInfo.GetValue(target));
                return true;
            }

            var propertyInfo = ReflectionUtilities.GetProperty(target.GetType(), memberName);
            if (callbackSignature.IsPropertyValid(propertyInfo))
            {
                info = new MightyInfo<T>(target, propertyInfo, (T) propertyInfo.GetValue(target));
                return true;
            }

            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (callbackSignature.IsMethodValid(methodInfo))
            {
                info = new MightyInfo<T>(target, methodInfo, (T) methodInfo.Invoke(target, null));
                return true;
            }

            info = null;
            return false;
        }

        #endregion /Get Value or Info

        #region Get Member Array

        public static bool GetArrayValueFromMember(this BaseMightyMember mightyMember, object target, string memberName,
            out object[] outValue)
        {
            if (string.IsNullOrEmpty(memberName))
            {
                outValue = null;
                return false;
            }

            if (target is BaseWrapperAttribute wrapper && GetCallbackName(target, memberName, out var callbackName))
                return GetArrayValueFromMember(mightyMember, mightyMember.GetWrapperTarget(wrapper),
                    callbackName, out outValue);
            if (InternalGetArrayValueFromMember(target, memberName, out outValue))
                return true;

            if (!(mightyMember is MightySerializedField serializedField)) return false;

            target = serializedField.Property.GetPropertyTargetReference();
            return target.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 &&
                   InternalGetArrayValueFromMember(target, memberName, out outValue);
        }

        public static bool GetArrayInfoFromMember(this BaseMightyMember mightyMember, object target, string memberName,
            out MightyInfo<object[]> mightyInfo)
        {
            if (string.IsNullOrEmpty(memberName))
            {
                mightyInfo = null;
                return false;
            }

            if (target is BaseWrapperAttribute wrapper && GetCallbackName(target, memberName, out var callbackName))
                return GetArrayInfoFromMember(mightyMember, mightyMember.GetWrapperTarget(wrapper),
                    callbackName, out mightyInfo);
            if (InternalGetArrayInfoFromMember(target, memberName, out mightyInfo))
                return true;

            if (!(mightyMember is MightySerializedField serializedField)) return false;

            target = serializedField.Property.GetPropertyTargetReference();
            return target.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0 &&
                   InternalGetArrayInfoFromMember(target, memberName, out mightyInfo);
        }

        internal static bool InternalGetArrayValueFromMember(object target, string memberName, out object[] outValue)
        {
            var fieldInfo = ReflectionUtilities.GetField(target.GetType(), memberName);
            if (fieldInfo != null)
            {
                if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
                {
                    var list = (IList) fieldInfo.GetValue(target);
                    outValue = new object[list.Count];
                    for (var i = 0; i < list.Count; i++)
                        outValue[i] = list[i];
                    return true;
                }

                outValue = null;
                return false;
            }

            var propertyInfo = ReflectionUtilities.GetProperty(target.GetType(), memberName);
            if (propertyInfo != null)
            {
                if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var list = (IList) propertyInfo.GetValue(target);
                    outValue = new object[list.Count];
                    for (var i = 0; i < list.Count; i++)
                        outValue[i] = list[i];
                    return true;
                }

                outValue = null;
                return false;
            }

            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (methodInfo != null)
            {
                if (typeof(IList).IsAssignableFrom(methodInfo.ReturnType))
                {
                    var list = (IList) methodInfo.Invoke(target, null);
                    outValue = new object[list.Count];
                    for (var i = 0; i < list.Count; i++)
                        outValue[i] = list[i];
                    return true;
                }

                outValue = null;
                return false;
            }

            outValue = null;
            return false;
        }

        internal static bool InternalGetArrayInfoFromMember(object target, string memberName, out MightyInfo<object[]> mightyInfo)
        {
            var fieldInfo = ReflectionUtilities.GetField(target.GetType(), memberName);
            if (fieldInfo != null)
            {
                if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
                {
                    var list = (IList) fieldInfo.GetValue(target);
                    mightyInfo = new MightyInfo<object[]>(target, fieldInfo, new object[list.Count]);

                    for (var i = 0; i < list.Count; i++)
                        mightyInfo.Value[i] = list[i];
                    return true;
                }

                mightyInfo = null;
                return false;
            }

            var propertyInfo = ReflectionUtilities.GetProperty(target.GetType(), memberName);
            if (propertyInfo != null)
            {
                if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var list = (IList) propertyInfo.GetValue(target);
                    mightyInfo = new MightyInfo<object[]>(target, propertyInfo, new object[list.Count]);

                    for (var i = 0; i < list.Count; i++)
                        mightyInfo.Value[i] = list[i];
                    return true;
                }

                mightyInfo = null;
                return false;
            }

            var methodInfo = ReflectionUtilities.GetMethod(target.GetType(), memberName);
            if (methodInfo != null)
            {
                if (typeof(IList).IsAssignableFrom(methodInfo.ReturnType))
                {
                    var list = (IList) methodInfo.Invoke(target, null);
                    mightyInfo = new MightyInfo<object[]>(target, methodInfo, new object[list.Count]);

                    for (var i = 0; i < list.Count; i++)
                        mightyInfo.Value[i] = list[i];
                    return true;
                }

                mightyInfo = null;
                return false;
            }

            mightyInfo = null;
            return false;
        }

        #endregion /Get Member Array
    }
}
#endif