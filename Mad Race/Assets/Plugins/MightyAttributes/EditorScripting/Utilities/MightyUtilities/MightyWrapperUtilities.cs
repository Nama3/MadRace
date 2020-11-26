#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MightyAttributes.Editor
{
    public static class MightyWrapperUtilities
    {
        public static bool HasAttributeOfType<T>(this MemberInfo memberInfo, bool includeWrappedAttributes = true) 
            where T : BaseMightyAttribute
        {
            if (GetAttributes<T>(memberInfo).Length > 0) return true;
            if (!includeWrappedAttributes) return false;
            
            var wrappers = GetAttributes<BaseWrapperAttribute>(memberInfo);
            foreach (var wrapper in wrappers)
                if (wrapper.GetType().HasAttributeOfType<T>())
                    return true;

            return false;
        }        
        
        public static bool HasAttributeOfType(this MemberInfo memberInfo, Type attributeType, bool includeWrappedAttributes = true)
        {
            if (GetAttributes(memberInfo, attributeType).Length > 0) return true;
            if (!includeWrappedAttributes) return false;
            
            var wrappers = GetAttributes<BaseWrapperAttribute>(memberInfo);
            foreach (var wrapper in wrappers)
                if (wrapper.GetType().HasAttributeOfType(attributeType))
                    return true;

            return false;
        }
        
        public static T[] GetWrappedAttributes<T>(this BaseMightyMember mightyMember) where T : BaseMightyAttribute
        {
            var wrappedAttributes = new List<T>();

            var wrappers = GetAttributes<BaseWrapperAttribute>(mightyMember.GetMemberInfo());

            if (wrappers.Length > 0)
                GetWrappedAttributes(wrappers, wrappedAttributes, mightyMember, mightyMember.Context.Target);

            return wrappedAttributes.ToArray();
        }

        public static void GetWrappedAttributes<T>(BaseWrapperAttribute[] wrappers, List<T> wrappedAttributes,
            BaseMightyMember mightyMember, object target) where T : BaseMightyAttribute
        {
            foreach (var wrapper in wrappers)
            {
                var nestedWrappers = GetAttributes<BaseWrapperAttribute>(wrapper.GetType());

                if (nestedWrappers.Length > 0)
                    GetWrappedAttributes(nestedWrappers, wrappedAttributes, mightyMember, wrapper);

                if (!mightyMember.TryGetWrapperInfoByWrapper(wrapper, out var info))
                    info = new MightyWrapperInfo(target, wrapper);

                foreach (var attribute in GetAttributes<T>(wrapper.GetType()).Where(x => !(x is IDoNotWrapAttribute)))
                {
                    wrappedAttributes.Add(attribute);
                    info.WrappedAttributes.Add(attribute);
                }

                mightyMember.WrappersInfo.Add(info);
            }
        }

        public static object[] GetWrappedAttributes(this MemberInfo memberInfo, Type attributeType)
        {
            var wrappedAttributes = new List<object>();
            var wrapperList = GetAttributes<BaseWrapperAttribute>(memberInfo);

            if (wrapperList.Length > 0)
                GetWrappedAttributes(wrapperList, attributeType, wrappedAttributes);

            return wrappedAttributes.ToArray();
        }

        public static void GetWrappedAttributes(BaseWrapperAttribute[] wrappers, Type attributeType, List<object> wrappedAttributes)
        {
            foreach (var wrapperAttribute in wrappers)
            {
                var nestedWrappers = GetAttributes<BaseWrapperAttribute>(wrapperAttribute.GetType());

                if (nestedWrappers.Length > 0)
                    GetWrappedAttributes(nestedWrappers, attributeType, wrappedAttributes);

                wrappedAttributes.AddRange(GetAttributes(wrapperAttribute.GetType(), attributeType)
                    .Where(x => !(x is IDoNotWrapAttribute)));
            }
        }

        public static T[] GetAttributes<T>(MemberInfo memberInfo) where T : Attribute => GetAttributes(memberInfo, typeof(T)) as T[];

        public static object[] GetAttributes(MemberInfo memberInfo, Type attributeType) =>
            memberInfo.GetCustomAttributes(attributeType, true);

        public static object GetWrapperReference<T>(object target) where T : Attribute =>
            target != null && target.GetType().GetCustomAttribute(typeof(T), true) == null
                ? GetWrapperReference<T>(target.GetType().GetCustomAttribute<BaseWrapperAttribute>())
                : target;

        public static object GetWrapperReference(object target, Type attributeType) =>
            target != null && target.GetType().GetCustomAttribute(attributeType, true) == null
                ? GetWrapperReference(target.GetType().GetCustomAttribute<BaseWrapperAttribute>(), attributeType)
                : target;

        public static object GetWrapperTarget(this BaseMightyMember mightyMember, BaseWrapperAttribute wrapper) => 
            mightyMember.TryGetWrapperInfoByWrapper(wrapper, out var wrapperInfo) ? wrapperInfo.Target : null;
    }
}
#endif