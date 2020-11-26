#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public static class PropertyUtilities
    {
        private const string ARRAY_PROPERTY_REGEX = @"^data\[\d+\]$|^Array$";
        private const string ARRAY_ITEM_REGEX = @"^data\[(\d+)\]$";

        public static bool ArrayPropertyPathItem(string item) => Regex.IsMatch(item, ARRAY_PROPERTY_REGEX);

        public static bool GetPropertyArrayIndex(string item, out int index)
        {
            index = 0;
            var match = Regex.Match(item, ARRAY_ITEM_REGEX);
            if (!match.Success || match.Groups.Count != 2) return false;
            index = Convert.ToInt32(match.Groups[1].Value);
            return true;
        }

        public static T GetAttribute<T>(this SerializedProperty property) where T : Attribute =>
            GetAttribute(property, typeof(T)) as T;

        public static T[] GetAttributes<T>(this SerializedProperty property) where T : Attribute =>
            GetAttributes(property, typeof(T)).Cast<T>().ToArray();

        public static object GetAttribute(this SerializedProperty property, Type attributeType)
        {
            object[] attributes = GetAttributes(property, attributeType);
            return attributes != null && attributes.Length > 0 ? attributes[0] : null;
        }

        public static object[] GetAttributes(this SerializedProperty property, Type attributeType)
        {
            var fieldInfo = ReflectionUtilities.GetField(property.GetSerializableClassProperty(out var name)?.GetSystemType() ??
                                                         property.GetTargetObject().GetType(), name);

            if (fieldInfo == null) return new object[0];

            var attributesList = new List<object>();
            var attributes = fieldInfo.GetCustomAttributes(attributeType, true);
            if (attributes.Length > 0) attributesList.AddRange(attributes);

            attributesList.AddRange(fieldInfo.GetWrappedAttributes(attributeType));

            return attributesList.ToArray();
        }

        public static bool IsCollection(this SerializedProperty property) =>
            property != null && property.propertyType != SerializedPropertyType.String && property.isArray;

        public static bool IsSerializableClassOrStruct(this SerializedProperty property) =>
            property.GetSystemType().IsSerializableClassOrStruct();

        public static bool IsPropertyFromSerializableClass(this SerializedProperty property, out string className, out string propertyName)
        {
            var splitPath = property.propertyPath.Split('.').ToList();
            while (ArrayPropertyPathItem(splitPath.Last())) splitPath.RemoveAt(splitPath.Count - 1);

            propertyName = splitPath.Last();

            if (splitPath.Count <= 1)
            {
                className = null;
                return false;
            }

            splitPath.RemoveAt(splitPath.Count - 1);
            while (ArrayPropertyPathItem(splitPath.Last())) splitPath.RemoveAt(splitPath.Count - 1);
            className = string.Join(".", splitPath);
            return true;
        }

        public static SerializedProperty GetSerializableClassProperty(this SerializedProperty property, out string propertyName) =>
            IsPropertyFromSerializableClass(property, out var className, out propertyName)
                ? property.serializedObject.FindProperty(className)
                : null;

        public static Object GetTargetObject(this SerializedProperty property) => property.serializedObject.targetObject;

        public static object GetAttributeTarget<T>(this SerializedProperty property) where T : Attribute
        {
            foreach (var attribute in property.GetAttributes<BaseWrapperAttribute>())
            {
                var wrappingObject = MightyWrapperUtilities.GetWrapperReference<T>(attribute);
                if (wrappingObject != null) return wrappingObject;
            }

            return property.GetPropertyTargetReference();
        }

        public static object GetAttributeTarget(this SerializedProperty property, Type attributeType)
        {
            foreach (var attribute in property.GetAttributes<BaseWrapperAttribute>())
            {
                var wrappingObject = MightyWrapperUtilities.GetWrapperReference(attribute, attributeType);
                if (wrappingObject != null) return wrappingObject;
            }

            return property.GetPropertyTargetReference();
        }

        public static object GetSerializableClassReference(this SerializedProperty property)
        {
            var targetObject = property.GetPropertyTargetReference();
            var targetType = targetObject.GetType();

            var path = property.propertyPath;
            var splitPath = path.Split('.').ToList();
            if (!ArrayPropertyPathItem(splitPath.Last()))
            {
                var propertyField = ReflectionUtilities.GetField(targetType, splitPath.Last());
                return propertyField.GetValue(targetObject);
            }
            else
            {
                var propertyField = ReflectionUtilities.GetField(targetType, splitPath[splitPath.Count - 3]);
                targetObject = propertyField.GetValue(targetObject);
                GetPropertyArrayIndex(splitPath.Last(), out var index);
                return ((IList) targetObject)[index];
            }
        }

        public static object GetPropertyTargetReference(this SerializedProperty property)
        {
            object targetObject = property.GetTargetObject();
            var targetType = targetObject.GetType();
            while (targetType != null)
            {
                var path = property.propertyPath;
                var splitPath = path.Split('.').ToList();
                while (ArrayPropertyPathItem(splitPath.Last())) splitPath.RemoveAt(splitPath.Count - 1);
                path = string.Join(".", splitPath);

                var fieldInfo = ReflectionUtilities.GetField(targetType, path);

                if (fieldInfo != null) return targetObject;
                if (splitPath.Count == 1)
                {
                    targetType = targetType.BaseType;
                    continue;
                }

                for (var i = 0; i < splitPath.Count; i++)
                {
                    var typeField = ReflectionUtilities.GetField(targetType, splitPath[i]);
                    if (typeField == null) break;
                    targetType = typeField.FieldType;
                    targetObject = typeField.GetValue(targetObject);
                    var newPath = new List<string>(splitPath);

                    if (typeof(IList).IsAssignableFrom(targetType))
                    {
                        if (targetType.IsArray)
                            targetType = targetType.GetElementType();
                        else if (targetType.IsGenericType)
                            targetType = targetType.GetGenericArguments()[0];

                        if (!GetPropertyArrayIndex(newPath[i + 2], out var index)) continue;

                        targetObject = ((IList) targetObject)[index];
                        if (newPath.Count > i + 3) i += 2;
                    }

                    newPath.RemoveRange(0, i + 1);
                    path = string.Join(".", newPath);

                    fieldInfo = ReflectionUtilities.GetField(targetType, path);
                    if (fieldInfo != null) return targetObject;
                }

                break;
            }

            return targetObject;
        }

        public static SerializedPropertyType GetPropertyType(this SerializedProperty property)
        {
            if (!property.IsCollection()) return property.propertyType;

            var type = property.GetSystemType();

            if (type == typeof(bool)) return SerializedPropertyType.Boolean;
            if (type.IsEnum) return SerializedPropertyType.Enum;
            if (type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) ||
                type == typeof(long) || type == typeof(ulong))
                return SerializedPropertyType.Integer;
            if (type == typeof(float) || type == typeof(double)) return SerializedPropertyType.Float;

            if (type == typeof(char)) return SerializedPropertyType.Character;
            if (type == typeof(string)) return SerializedPropertyType.String;

            if (type == typeof(Vector2)) return SerializedPropertyType.Vector2;
            if (type == typeof(Vector3)) return SerializedPropertyType.Vector3;
            if (type == typeof(Vector4)) return SerializedPropertyType.Vector4;

            if (type == typeof(Rect)) return SerializedPropertyType.Rect;
            if (type == typeof(Bounds)) return SerializedPropertyType.Bounds;

            if (type == typeof(Vector2Int)) return SerializedPropertyType.Vector2Int;
            if (type == typeof(Vector3Int)) return SerializedPropertyType.Vector3Int;
            if (type == typeof(RectInt)) return SerializedPropertyType.RectInt;
            if (type == typeof(BoundsInt)) return SerializedPropertyType.BoundsInt;

            if (type == typeof(Quaternion)) return SerializedPropertyType.Quaternion;
            if (type == typeof(LayerMask)) return SerializedPropertyType.LayerMask;
            if (type == typeof(Color)) return SerializedPropertyType.Color;

            if (type == typeof(Gradient)) return SerializedPropertyType.Gradient;
            if (type == typeof(AnimationCurve)) return SerializedPropertyType.AnimationCurve;

            return typeof(Object).IsAssignableFrom(type) ? SerializedPropertyType.ObjectReference : SerializedPropertyType.Generic;
        }

        public static Type GetSystemType(this SerializedProperty property)
        {
            var targetType = property.GetTargetObject().GetType();
            FieldInfo fieldInfo = null;
            while (targetType != null)
            {
                var path = property.propertyPath;
                var splitPath = path.Split('.').ToList();
                while (ArrayPropertyPathItem(splitPath.Last())) splitPath.RemoveAt(splitPath.Count - 1);
                path = string.Join(".", splitPath);

                fieldInfo = ReflectionUtilities.GetField(targetType, path);

                if (fieldInfo != null) break;
                if (splitPath.Count == 1)
                {
                    targetType = targetType.BaseType;
                    continue;
                }

                for (var i = 0; i < splitPath.Count; i++)
                {
                    var typeField = ReflectionUtilities.GetField(targetType, splitPath[i]);
                    if (typeField == null) break;
                    targetType = typeField.FieldType;
                    var newPath = new List<string>(splitPath);

                    if (typeof(IList).IsAssignableFrom(targetType))
                    {
                        if (targetType.IsArray)
                            targetType = targetType.GetElementType();
                        else if (targetType.IsGenericType)
                            targetType = targetType.GetGenericArguments()[0];

                        if (newPath.Count > i + 3) i += 2;
                        else return targetType;
                    }

                    newPath.RemoveRange(0, i + 1);
                    path = string.Join(".", newPath);

                    fieldInfo = ReflectionUtilities.GetField(targetType, path);
                    if (fieldInfo != null) break;
                }

                break;
            }

            if (fieldInfo == null) return null;

            if (fieldInfo.FieldType.IsArray)
                return fieldInfo.FieldType.GetElementType();
            if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType) && fieldInfo.FieldType.IsGenericType)
                return fieldInfo.FieldType.GetGenericArguments()[0];
            return fieldInfo.FieldType;
        }

        public static GameObject GetGameObject(this SerializedProperty property) =>
            property.GetTargetObject() is MonoBehaviour monoBehaviour ? monoBehaviour.gameObject : null;


        #region Get Generic

        public static object GetGenericValue(object target, SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    var propertyField = target.GetType().GetField(property.name);
                    return propertyField.GetValue(target);

                case SerializedPropertyType.Enum:
                    return property.GetSystemType().GetCustomAttribute<FlagsAttribute>() != null
                        ? property.intValue
                        : property.enumValueIndex;

                case SerializedPropertyType.Integer:
                    return property.intValue;

                case SerializedPropertyType.Boolean:
                    return property.boolValue;

                case SerializedPropertyType.Float:
                    return property.floatValue;

                case SerializedPropertyType.String:
                    return property.stringValue;

                case SerializedPropertyType.Color:
                    return property.colorValue;

                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue;

                case SerializedPropertyType.LayerMask:
                    return property.intValue;

                case SerializedPropertyType.Vector2:
                    return property.vector2Value;

                case SerializedPropertyType.Vector3:
                    return property.vector3Value;

                case SerializedPropertyType.Vector4:
                    return property.vector4Value;

                case SerializedPropertyType.Rect:
                    return property.rectValue;

                case SerializedPropertyType.AnimationCurve:
                    return property.animationCurveValue;

                case SerializedPropertyType.Bounds:
                    return property.boundsValue;

                case SerializedPropertyType.Quaternion:
                    return property.quaternionValue;

                case SerializedPropertyType.Vector2Int:
                    return property.vector2IntValue;

                case SerializedPropertyType.Vector3Int:
                    return property.vector3IntValue;

                case SerializedPropertyType.RectInt:
                    return property.rectIntValue;

                case SerializedPropertyType.BoundsInt:
                    return property.boundsIntValue;

                default:
                    return null;
            }
        }

        #endregion

        #region Set Generic

        public static bool SetGenericValue(object target, SerializedProperty property, string valueName, SerializedPropertyType type)
        {
            switch (type)
            {
                case SerializedPropertyType.Generic:
                    var propertyType = property.GetSystemType();
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out object objectValue)) return false;

                    var propertyField = target.GetType().GetField(property.name);
                    var genericTarget = propertyField.GetValue(target);

                    var fields = propertyType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        if (field.IsPrivate && field.GetCustomAttributes(typeof(SerializeField), false).Length <= 0) continue;
                        var instanceField = objectValue.GetType().GetField(field.Name,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (instanceField == null) continue;
                        var value = instanceField.GetValue(objectValue);

                        field.SetValue(genericTarget, value);
                    }

                    if (!propertyType.IsClass)
                        propertyField.SetValue(target, genericTarget);

                    return true;

                case SerializedPropertyType.Enum:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Enum enumValue)) return false;

                    property.intValue = Convert.ToInt32(enumValue);
                    return true;
                case SerializedPropertyType.Integer:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out int intValue)) return false;

                    property.intValue = intValue;
                    return true;

                case SerializedPropertyType.Boolean:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out bool boolValue)) return false;

                    property.boolValue = boolValue;
                    return true;

                case SerializedPropertyType.Float:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out float floatValue)) return false;

                    property.floatValue = floatValue;
                    return true;

                case SerializedPropertyType.String:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out string stringValue)) return false;

                    property.stringValue = stringValue;
                    return true;

                case SerializedPropertyType.Color:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Color colorValue)) return false;

                    property.colorValue = colorValue;
                    return true;

                case SerializedPropertyType.ObjectReference:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Object unityObjectValue)) return false;

                    property.objectReferenceValue = unityObjectValue;
                    return true;

                case SerializedPropertyType.LayerMask:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out LayerMask layerMaskValue)) return false;

                    property.intValue = layerMaskValue;
                    return true;

                case SerializedPropertyType.Vector2:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Vector2 vector2Value)) return false;

                    property.vector2Value = vector2Value;
                    return true;

                case SerializedPropertyType.Vector3:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Vector3 vector3Value)) return false;

                    property.vector3Value = vector3Value;
                    return true;

                case SerializedPropertyType.Vector4:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Vector4 vector4Value)) return false;

                    property.vector4Value = vector4Value;
                    return true;

                case SerializedPropertyType.Rect:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Rect rectValue)) return false;

                    property.rectValue = rectValue;
                    return true;

                case SerializedPropertyType.AnimationCurve:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out AnimationCurve animationCurveValue))
                        return false;

                    property.animationCurveValue = animationCurveValue;
                    return true;

                case SerializedPropertyType.Bounds:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Bounds boundsValue)) return false;

                    property.boundsValue = boundsValue;
                    return true;

                case SerializedPropertyType.Quaternion:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Quaternion quaternionValue)) return false;

                    property.quaternionValue = quaternionValue;
                    return true;

                case SerializedPropertyType.Vector2Int:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Vector2Int vector2IntValue)) return false;

                    property.vector2IntValue = vector2IntValue;
                    return true;

                case SerializedPropertyType.Vector3Int:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out Vector3Int vector3IntValue)) return false;

                    property.vector3IntValue = vector3IntValue;
                    return true;

                case SerializedPropertyType.RectInt:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out RectInt rectIntValue)) return false;

                    property.rectIntValue = rectIntValue;
                    return true;

                case SerializedPropertyType.BoundsInt:
                    if (!MemberUtilities.InternalGetValueFromMember(target, valueName, true, out BoundsInt boundsIntValue)) return false;

                    property.boundsIntValue = boundsIntValue;
                    return true;

                default:
                    return false;
            }
        }

        public static bool SetArrayElementGenericValue(object target, SerializedProperty element, string valueName,
            SerializedPropertyType type, int index)
        {
            switch (type)
            {
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Integer:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var intValue)) return false;

                    element.intValue = Convert.ToInt32(intValue[index]);
                    return true;

                case SerializedPropertyType.Boolean:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var boolValue)) return false;

                    element.boolValue = bool.Parse(boolValue[index].ToString());
                    return true;

                case SerializedPropertyType.Float:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var floatValue)) return false;

                    element.floatValue = float.Parse(floatValue[index].ToString());
                    return true;

                case SerializedPropertyType.String:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var stringValue)) return false;

                    element.stringValue = (string) stringValue[index];
                    return true;

                case SerializedPropertyType.Color:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var colorValue)) return false;

                    element.colorValue = (Color) colorValue[index];
                    return true;

                case SerializedPropertyType.ObjectReference:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var unityObjectValue)) return false;

                    element.objectReferenceValue = (Object) unityObjectValue[index];
                    return true;

                case SerializedPropertyType.LayerMask:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var layerMaskValue)) return false;

                    element.intValue = (LayerMask) layerMaskValue[index];
                    return true;

                case SerializedPropertyType.Vector2:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var vector2Value)) return false;

                    element.vector2Value = (Vector2) vector2Value[index];
                    return true;

                case SerializedPropertyType.Vector3:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var vector3Value)) return false;

                    element.vector3Value = (Vector3) vector3Value[index];
                    return true;

                case SerializedPropertyType.Vector4:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var vector4Value)) return false;

                    element.vector4Value = (Vector4) vector4Value[index];
                    return true;

                case SerializedPropertyType.Rect:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var rectValue)) return false;

                    element.rectValue = (Rect) rectValue[index];
                    return true;

                case SerializedPropertyType.AnimationCurve:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var animationCurveValue)) return false;

                    element.animationCurveValue = (AnimationCurve) animationCurveValue[index];
                    return true;

                case SerializedPropertyType.Bounds:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var boundsValue)) return false;

                    element.boundsValue = (Bounds) boundsValue[index];
                    return true;

                case SerializedPropertyType.Quaternion:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var quaternionValue)) return false;

                    element.quaternionValue = (Quaternion) quaternionValue[index];
                    return true;

                case SerializedPropertyType.Vector2Int:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var vector2IntValue)) return false;

                    element.vector2IntValue = (Vector2Int) vector2IntValue[index];
                    return true;

                case SerializedPropertyType.Vector3Int:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var vector3IntValue)) return false;

                    element.vector3IntValue = (Vector3Int) vector3IntValue[index];
                    return true;

                case SerializedPropertyType.RectInt:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var rectIntValue)) return false;

                    element.rectIntValue = (RectInt) rectIntValue[index];
                    return true;

                case SerializedPropertyType.BoundsInt:
                    if (!MemberUtilities.InternalGetArrayValueFromMember(target, valueName, out var boundsIntValue)) return false;

                    element.boundsIntValue = (BoundsInt) boundsIntValue[index];
                    return true;

                default:
                    return false;
            }
        }

        public static object[] SetArrayGenericValue(SerializedProperty property, SerializedPropertyType type, object target)
        {
            var value = new object[property.arraySize];
            for (var i = 0; i < property.arraySize; i++)
                switch (type)
                {
                    case SerializedPropertyType.Generic:
                        var propertyField = target.GetField(property.name);
                        var fieldValue = propertyField.GetValue(target);
                        var arrayValue = (IList) fieldValue;
                        value[i] = arrayValue[i];
                        break;

                    case SerializedPropertyType.Enum:
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.LayerMask:
                        value[i] = property.GetArrayElementAtIndex(i).intValue;
                        break;

                    case SerializedPropertyType.Boolean:
                        value[i] = property.GetArrayElementAtIndex(i).boolValue;
                        break;

                    case SerializedPropertyType.Float:
                        value[i] = property.GetArrayElementAtIndex(i).floatValue;
                        break;

                    case SerializedPropertyType.String:
                        value[i] = property.GetArrayElementAtIndex(i).stringValue;
                        break;

                    case SerializedPropertyType.Color:
                        value[i] = property.GetArrayElementAtIndex(i).colorValue;
                        break;

                    case SerializedPropertyType.ObjectReference:
                        value[i] = property.GetArrayElementAtIndex(i).objectReferenceValue;
                        break;

                    case SerializedPropertyType.Vector2:
                        value[i] = property.GetArrayElementAtIndex(i).vector2Value;
                        break;

                    case SerializedPropertyType.Vector3:
                        value[i] = property.GetArrayElementAtIndex(i).vector3Value;
                        break;

                    case SerializedPropertyType.Vector4:
                        value[i] = property.GetArrayElementAtIndex(i).vector4Value;
                        break;

                    case SerializedPropertyType.Rect:
                        value[i] = property.GetArrayElementAtIndex(i).rectValue;
                        break;

                    case SerializedPropertyType.AnimationCurve:
                        value[i] = property.GetArrayElementAtIndex(i).animationCurveValue;
                        break;

                    case SerializedPropertyType.Bounds:
                        value[i] = property.GetArrayElementAtIndex(i).boundsValue;
                        break;

                    case SerializedPropertyType.Quaternion:
                        value[i] = property.GetArrayElementAtIndex(i).quaternionValue;
                        break;

                    case SerializedPropertyType.Vector2Int:
                        value[i] = property.GetArrayElementAtIndex(i).vector2IntValue;
                        break;

                    case SerializedPropertyType.Vector3Int:
                        value[i] = property.GetArrayElementAtIndex(i).vector3IntValue;
                        break;

                    case SerializedPropertyType.RectInt:
                        value[i] = property.GetArrayElementAtIndex(i).rectIntValue;
                        break;

                    case SerializedPropertyType.BoundsInt:
                        value[i] = property.GetArrayElementAtIndex(i).boundsIntValue;
                        break;

                    default:
                        return null;
                }

            return value;
        }

        #endregion /Set Generic
    }
}
#endif