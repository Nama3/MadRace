#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class CallbackSignature
    {
        public static readonly CallbackSignature AnyTypeNoParams = new CallbackSignature(null);
        public static readonly CallbackSignature VoidNoParams = new CallbackSignature(typeof(void));

        public Type ReturnType { get; }
        public bool CheckElementType { get; }
        public Type[] ParamTypes { get; private set; }

        public CallbackSignature(Type returnType, params Type[] paramTypes)
        {
            ReturnType = returnType;
            ParamTypes = paramTypes;
        }

        public CallbackSignature(Type returnType, bool checkElementType, params Type[] paramTypes)
        {
            ReturnType = returnType;
            CheckElementType = checkElementType;
            ParamTypes = paramTypes;
        }

        public void SetParamsType(params Type[] paramTypes) => ParamTypes = paramTypes;

        public bool IsFieldValid(FieldInfo callbackInfo)
        {
            if (callbackInfo == null) return false;
            var callbackType = CheckElementType && callbackInfo.FieldType.IsArray
                ? callbackInfo.FieldType.GetElementType()
                : callbackInfo.FieldType;

            return CompareTypes(UnderlyingTypeIfNullable(ReturnType), UnderlyingTypeIfNullable(callbackType));
        }

        public bool IsPropertyValid(PropertyInfo callbackInfo)
        {
            if (callbackInfo == null) return false;
            var callbackType = CheckElementType && callbackInfo.PropertyType.IsArray
                ? callbackInfo.PropertyType.GetElementType()
                : callbackInfo.PropertyType;

            return CompareTypes(UnderlyingTypeIfNullable(ReturnType), UnderlyingTypeIfNullable(callbackType));
        }

        public bool IsMethodValid(MethodInfo callbackInfo)
        {
            if (callbackInfo == null) return false;
            var callbackType = CheckElementType && callbackInfo.ReturnType.IsArray
                ? callbackInfo.ReturnType.GetElementType()
                : callbackInfo.ReturnType;

            if (!CompareTypes(UnderlyingTypeIfNullable(ReturnType), UnderlyingTypeIfNullable(callbackType))) return false;

            var callbackParameters = callbackInfo.GetParameters();
            if ((ParamTypes == null || ParamTypes.Length == 0) && callbackParameters.Length == 0) return true;

            if (ParamTypes == null || ParamTypes.Length != callbackParameters.Length) return false;

            for (var i = 0; i < callbackParameters.Length; i++)
                if (!CompareTypes(ParamTypes[i], callbackParameters[i].ParameterType))
                    return false;

            return true;
        }

        private static Type UnderlyingTypeIfNullable(Type type) =>
            type != null && Nullable.GetUnderlyingType(type) is Type underlyingType ? underlyingType : type;

        private static bool CompareTypes(Type first, Type second)
        {
            if (first == null && second == null) return true;
            
            if (first == null) return false;
            if (second == null) return false;
            
            return second == first || second.IsSubclassOf(first) || first.IsAssignableFrom(second);
        }
    }

    public static class ReflectionUtilities
    {
        public const BindingFlags ANY_MEMBER_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                                     BindingFlags.Public | BindingFlags.DeclaredOnly;

        public static List<Type> GetTypeHierarchy(Type type)
        {
            var types = new List<Type> {type};
            while ((type = type.BaseType) != null) types.Add(type);
            return types;
        }

        public static IEnumerable<FieldInfo> GetAllFields(Type type, Func<FieldInfo, bool> predicate)
        {
            var types = GetTypeHierarchy(type);

            for (var i = types.Count - 1; i >= 0; i--)
                foreach (var fieldInfo in types[i].GetFields(ANY_MEMBER_FLAGS).Where(predicate))
                    yield return fieldInfo;
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(Type type, Func<PropertyInfo, bool> predicate)
        {
            var types = GetTypeHierarchy(type);

            for (var i = types.Count - 1; i >= 0; i--)
                foreach (var propertyInfo in types[i].GetProperties(ANY_MEMBER_FLAGS).Where(predicate))
                    yield return propertyInfo;
        }

        public static IEnumerable<MethodInfo> GetAllMethods(Type type, Func<MethodInfo, bool> predicate)
        {
            var types = GetTypeHierarchy(type);

            for (var i = types.Count - 1; i >= 0; i--)
                foreach (var methodInfo in types[i].GetMethods(ANY_MEMBER_FLAGS).Where(predicate))
                    yield return methodInfo;
        }

        public static IEnumerable<MemberInfo> GetAllMembers(Type type, Func<MemberInfo, bool> predicate)
        {
            var types = GetTypeHierarchy(type);

            for (var i = types.Count - 1; i >= 0; i--)
                foreach (var memberInfo in types[i].GetMembers(ANY_MEMBER_FLAGS).Where(predicate))
                    yield return memberInfo;
        }

        public static IEnumerable<(MemberInfo info, Attribute[] attributes)> GetAllMembersWithMightyAttributes(Type type)
        {
            var types = GetTypeHierarchy(type);
            
            for (var i = types.Count - 1; i >= 0; i--)
                foreach (var member in types[i].GetMembers(ANY_MEMBER_FLAGS))
                {
                    var attributes = member.GetCustomAttributes(typeof(Attribute), true);
                    if (attributes.Length == 0 || attributes.All(a => !(a is BaseMightyAttribute))) continue;
                    yield return (member, (Attribute[]) attributes);
                }
        }

        public static (MemberInfo info, SerializedProperty property)[] GetAllMightyMembers(Type type,
            Func<string, SerializedProperty> findProperty)
        {
            var types = GetTypeHierarchy(type);

            var hasMightyMembers = false;
            var membersList = new List<(MemberInfo info, SerializedProperty property)>();

            for (var i = types.Count - 1; i >= 0; i--)
                foreach (var member in types[i].GetMembers(ANY_MEMBER_FLAGS))
                {
                    var isMighty = member.HasAttributeOfType<BaseMightyAttribute>();
                    if (isMighty) hasMightyMembers = true;

                    if (findProperty.Invoke(member.Name) is SerializedProperty property)
                        membersList.Add((member, property));
                    else if (isMighty)
                        membersList.Add((member, null));
                }

            return hasMightyMembers ? membersList.ToArray() : new (MemberInfo info, SerializedProperty property)[0];
        }

        public static FieldInfo GetField(Type type, string fieldName) =>
            GetAllFields(type, f => f.Name.Equals(fieldName, StringComparison.InvariantCulture)).LastOrDefault();

        public static PropertyInfo GetProperty(Type type, string propertyName) =>
            GetAllProperties(type, p => p.Name.Equals(propertyName, StringComparison.InvariantCulture)).LastOrDefault();

        public static MethodInfo GetMethod(Type type, string methodName) =>
            GetAllMethods(type, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).LastOrDefault();

        public static MemberInfo GetMember(Type type, string methodName) =>
            GetAllMembers(type, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).LastOrDefault();

        public static FieldInfo GetField(this object target, string fieldName) => GetField(target.GetType(), fieldName);

        public static PropertyInfo GetProperty(this object target, string propertyName) => GetProperty(target.GetType(), propertyName);

        public static MethodInfo GetMethod(this object target, string methodName) => GetMethod(target.GetType(), methodName);

        public static MemberInfo GetMember(this object target, string memberName) => GetMember(target.GetType(), memberName);

        public static bool IsCallbackName(this FieldInfo fieldInfo) =>
            fieldInfo != null && fieldInfo.GetCustomAttribute<CallbackNameAttribute>() != null;

        public static bool IsCallbackName(this PropertyInfo propertyInfo) =>
            propertyInfo != null && propertyInfo.GetCustomAttribute<CallbackNameAttribute>() != null;

        public static bool IsCallbackName(this MethodInfo methodInfo) =>
            methodInfo != null && methodInfo.GetCustomAttribute<CallbackNameAttribute>() != null;

        public static bool InfoExist(this Type targetType, string memberName) =>
            GetField(targetType, memberName) != null ||
            GetProperty(targetType, memberName) != null ||
            GetMethod(targetType, memberName) != null;

        public static bool InfoValid(this Type targetType, string memberName, [NotNull] CallbackSignature callbackSignature) =>
            callbackSignature.IsFieldValid(GetField(targetType, memberName)) ||
            callbackSignature.IsPropertyValid(GetProperty(targetType, memberName)) ||
            callbackSignature.IsMethodValid(GetMethod(targetType, memberName));

        public static bool GetMemberInfo(this Type targetType, string memberName, [NotNull] CallbackSignature callbackSignature,
            out MemberInfo memberInfo)
        {
            memberInfo = GetField(targetType, memberName);
            if (callbackSignature.IsFieldValid((FieldInfo) memberInfo)) return true;
            memberInfo = GetMethod(targetType, memberName);
            if (callbackSignature.IsMethodValid((MethodInfo) memberInfo)) return true;
            memberInfo = GetProperty(targetType, memberName);
            return callbackSignature.IsPropertyValid((PropertyInfo) memberInfo);
        }

        public static IEnumerable<FieldInfo> GetSerializableFields(this Type type, bool ignoreHidden = false,
            bool ignoreEditorSerialize = false) => type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.GetCustomAttribute(typeof(NonSerializedAttribute)) == null &&
                        (!ignoreHidden || f.GetCustomAttribute(typeof(HideAttribute)) == null) &&
                        (f.IsPublic || f.GetCustomAttribute(typeof(SerializeField)) != null ||
                         !ignoreEditorSerialize && f.GetCustomAttribute(typeof(EditorSerializeAttribute)) != null));

        public static int GetMemberTypeOrder(this MemberInfo member, bool isSerialized)
        {
            switch (member)
            {
                case FieldInfo _:
                    return isSerialized ? 0 : 1;
                case PropertyInfo _:
                    return 2;
                case MethodInfo _:
                    return 3;
            }

            return int.MaxValue;
        }
    }
}
#endif