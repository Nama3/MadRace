#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ValueFromDrawer : BaseAutoValueDrawer<ValueFromAttribute>
    {
        protected override InitState InitPropertyImpl(MightySerializedField serializedField, ValueFromAttribute attribute)
        {
            var property = serializedField.Property;
            var target = attribute.Target;
            var propertyTarget = property.GetPropertyTargetReference();

            if (!target.GetType().InfoExist(attribute.ValueCallback))
                return new InitState(false, "Callback name: \"" + attribute.ValueCallback + "\" is invalid");

            if (!target.GetType()
                .GetMemberInfo(attribute.ValueCallback, new CallbackSignature(property.GetSystemType(), true), out _))
                return new InitState(false, "\"" + attribute.ValueCallback + "\" type is invalid");

            if (!property.IsCollection())
                return PropertyUtilities.SetGenericValue(propertyTarget, property, attribute.ValueCallback, property.propertyType)
                    ? new InitState(true)
                    : new InitState(false, "\"" + property.displayName + "\" type is not serializable");

            var state = new InitState(true);
            var index = 0;

            if (serializedField.GetArrayValueFromMember(target, attribute.ValueCallback, out var outArray) &&
                property.CompareArrays(outArray, propertyTarget)) return state;

            if (property.arraySize == 0)
                property.InsertArrayElementAtIndex(0);

            if (property.GetArrayElementAtIndex(0).propertyType == SerializedPropertyType.Generic)
            {
                try
                {
                    var propertyType = property.GetSystemType();
                    var propertyField = propertyTarget.GetField(property.name);
                    var array = (IList) Array.CreateInstance(propertyType, outArray.Length);

                    for (var i = 0; i < outArray.Length; i++)
                        array[i] = outArray[i];

                    propertyField.SetValue(propertyTarget, array);
                    return new InitState(true);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    property.DeleteArrayElementAtIndex(index);
                    return new InitState(false, "\"" + property.displayName + "\" type is not serializable");
                }
            }

            property.ClearArray();
            while (index < outArray.Length)
            {
                try
                {
                    property.InsertArrayElementAtIndex(index);
                    if (!(state = PropertyUtilities.SetArrayElementGenericValue(target,
                        property.GetArrayElementAtIndex(index), attribute.ValueCallback,
                        property.GetArrayElementAtIndex(index).propertyType, index)
                        ? new InitState(true)
                        : new InitState(false, "\"" + property.displayName + "\" type is not serializable")).isOk)
                        return state;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    property.DeleteArrayElementAtIndex(index);
                    break;
                }

                index++;
            }

            return state;
        }
    }
}
#endif