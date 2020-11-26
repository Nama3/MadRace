#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MightyAttributes.Editor
{
    public static class MightyDrawerUtilities
    {
        #region Populate

        public static bool PopulateAttributesList<Ta>(List<Ta> attributes, IEnumerable<object> allAttributes) where Ta : BaseMightyAttribute
        {
            var any = false;
            foreach (var attribute in allAttributes)
            {
                if (attribute is IExcludeFromAutoRun || !(attribute is Ta mightyAttribute)) continue;

                any = true;
                attributes.Add(mightyAttribute);
            }

            return any;
        }

        public static bool PopulateAttributesListWithException<Ta>(List<Ta> attributes, IEnumerable<object> allAttributes,
            Type attributeExceptionType, bool shouldBeException) where Ta : BaseMightyAttribute
        {
            var any = false;
            foreach (var attribute in allAttributes)
            {
                if (attribute is IExcludeFromAutoRun || !(attribute is Ta mightyAttribute) ||
                    attributeExceptionType.IsInstanceOfType(mightyAttribute) != shouldBeException) continue;

                any = true;
                attributes.Add(mightyAttribute);
            }

            return any;
        }

        #endregion /Populate

        #region Attributes Caches

        #region Members

        public static bool CacheOrderDrawerForField(this BaseMightyMember mightyMember, MemberInfo memberInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            return mightyMember.CacheSingleAttribute<OrderAttribute>(memberInfo.GetCustomAttributes<OrderAttribute>(true)) ||
                   mightyMember.CacheSingleAttribute<OrderAttribute>(wrappedAttributes);
        }

        public static void CacheAnywhereDecoratorsForMember(this BaseMightyMember mightyMember, MemberInfo memberInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var decoratorAttributes = new List<BaseGlobalDecoratorAttribute>();

            var any = PopulateAttributesListWithException(decoratorAttributes,
                memberInfo.GetCustomAttributes<BaseGlobalDecoratorAttribute>(true), typeof(IDrawAnywhereAttribute), true);
            any = PopulateAttributesListWithException(decoratorAttributes, wrappedAttributes, typeof(IDrawAnywhereAttribute), true) || any;

            if (any)
                mightyMember.SetAttributes(decoratorAttributes.ToArray());
        }

        public static void CacheShowConditionForMember(this BaseMightyMember mightyMember, MemberInfo memberInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (mightyMember.CacheSingleAttribute<BaseShowConditionAttribute>(
                memberInfo.GetCustomAttributes<BaseShowConditionAttribute>(true))) return;

            mightyMember.CacheSingleAttribute<BaseShowConditionAttribute>(wrappedAttributes);
        }

        public static void CacheEnableConditionForMember(this BaseMightyMember mightyMember, MemberInfo memberInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (mightyMember.CacheSingleAttribute<BaseEnableConditionAttribute>(
                memberInfo.GetCustomAttributes<BaseEnableConditionAttribute>(true))) return;

            mightyMember.CacheSingleAttribute<BaseEnableConditionAttribute>(wrappedAttributes);
        }

        public static void CacheSimpleGrouperForField(this BaseMightyMember mightyMember, MemberInfo memberInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (!mightyMember.CacheSingleAttribute(memberInfo.GetCustomAttributes<BaseSimpleGroupAttribute>(true),
                out BaseSimpleGroupAttribute attribute) && !mightyMember.CacheSingleAttribute(wrappedAttributes, out attribute)) return;
        }

        public static void CacheFoldableGrouperForField(this BaseMightyMember mightyMember, MemberInfo memberInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (!mightyMember.CacheSingleAttribute(memberInfo.GetCustomAttributes<BaseFoldGroupAttribute>(true),
                out BaseFoldGroupAttribute attribute) && !mightyMember.CacheSingleAttribute(wrappedAttributes, out attribute)) return;
        }

        #endregion /Members

        #region Non Serialized Fields

        public static void CacheNonSerializedDrawerForField(this MightyNonSerializedField nonSerializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (nonSerializedField.CacheSingleAttribute<BaseNonSerializedFieldAttribute>(
                fieldInfo.GetCustomAttributes<BaseNonSerializedFieldAttribute>(true))) return;

            nonSerializedField.CacheSingleAttribute<BaseNonSerializedFieldAttribute>(wrappedAttributes);
        }

        #endregion /Non Serialized Fields

        #region Native Properties

        public static void CacheNativePropertyDrawerForProperty(this MightyNativeProperty nativeProperty, PropertyInfo propertyInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (nativeProperty.CacheSingleAttribute<BaseNativePropertyAttribute>(
                propertyInfo.GetCustomAttributes<BaseNativePropertyAttribute>(true))) return;

            nativeProperty.CacheSingleAttribute<BaseNativePropertyAttribute>(wrappedAttributes);
        }

        #endregion /Native Properties

        #region Methods

        public static void CacheMethodDrawerForMethod(this MightyMethod method, MethodInfo methodInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (method.CacheSingleAttribute<BaseMethodAttribute>(methodInfo.GetCustomAttributes<BaseMethodAttribute>(true))) return;

            method.CacheSingleAttribute<BaseMethodAttribute>(wrappedAttributes);
        }

        #endregion /Methods

        #region Classes

        public static void CacheClassDrawersForType(this MightyMember<Type> mightyMember, Type type,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var classAttributes = new List<BaseClassAttribute>();

            var any = PopulateAttributesList(classAttributes, type.GetCustomAttributes<BaseClassAttribute>(true));
            any = PopulateAttributesList(classAttributes, wrappedAttributes) || any;

            if (any) mightyMember.SetAttributes(classAttributes.ToArray());
        }

        public static void CacheHierarchyForType(this MightyComponent component, Type type,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var hierarchyAttributes = new List<BaseHierarchyAttribute>();

            var any = PopulateAttributesList(hierarchyAttributes, type.GetCustomAttributes<BaseHierarchyAttribute>(true));
            any = PopulateAttributesList(hierarchyAttributes, wrappedAttributes) || any;

            if (any) component.SetAttributes(hierarchyAttributes.ToArray());
        }

        public static void CacheReloadScriptForType(this MightyType mightyType, Type type,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var reloadScriptAttributes = new List<BaseReloadScriptsAttribute>();

            var any = PopulateAttributesList(reloadScriptAttributes, type.GetCustomAttributes<BaseReloadScriptsAttribute>(true));
            any = PopulateAttributesList(reloadScriptAttributes, wrappedAttributes) || any;

            if (any) mightyType.SetAttributes(reloadScriptAttributes.ToArray());
        }

        #endregion /Classes

        #region Serialized Fields

        public static void CacheAutoValueDrawerForField(this MightySerializedField serializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (serializedField.CacheSingleAttribute<BaseAutoValueAttribute>(fieldInfo.GetCustomAttributes<BaseAutoValueAttribute>(true)))
                return;

            serializedField.CacheSingleAttribute<BaseAutoValueAttribute>(wrappedAttributes);
        }

        public static void CacheGlobalDecoratorsForField(this MightySerializedField serializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var decoratorAttributes = new List<BaseGlobalDecoratorAttribute>();

            var any = PopulateAttributesList(decoratorAttributes, fieldInfo.GetCustomAttributes<BaseGlobalDecoratorAttribute>(true));
            any = PopulateAttributesList(decoratorAttributes, wrappedAttributes) || any;

            if (any) serializedField.SetAttributes(decoratorAttributes.ToArray());
        }

        public static void CacheChangeCheckForField(this MightySerializedField serializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var changeCheckAttributes = new List<BaseChangeCheckAttribute>();

            var any = PopulateAttributesList(changeCheckAttributes, fieldInfo.GetCustomAttributes<BaseChangeCheckAttribute>(true));
            any = PopulateAttributesList(changeCheckAttributes, wrappedAttributes) || any;

            if (any) serializedField.SetAttributes(changeCheckAttributes.ToArray());
        }

        public static void CacheValidatorsForField(this MightySerializedField serializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            var validatorAttributes = new List<BaseValidatorAttribute>();

            var any = PopulateAttributesList(validatorAttributes, fieldInfo.GetCustomAttributes<BaseValidatorAttribute>(true));
            any = PopulateAttributesList(validatorAttributes, wrappedAttributes) || any;

            if (any) serializedField.SetAttributes(validatorAttributes.ToArray());
        }

        public static void CachePropertyDrawerForField(this MightySerializedField serializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (serializedField.CacheSingleAttribute<BasePropertyDrawerAttribute>(
                fieldInfo.GetCustomAttributes<BasePropertyDrawerAttribute>(true))) return;

            serializedField.CacheSingleAttribute<BasePropertyDrawerAttribute>(wrappedAttributes);
        }

        public static void CacheArrayDrawerForField(this MightySerializedField serializedField, FieldInfo fieldInfo,
            IEnumerable<BaseMightyAttribute> wrappedAttributes)
        {
            if (serializedField.CacheSingleAttribute<BaseArrayAttribute>(fieldInfo.GetCustomAttributes<BaseArrayAttribute>(true))) return;

            serializedField.CacheSingleAttribute<BaseArrayAttribute>(wrappedAttributes);
        }

        #endregion /Serialized Fields

        #endregion /Attributes Caches
    }
}
#endif