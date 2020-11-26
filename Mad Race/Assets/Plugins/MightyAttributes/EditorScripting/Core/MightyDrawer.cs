#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MightyAttributes.Editor
{
    public class MightyDrawer
    {
        public delegate void PropertyDrawCallback(MightySerializedField serializedField, SerializedProperty property,
            BasePropertyDrawerAttribute attribute);

        #region Fields

        public bool HasMightyMembers { get; private set; }

        private MightyContext m_mightyContext;

        private MightyMember<Type> m_classMember;
        private MightyMembersCache m_membersCache;

        private Dictionary<string, List<BaseMightyMember>> m_groupedMembersByGroup;
        private Dictionary<string, List<BaseMightyMember>> m_foldableGroupedMembersByGroup;

        private HashSet<string> m_drawnGroups;
        private HashSet<string> m_drawnFoldableGroups;

        private int m_serializedFieldCount;
        private bool m_isMonoScript;

        public HideStatus hideStatus = HideStatus.Nothing;
        private bool m_hasOrder;

        #endregion

        #region Enable

        public void OnEnableMonoScript(Object context, SerializedObject serializedObject) =>
            OnEnable(context.GetType(), context, context, serializedObject.FindProperty, true, serializedObject);

        public void OnEnableSerializableClass(Object context, object target, SerializedProperty property) =>
            OnEnable(property.GetSystemType(), context, target, property.FindPropertyRelative, false, null);

        public void OnEnable(Type type, Object context, object target, Func<string, SerializedProperty> findProperty,
            bool isMonoScript, SerializedObject serializedObject)
        {
            m_isMonoScript = isMonoScript;

            if (isMonoScript && findProperty != null) m_mightyContext = new MightyContext(this, context, findProperty("m_Script"), target);
            else m_mightyContext = new MightyContext(this, context, null, target);

            HasMightyMembers = CacheClass(type, m_mightyContext);
            HasMightyMembers = CacheMembers(type, m_mightyContext, findProperty) || HasMightyMembers;

            EnableMembers();

            if (!isMonoScript) return;

            ApplyAutoValues();
            RefreshAllDrawers();
            if (serializedObject != null && serializedObject.hasModifiedProperties) serializedObject.ApplyModifiedProperties();
        }

        private void EnableMembers()
        {
            if (m_classMember != null)
            {
                m_classMember.EnableDrawers();
                if (m_classMember.TryGetAttributes(out BaseClassAttribute[] attributes))
                    foreach (var attribute in attributes)
                        ((IClassDrawer) attribute.Drawer).OnEnableClass(m_classMember, attribute);
            }

            if (m_membersCache == null) return;

            foreach (var mightyMember in m_membersCache.Values)
            {
                mightyMember.EnableDrawers();
                CacheGroupedMember(mightyMember);

                if (!(mightyMember is MightyMethod mightyMethod)) continue;
                if (!mightyMethod.TryGetAttribute(out BaseMethodAttribute attribute)) continue;

                ((IMethodDrawer) attribute.Drawer).OnEnable(mightyMethod, attribute);
            }
        }

        #region Caches

        private bool CacheMembers(Type type, MightyContext context, Func<string, SerializedProperty> findProperty)
        {
            var allMembers = ReflectionUtilities.GetAllMightyMembers(type, findProperty);

            var membersCount = allMembers.Length;
            if (membersCount == 0) return false;

            Array.Sort(allMembers, (first, second) => first.info.GetMemberTypeOrder(first.property != null) -
                                                      second.info.GetMemberTypeOrder(second.property != null));

            m_membersCache = new MightyMembersCache();

            m_groupedMembersByGroup = new Dictionary<string, List<BaseMightyMember>>();
            m_drawnGroups = new HashSet<string>();

            m_foldableGroupedMembersByGroup = new Dictionary<string, List<BaseMightyMember>>();
            m_drawnFoldableGroups = new HashSet<string>();

            for (short i = 0; i < allMembers.Length; i++)
            {
                var (info, serializedProperty) = allMembers[i];
                BaseMightyMember mightyMember = null;
                switch (info)
                {
                    case FieldInfo field:
                        if (serializedProperty != null)
                            mightyMember = CacheSerializedField(field, serializedProperty, context);
                        else
                            mightyMember = CacheNonSerializedField(field, context);
                        break;
                    case PropertyInfo property:
                        mightyMember = CacheNativeProperty(property, context);
                        break;
                    case MethodInfo method:
                        mightyMember = CacheMethod(method, context);
                        break;
                }

                if (mightyMember == null) continue;

                mightyMember.DrawIndex = i;
            }

            if (m_hasOrder) ReorderMembers();

            return true;
        }

        private bool CacheClass(Type type, MightyContext context)
        {
            if (type.IsSubclassOf(typeof(BaseWrapperAttribute)) || !type.HasAttributeOfType<BaseClassAttribute>()) return false;

            m_classMember = new MightyMember<Type>(type, context);

            var wrappedAttributes = m_classMember.GetWrappedAttributes<BaseClassAttribute>();

            m_classMember.CacheClassDrawersForType(type, wrappedAttributes);

            return true;
        }

        private MightySerializedField CacheSerializedField(FieldInfo field, SerializedProperty property, MightyContext context)
        {
            m_serializedFieldCount++;

            var serializedField = (MightySerializedField) m_membersCache.Add(new MightySerializedField(property, field, context));

            var wrappedAttributes = serializedField.GetWrappedAttributes<BaseMightyAttribute>();

            m_hasOrder = serializedField.CacheOrderDrawerForField(field, wrappedAttributes) || m_hasOrder;

            serializedField.CacheAutoValueDrawerForField(field, wrappedAttributes);

            if ((hideStatus & HideStatus.SerializedFields) == HideStatus.SerializedFields) return serializedField;

            serializedField.CacheShowConditionForMember(field, wrappedAttributes);
            serializedField.CacheEnableConditionForMember(field, wrappedAttributes);

            serializedField.CacheChangeCheckForField(field, wrappedAttributes);
            serializedField.CacheValidatorsForField(field, wrappedAttributes);

            serializedField.CacheGlobalDecoratorsForField(field, wrappedAttributes);

            serializedField.CachePropertyDrawerForField(field, wrappedAttributes);
            serializedField.CacheArrayDrawerForField(field, wrappedAttributes);

            serializedField.CacheSimpleGrouperForField(field, wrappedAttributes);
            serializedField.CacheFoldableGrouperForField(field, wrappedAttributes);

            return serializedField;
        }

        private MightyNonSerializedField CacheNonSerializedField(FieldInfo field, MightyContext context)
        {
            var mightyField = (MightyNonSerializedField) m_membersCache.Add(new MightyNonSerializedField(field, context));

            var wrappedAttributes = mightyField.GetWrappedAttributes<BaseMightyAttribute>();

            m_hasOrder = mightyField.CacheOrderDrawerForField(field, wrappedAttributes) || m_hasOrder;

            mightyField.CacheShowConditionForMember(field, wrappedAttributes);
            mightyField.CacheEnableConditionForMember(field, wrappedAttributes);

            mightyField.CacheAnywhereDecoratorsForMember(field, wrappedAttributes);

            mightyField.CacheNonSerializedDrawerForField(field, wrappedAttributes);

            mightyField.CacheSimpleGrouperForField(field, wrappedAttributes);
            mightyField.CacheFoldableGrouperForField(field, wrappedAttributes);

            return mightyField;
        }

        private MightyNativeProperty CacheNativeProperty(PropertyInfo property, MightyContext context)
        {
            var mightyProperty = (MightyNativeProperty) m_membersCache.Add(new MightyNativeProperty(property, context));

            var wrappedAttributes = mightyProperty.GetWrappedAttributes<BaseMightyAttribute>();

            m_hasOrder = mightyProperty.CacheOrderDrawerForField(property, wrappedAttributes) || m_hasOrder;

            mightyProperty.CacheShowConditionForMember(property, wrappedAttributes);
            mightyProperty.CacheEnableConditionForMember(property, wrappedAttributes);

            mightyProperty.CacheAnywhereDecoratorsForMember(property, wrappedAttributes);

            mightyProperty.CacheNativePropertyDrawerForProperty(property, wrappedAttributes);

            mightyProperty.CacheSimpleGrouperForField(property, wrappedAttributes);
            mightyProperty.CacheFoldableGrouperForField(property, wrappedAttributes);

            return mightyProperty;
        }

        private MightyMethod CacheMethod(MethodInfo method, MightyContext context)
        {
            var mightyMethod = (MightyMethod) m_membersCache.Add(new MightyMethod(method, context));

            var wrappedAttributes = mightyMethod.GetWrappedAttributes<BaseMightyAttribute>();

            m_hasOrder = mightyMethod.CacheOrderDrawerForField(method, wrappedAttributes) || m_hasOrder;

            mightyMethod.CacheAnywhereDecoratorsForMember(method, wrappedAttributes);

            mightyMethod.CacheShowConditionForMember(method, wrappedAttributes);
            mightyMethod.CacheEnableConditionForMember(method, wrappedAttributes);

            mightyMethod.CacheMethodDrawerForMethod(method, wrappedAttributes);

            mightyMethod.CacheSimpleGrouperForField(method, wrappedAttributes);
            mightyMethod.CacheFoldableGrouperForField(method, wrappedAttributes);

            return mightyMethod;
        }

        private void CacheGroupedMember(BaseMightyMember mightyMember)
        {
            if (mightyMember.IsGrouped)
            {
                var groupID = mightyMember.GroupID;
                if (m_groupedMembersByGroup.ContainsKey(groupID)) m_groupedMembersByGroup[groupID].Add(mightyMember);
                else m_groupedMembersByGroup[groupID] = new List<BaseMightyMember> {mightyMember};
            }
            else if (mightyMember.IsFoldableGrouped)
            {
                var groupID = mightyMember.GroupID;
                if (m_foldableGroupedMembersByGroup.ContainsKey(groupID))
                    m_foldableGroupedMembersByGroup[groupID].Add(mightyMember);
                else m_foldableGroupedMembersByGroup[groupID] = new List<BaseMightyMember> {mightyMember};
            }
        }

        #endregion /Caches

        #endregion /Enable

        #region Disable

        public void OnDisable()
        {
            if (m_classMember != null)
            {
                if (m_classMember.TryGetAttributes(out BaseClassAttribute[] attributes))
                    foreach (var attribute in attributes)
                        ((IClassDrawer) attribute.Drawer).OnDisableClass(m_classMember, attribute);
            }

            m_groupedMembersByGroup?.Clear();
            m_drawnGroups?.Clear();

            m_foldableGroupedMembersByGroup?.Clear();
            m_drawnFoldableGroups?.Clear();

            m_membersCache?.ClearCache();
        }

        #endregion /Disable

        #region InspectorGUI

        public void OnGUI(SerializedObject serializedObject)
        {
            if (!HasMightyMembers) return;

            BeginOnGUI();
            ManageMembers(out var valueChanged);

            if (m_isMonoScript && valueChanged)
            {
                serializedObject.ManageValueChanged();
                ApplyAutoValues();
                RefreshAllDrawers();
                serializedObject.ManageValueChanged();
            }

            EndOnGUI();
        }

        public void BeginOnGUI()
        {
            MightyEditorUtilities.ResetChange();

            if (m_classMember != null)
            {
                if (m_classMember.TryGetAttributes(out BaseClassAttribute[] attributes))
                    foreach (var attribute in attributes)
                        ((IClassDrawer) attribute.Drawer).BeginDrawClass(m_classMember, attribute);
            }

            if ((hideStatus & HideStatus.ScriptField) != HideStatus.ScriptField && m_isMonoScript)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(m_mightyContext.ScriptProperty);
                GUI.enabled = true;
            }
        }

        public void EndOnGUI()
        {
            if (m_classMember == null)
            {
                hideStatus = HideStatus.Nothing;
                return;
            }

            if (m_classMember.TryGetAttributes(out BaseClassAttribute[] attributes))
                foreach (var attribute in attributes)
                    ((IClassDrawer) attribute.Drawer).EndDrawClass(m_classMember, attribute);

            hideStatus = HideStatus.Nothing;
        }

        public void ManageMembers(out bool valueChanged)
        {
            if (m_membersCache == null || m_membersCache.Count == 0)
            {
                valueChanged = false;
                return;
            }

            m_drawnGroups.Clear();
            m_drawnFoldableGroups.Clear();

            valueChanged = false;

            for (short i = 0; i < m_membersCache.Count; i++)
            {
                var mightyMember = m_membersCache[i];
                mightyMember.DrawIndex = i;

                if (!valueChanged)
                    EditorGUI.BeginChangeCheck();

                ManageGroups(mightyMember, ref valueChanged);

                valueChanged = valueChanged || EditorGUI.EndChangeCheck() || MightyEditorUtilities.HasEditorChanged();
            }
        }

        private void ManageGroups(BaseMightyMember mightyMember, ref bool valueChanged)
        {
            if (mightyMember.IsGrouped)
            {
                var groupID = mightyMember.GroupID;
                if (m_drawnGroups.Contains(groupID)) return;

                if (!mightyMember.TryGetAttribute(out BaseSimpleGroupAttribute groupAttribute))
                {
                    DrawMember(mightyMember, true, ref valueChanged);
                    return;
                }

                var grouper = (ISimpleGrouper) groupAttribute.Drawer;

                var currentFields = m_groupedMembersByGroup[groupID];

                var canDrawGroup = true;
                var flag = false;
                foreach (var currentField in currentFields)
                    canDrawGroup = flag = !currentField.TryGetAttribute(out BaseShowConditionAttribute conditionAttribute) ||
                                          ((IShowConditionDrawer) conditionAttribute.Drawer).CanDraw(currentField, conditionAttribute) ||
                                          flag;

                if (canDrawGroup)
                {
                    m_drawnGroups.Add(groupID);
                    grouper.BeginGroup(mightyMember, groupAttribute);
                }

                DrawMembers(currentFields, canDrawGroup, ref valueChanged);
                if (canDrawGroup) grouper.EndGroup(mightyMember, groupAttribute);
            }
            else if (mightyMember.IsFoldableGrouped)
            {
                var groupID = mightyMember.GroupID;
                if (m_drawnFoldableGroups.Contains(groupID)) return;

                if (!mightyMember.TryGetAttribute(out BaseFoldGroupAttribute foldableGroupAttribute))
                {
                    DrawMember(mightyMember, true, ref valueChanged);
                    return;
                }

                var foldableGrouper = (IFoldGrouper) foldableGroupAttribute.Drawer;

                var currentFields = m_foldableGroupedMembersByGroup[groupID];

                var canDrawGroup = true;
                var flag = false;
                foreach (var currentField in currentFields)
                    canDrawGroup = flag = !currentField.TryGetAttribute(out BaseShowConditionAttribute conditionAttribute) ||
                                          ((IShowConditionDrawer) conditionAttribute.Drawer).CanDraw(currentField, conditionAttribute) ||
                                          flag;

                if (canDrawGroup)
                {
                    m_drawnFoldableGroups.Add(groupID);

                    var canDraw = foldableGrouper.CanDraw(mightyMember, foldableGroupAttribute);

                    if (canDraw) foldableGrouper.BeginGroup(mightyMember);
                    DrawMembers(currentFields, canDraw, ref valueChanged);

                    foldableGrouper.EndGroup(mightyMember, canDraw);
                }
            }
            else DrawMember(mightyMember, true, ref valueChanged);
        }

        private void DrawMembers(List<BaseMightyMember> mightyMembers, bool canDraw, ref bool valueChanged)
        {
            foreach (var mightyMember in mightyMembers) DrawMember(mightyMember, canDraw, ref valueChanged);
        }

        private void DrawMember(BaseMightyMember mightyMember, bool canDraw, ref bool valueChanged)
        {
            switch (mightyMember)
            {
                case MightySerializedField serializedField when canDraw:
                    DrawSerializedField(serializedField);
                    break;
                case MightyNonSerializedField nonSerializedField when canDraw:
                    DrawNonSerializedField(nonSerializedField);
                    break;
                case MightyNativeProperty nativeProperty when canDraw:
                    DrawNativeProperty(nativeProperty);
                    break;
                case MightyMethod method:
                    DrawMethod(method, canDraw, ref valueChanged);
                    break;
            }
        }

        private void DrawSerializedField(MightySerializedField serializedField)
        {
            if ((hideStatus & HideStatus.SerializedFields) == HideStatus.SerializedFields) return;

            var hasDecorations = serializedField.TryGetAttributes(out BaseGlobalDecoratorAttribute[] decoratorAttributes);

            if (serializedField.TryGetAttribute(out BaseShowConditionAttribute showConditionAttribute))
            {
                var canDraw = ((IShowConditionDrawer) showConditionAttribute.Drawer).CanDraw(serializedField, showConditionAttribute);

                if (!canDraw)
                {
                    if (!hasDecorations) return;

                    foreach (var attribute in decoratorAttributes)
                    {
                        if (!(attribute is IDrawAnywhereAttribute drawAnywhereAttribute)) continue;

                        ((IDrawAnywhereDecorator) attribute.Drawer).BeginDrawAnywhere(serializedField,
                            drawAnywhereAttribute);
                    }


                    for (var i = decoratorAttributes.Length - 1; i >= 0; i--)
                    {
                        if (!(decoratorAttributes[i] is IDrawAnywhereAttribute drawAnywhereAttribute)) continue;

                        ((IDrawAnywhereDecorator) decoratorAttributes[i].Drawer).EndDrawAnywhere(serializedField,
                            drawAnywhereAttribute);
                    }

                    return;
                }
            }

            var hasArrayAttribute = serializedField.TryGetAttribute(out BaseArrayAttribute arrayAttribute);
            var hasChangeChecks = serializedField.TryGetAttributes(out BaseChangeCheckAttribute[] changeCheckAttributes);
            var hasValidators = serializedField.TryGetAttributes(out BaseValidatorAttribute[] validatorAttributes);
            var hasDrawerAttribute = serializedField.TryGetAttribute(out BasePropertyDrawerAttribute drawerAttribute);
            var hasEnableCondition = serializedField.TryGetAttribute(out BaseEnableConditionAttribute enableConditionAttribute);

            var decoratorsLength = hasDecorations ? decoratorAttributes.Length : 0;

            var lastArrayDecoratorIndex = -1;

            var isPropertyCollection = serializedField.Property.IsCollection();

            if (isPropertyCollection)
                for (var i = decoratorsLength - 1; i >= 0; i--)
                {
                    if (!(decoratorAttributes[i] is BaseArrayDecoratorAttribute)) continue;

                    lastArrayDecoratorIndex = i;
                    break;
                }

            if (hasDecorations)
                for (var i = 0; i < decoratorsLength; i++)
                {
                    switch (decoratorAttributes[i])
                    {
                        case BaseArrayDecoratorAttribute arrayDecoratorAttribute
                            when (!hasArrayAttribute && (!isPropertyCollection || i != lastArrayDecoratorIndex)):
                            ((IArrayDecoratorDrawer) arrayDecoratorAttribute.Drawer).BeginDrawMember(serializedField, arrayDecoratorAttribute, null);
                            break;
                        case BaseDecoratorAttribute decoratorAttribute:
                            ((IDecoratorDrawer) decoratorAttribute.Drawer).BeginDraw(serializedField,
                                decoratorAttribute);
                            break;
                    }
                }

            
            if (hasEnableCondition)
                ((IEnableConditionDrawer) enableConditionAttribute.Drawer).BeginEnable(serializedField, enableConditionAttribute);

            EditorGUI.BeginChangeCheck();
            if (hasChangeChecks)
                foreach (var attribute in changeCheckAttributes)
                    ((IChangeCheckDrawer) attribute.Drawer).BeginChangeCheck(serializedField, attribute);

            if (!hasArrayAttribute)
            {
                var propertyDrawCallback = hasDrawerAttribute
                    ? new PropertyDrawCallback((mightyMember, property, baseAttribute) =>
                        ((IPropertyDrawer) drawerAttribute.Drawer).DrawProperty(mightyMember, property,
                            baseAttribute))
                    : (_, property, __) => MightyGUIUtilities.DrawPropertyField(property);

                if (lastArrayDecoratorIndex != -1)
                {
                    var lastAttribute = (BaseArrayDecoratorAttribute) decoratorAttributes[lastArrayDecoratorIndex];
                    var lastDecorator = (IArrayDecoratorDrawer) lastAttribute.Drawer;

                    lastDecorator.BeginDrawMember(serializedField, lastAttribute, propertyDrawCallback, drawerAttribute);
                    lastDecorator.EndDrawMember(serializedField, lastAttribute, propertyDrawCallback, drawerAttribute);
                }
                else
                    propertyDrawCallback(serializedField, serializedField.Property, drawerAttribute);
            }
            else
                ((IArrayDrawer) arrayAttribute.Drawer).DrawArray(serializedField, arrayAttribute,
                    drawerAttribute?.Drawer as IArrayElementDrawer, drawerAttribute);

            if (hasValidators)
                foreach (var attribute in validatorAttributes)
                    ((IValidatorDrawer) attribute.Drawer).ValidateProperty(serializedField, attribute);

            var changed = EditorGUI.EndChangeCheck() || MightyEditorUtilities.HasEditorChanged();
            if (hasChangeChecks)
                if (hasChangeChecks)
                    foreach (var attribute in changeCheckAttributes)
                        ((IChangeCheckDrawer) attribute.Drawer).EndChangeCheck(changed, serializedField,
                            attribute);

            if (hasEnableCondition)
                ((IEnableConditionDrawer) enableConditionAttribute.Drawer).EndEnable(serializedField,
                    enableConditionAttribute);

            if (hasDecorations)
                for (var i = decoratorsLength - 1; i >= 0; i--)
                {
                    switch (decoratorAttributes[i])
                    {
                        case BaseArrayDecoratorAttribute arrayDecoratorAttribute
                            when (!hasArrayAttribute && (!isPropertyCollection || i != lastArrayDecoratorIndex)):
                            ((IArrayDecoratorDrawer) arrayDecoratorAttribute.Drawer).EndDrawMember(
                                serializedField, (BaseArrayDecoratorAttribute) decoratorAttributes[i], null);
                            break;
                        case BaseDecoratorAttribute decoratorAttribute:
                            ((IDecoratorDrawer) decoratorAttribute.Drawer).EndDraw(serializedField,
                                (BaseDecoratorAttribute) decoratorAttributes[i]);
                            break;
                    }
                }
        }

        private static void DrawNonSerializedField(MightyNonSerializedField nonSerializedField)
        {
            DrawMember(nonSerializedField, true, (member, canDrawMember) =>
            {
                if (canDrawMember && member.TryGetAttribute(out BaseNonSerializedFieldAttribute attribute))
                    ((INonSerializedFieldDrawer) attribute.Drawer).DrawField((MightyNonSerializedField) member, attribute);
            });
        }

        private static void DrawNativeProperty(MightyNativeProperty nativeProperty)
        {
            DrawMember(nativeProperty, true, (member, canDrawMember) =>
            {
                if (canDrawMember && member.TryGetAttribute(out BaseNativePropertyAttribute attribute))
                    ((INativePropertyDrawer) attribute.Drawer).DrawNativeProperty(
                        (MightyNativeProperty) member, attribute);
            });
        }

        private static void DrawMethod(MightyMethod method, bool canDraw, ref bool valueChanged)
        {
            var valueChangedRef = valueChanged;
            DrawMember(method, canDraw, (member, canDrawMember) =>
            {
                if (!member.TryGetAttribute(out BaseMethodAttribute attribute)) return;

                var drawer = (IMethodDrawer) attribute.Drawer;

                EditorGUI.BeginChangeCheck();

                drawer.OnInspectorGUI(canDrawMember, (MightyMethod) member, attribute);

                var modified = EditorGUI.EndChangeCheck() || valueChangedRef || MightyEditorUtilities.HasEditorChanged();

                drawer.OnModifiedProperties(modified, (MightyMethod) member, attribute);
                valueChangedRef = valueChangedRef || modified || MightyEditorUtilities.HasEditorChanged();
            });
            valueChanged = valueChangedRef;
        }

        private static void DrawMember<T>(MightyMember<T> mightyMember, bool canDraw, Action<MightyMember<T>, bool> memberDrawer)
            where T : MemberInfo
        {
            if (canDraw)
            {
                if (mightyMember.TryGetAttribute(out BaseShowConditionAttribute showConditionAttribute))
                    canDraw = ((IShowConditionDrawer) showConditionAttribute.Drawer).CanDraw(mightyMember,
                        showConditionAttribute);
            }

            if (canDraw)
            {
                var hasDecorators = mightyMember.TryGetAttributes(out BaseGlobalDecoratorAttribute[] decoratorAttributes);
                var hasEnableCondition = mightyMember.TryGetAttribute(out BaseEnableConditionAttribute enableConditionAttribute);

                if (hasDecorators)
                    foreach (var attribute in decoratorAttributes)
                        if (attribute is IDrawAnywhereAttribute drawAnywhereAttribute)
                            ((IDrawAnywhereDecorator) attribute.Drawer).BeginDrawAnywhere(mightyMember,
                                drawAnywhereAttribute);


                if (hasEnableCondition)
                    ((IEnableConditionDrawer) enableConditionAttribute.Drawer).BeginEnable(mightyMember,
                        enableConditionAttribute);

                memberDrawer(mightyMember, true);

                if (hasEnableCondition)
                    ((IEnableConditionDrawer) enableConditionAttribute.Drawer).EndEnable(mightyMember,
                        enableConditionAttribute);

                if (hasDecorators)
                    for (var i = decoratorAttributes.Length - 1; i >= 0; i--)
                        if (decoratorAttributes[i] is IDrawAnywhereAttribute drawAnywhereAttribute)
                            ((IDrawAnywhereDecorator) decoratorAttributes[i].Drawer).EndDrawAnywhere(mightyMember,
                                drawAnywhereAttribute);
            }
            else
                memberDrawer(mightyMember, false);
        }

        public void ApplyAutoValues()
        {
            var count = 0;

            if (m_membersCache == null) return;

            foreach (var mightyMember in m_membersCache.Values)
            {
                if (count == m_serializedFieldCount) break;

                if (!(mightyMember is MightySerializedField serializedField)) continue;

                count++;
                serializedField.ApplyAutoValue();

                if (serializedField.TryGetAttribute(out NestAttribute attribute))
                    ((NestDrawer) attribute.Drawer).ApplyAutoValues(serializedField, attribute);
            }
        }

        public void RefreshAllDrawers(bool refreshSerializedFields = true)
        {
            m_classMember?.RefreshDrawers();

            if (m_membersCache != null)
                foreach (var mightyMember in m_membersCache.Values)
                    if (refreshSerializedFields || !(mightyMember is MightySerializedField))
                        mightyMember.RefreshDrawers();

            MightyHierarchy.RefreshHierarchyDrawers();
        }

        #endregion /InspectorGUI

        public void ReorderMembers() => m_membersCache.ApplyDrawOrders();
    }
}
#endif