#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public class MightyWrapperInfo
    {
        public object Target { get; }

        public BaseWrapperAttribute Wrapper { get; }
        public List<BaseMightyAttribute> WrappedAttributes { get; }

        public MightyWrapperInfo(object target, BaseWrapperAttribute wrapper)
        {
            Target = target;

            Wrapper = wrapper;
            WrappedAttributes = new List<BaseMightyAttribute>();
        }
    }

    public class MightyContext
    {
        public MightyDrawer Drawer { get; }

        public Object Object { get; }
        public SerializedProperty ScriptProperty { get; }
        public object Target { get; }

        public MightyContext(MightyDrawer drawer, Object obj, SerializedProperty scriptProperty, object target)
        {
            Object = obj;
            ScriptProperty = scriptProperty;
            Target = target;
            Drawer = drawer;
        }
    }

    public class MightyComponentContext : MightyContext
    {
        public Component Component => (Component) Object;
        public GameObject GameObject => Component.gameObject;

        public MightyComponentContext(Component component) : base(null, component, null, component)
        {
        }
    }

    public abstract class BaseMightyMember
    {
        public long ID { get; }
        public MightyContext Context { get; }

        public short DrawIndex { get; set; }

        public List<MightyWrapperInfo> WrappersInfo { get; }

        public bool IsGrouped { get; private set; }
        public bool IsFoldableGrouped { get; private set; }

        public string GroupID { get; private set; }
        public string GroupName { get; private set; }

        protected BaseMightyMember(MightyContext context)
        {
            ID = ReferencesUtilities.GetUniqueID(this);
            Context = context;

            WrappersInfo = new List<MightyWrapperInfo>();
        }

        #region Abstract

        public abstract string MemberName { get; }
        public abstract MemberInfo GetMemberInfo();

        public abstract bool IsFoldable();

        public abstract bool HasAttribute<Ta>() where Ta : BaseMightyAttribute;

        public abstract bool TryGetAttribute<Ta>(out Ta attribute) where Ta : BaseMightyAttribute;
        public abstract void SetAttribute<Ta>(Ta attribute) where Ta : BaseMightyAttribute;

        public abstract bool TryGetAttributes<Ta>(out Ta[] attributes) where Ta : BaseMightyAttribute;
        public abstract void SetAttributes<Ta>(Ta[] attributes) where Ta : BaseMightyAttribute;

        public abstract object GetAttributeTarget<Ta>(Ta attribute) where Ta : BaseMightyAttribute;
        public abstract void EnableDrawers();

        protected abstract (IRefreshDrawer[], BaseMightyAttribute[]) GetRefreshDrawers();

        #endregion /Abstract

        #region Public

        public void RefreshDrawers()
        {
            var (drawers, attributes) = GetRefreshDrawers();
            for (var i = 0; i < drawers.Length; i++) drawers[i].RefreshDrawer(this, attributes[i]);
        }

        public bool TryGetWrapperInfoByAttribute(BaseMightyAttribute attribute, out MightyWrapperInfo wrapperInfo)
        {
            foreach (var info in WrappersInfo)
            {
                if (!info.WrappedAttributes.Contains(attribute)) continue;

                wrapperInfo = info;
                return true;
            }

            wrapperInfo = null;
            return false;
        }

        public bool TryGetWrapperInfoByWrapper(BaseWrapperAttribute wrapper, out MightyWrapperInfo wrapperInfo)
        {
            foreach (var info in WrappersInfo)
            {
                if (!Equals(info.Wrapper, wrapper)) continue;

                wrapperInfo = info;
                return true;
            }

            wrapperInfo = null;
            return false;
        }

        public bool TryGetDrawOrder(out short drawOrder)
        {
            if (TryGetAttribute(out OrderAttribute attribute))
            {
                drawOrder = MightyDrawersDatabase.GetDrawer<OrderDrawer>().GetOrder(this, attribute);
                return true;
            }

            drawOrder = short.MaxValue;
            return false;
        }

        public string GetGroupName(BaseGroupAttribute baseAttribute)
        {
            var name = baseAttribute.GroupName;
            if (baseAttribute.NameAsCallback) this.GetValueFromMember(baseAttribute.Target, name, out name);
            return name ?? "";
        }

        public void SetGroup(Type groupType, string groupName, bool foldable)
        {
            if (foldable) IsFoldableGrouped = true;
            else IsGrouped = true;

            GroupID = $"{Context.Object.GetInstanceID()}.{Context.Target.GetHashCode()}.{groupType}.{groupName}";
            GroupName = groupName;
        }

        #endregion /Public

        #region Cache

        public bool CacheSingleAttribute<Ta>(IEnumerable<object> attributes, out Ta outAttribute) where Ta : BaseMightyAttribute
        {
            foreach (var attribute in attributes)
                if (CacheSingleAttribute(attribute, out outAttribute))
                    return true;

            outAttribute = default;
            return false;
        }

        public bool CacheSingleAttribute<Ta>(IEnumerable<object> attributes) where Ta : BaseMightyAttribute
        {
            foreach (var attribute in attributes)
                if (CacheSingleAttribute<Ta>(attribute))
                    return true;

            return false;
        }

        public bool CacheSingleAttribute<Ta>(object attribute, out Ta outAttribute) where Ta : BaseMightyAttribute
        {
            if (attribute is IExcludeFromAutoRun || !(attribute is Ta mightyAttribute))
            {
                outAttribute = default;
                return false;
            }

            SetAttribute(mightyAttribute);
            outAttribute = mightyAttribute;
            return true;
        }

        private bool CacheSingleAttribute<Ta>(object attribute) where Ta : BaseMightyAttribute
        {
            if (attribute is IExcludeFromAutoRun || !(attribute is Ta mightyAttribute)) return false;

            SetAttribute(mightyAttribute);
            return true;
        }

        #endregion /Cache
    }

    public class MightyMember<T> : BaseMightyMember where T : MemberInfo
    {
        protected readonly Dictionary<Type, BaseMightyAttribute> m_attributesByType = new Dictionary<Type, BaseMightyAttribute>();

        public T MemberInfo { get; }

        public MightyMember(T memberInfo, MightyContext context) : base(context)
        {
            MemberInfo = memberInfo;
            MemberName = memberInfo.Name;
        }

        #region Overrides

        public override string MemberName { get; }
        public override MemberInfo GetMemberInfo() => MemberInfo;
        public override bool IsFoldable() => false;

        public override bool HasAttribute<Ta>()
        {
            if (m_attributesByType.ContainsKey(typeof(Ta))) return true;

            foreach (var value in m_attributesByType.Values)
                if (value is Ta)
                    return true;

            return false;
        }

        public override bool TryGetAttribute<Ta>(out Ta attribute)
        {
            if (m_attributesByType.TryGetValue(typeof(Ta), out var mightyAttribute))
            {
                attribute = mightyAttribute as Ta;
                return true;
            }

            foreach (var value in m_attributesByType.Values)
                if (value is Ta ta)
                {
                    attribute = ta;
                    return true;
                }

            attribute = default;
            return false;
        }

        public override void SetAttribute<Ta>(Ta attribute) => m_attributesByType[attribute.GetType()] = attribute;

        public override bool TryGetAttributes<Ta>(out Ta[] attributes)
        {
            var attributesList = new List<Ta>();

            foreach (var value in m_attributesByType.Values)
                if (value is Ta ta)
                    attributesList.Add(ta);

            if (attributesList.Count > 0)
            {
                attributes = attributesList.ToArray();
                return true;
            }

            attributes = default;
            return false;
        }

        public override void SetAttributes<Ta>(Ta[] attributes)
        {
            foreach (var attribute in attributes) SetAttribute(attribute);
        }

        public override object GetAttributeTarget<Ta>(Ta attribute) =>
            MemberInfo.GetAttributeTarget(attribute.GetType(), Context.Target) ?? Context.Target;

        public override void EnableDrawers()
        {
            foreach (var value in m_attributesByType.Values)
            {
                value.InitAttribute(GetAttributeTarget(value));
                value.Drawer.EnableDrawer(this, value);
            }
        }

        protected override (IRefreshDrawer[], BaseMightyAttribute[]) GetRefreshDrawers()
        {
            var drawersList = new List<IRefreshDrawer>();
            var attributesList = new List<BaseMightyAttribute>();
            foreach (var value in m_attributesByType.Values)
            {
                if (!(MightyDrawersDatabase.GetDrawerForAttribute<IMightyDrawer>(value.GetType()) is IRefreshDrawer refreshDrawer))
                    continue;

                drawersList.Add(refreshDrawer);
                attributesList.Add(value);
            }

            return (drawersList.ToArray(), attributesList.ToArray());
        }

        #endregion /Overrides
    }

    public class MightySerializedField : MightyMember<FieldInfo>
    {
        public SerializedProperty Property { get; }
        public Type PropertyType { get; }

        public bool IsExpanded => Property.isExpanded;
        public int ArraySize => Property.IsCollection() ? Property.arraySize : -1;

        public MightySerializedField(SerializedProperty property, FieldInfo fieldInfo, MightyContext context) : base(fieldInfo, context)
        {
            Property = property;
            PropertyType = property.GetSystemType();
        }

        public SerializedProperty GetElement(int index) => !Property.IsCollection() ? Property : Property.GetArrayElementAtIndex(index);

        public int GetElementIndex(SerializedProperty property)
        {
            if (Property.IsCollection())
                for (var i = 0; i < Property.arraySize; i++)
                    if (Property.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
                        return i;
            return 0;
        }

        public void ApplyAutoValue()
        {
            if (!TryGetAttribute(out BaseAutoValueAttribute attribute)) return;

            var drawer = MightyDrawersDatabase.GetDrawerForAttribute<IAutoValueDrawer>(attribute.GetType());

            var state = drawer.InitProperty(this, attribute);
            if (!state.isOk) MightyGUIUtilities.DrawHelpBox(state.message);
            else MightyEditorUtilities.RegisterChange();
        }

        #region Overrides

        public override bool IsFoldable()
        {
            if (Property.IsCollection())
            {
                if (!TryGetAttribute(out BaseArrayAttribute attribute)) return true;

                return !((IArrayDrawer) attribute.Drawer).GetOptionsForMember(this, attribute).Contains(ArrayOption.HideLabel);
            }

            if (PropertyType.IsSerializableClassOrStruct())
            {
                if (!TryGetAttribute(out NestAttribute attribute)) return true;

                return !((NestDrawer) attribute.Drawer).GetOptionsForMember(this, attribute).Contains(NestOption.DontFold);
            }

            return false;
        }

        public override object GetAttributeTarget<Ta>(Ta attribute) => Property.GetAttributeTarget(attribute.GetType()) ?? Context.Target;

        #endregion /Overrides
    }

    public class MightyNonSerializedField : MightyMember<FieldInfo>
    {
        public MightyNonSerializedField(FieldInfo memberInfo, MightyContext context) : base(memberInfo, context)
        {
        }
    }

    public class MightyNativeProperty : MightyMember<PropertyInfo>
    {
        public MightyNativeProperty(PropertyInfo memberInfo, MightyContext context) : base(memberInfo, context)
        {
        }
    }

    public class MightyMethod : MightyMember<MethodInfo>
    {
        public MightyMethod(MethodInfo memberInfo, MightyContext context) : base(memberInfo, context)
        {
        }
    }

    public class MightyComponent : MightyMember<Type>
    {
        public MightyComponentContext ComponentContext => (MightyComponentContext) Context;

        public MightyComponent(Type memberInfo, MightyComponentContext context) : base(memberInfo, context)
        {
        }
    }

    public class MightyType : MightyMember<Type>
    {
        public Assembly Assembly { get; }

        public MightyType(Type type, Assembly assembly, object target) : base(type, new MightyContext(null, null, null, target)) =>
            Assembly = assembly;
    }
}
#endif