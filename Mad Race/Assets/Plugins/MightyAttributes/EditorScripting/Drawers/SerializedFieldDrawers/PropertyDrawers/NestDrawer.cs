#if UNITY_EDITOR
using System;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class NestDrawer : BasePropertyDrawer<NestAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(object[], MightyDrawer[], int)> m_nestCache = new MightyCache<(object[], MightyDrawer[], int)>();

        private readonly MightyCache<MightyInfo<NestOption>> m_optionsCache = new MightyCache<MightyInfo<NestOption>>();

        public MightyDrawer EnableProperty(SerializedProperty property, object classReference)
        {
            if (classReference.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length == 0)
            {
                MightyGUIUtilities.DrawPropertyField(property);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" type should be a Serializable class");
                return null;
            }

            var drawer = new MightyDrawer();
            drawer.OnEnableSerializableClass(property.GetTargetObject(), classReference, property);
            return drawer;
        }

        public void DrawProperty(SerializedProperty property, NestOption options, MightyDrawer drawer)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawPropertyField(property);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" can't be an array");
                return;
            }

            DrawNest(property, options, drawer);
        }

        private void DrawNest(SerializedProperty property, NestOption options, MightyDrawer drawer)
        {
            if (!drawer.HasMightyMembers)
            {
                MightyGUIUtilities.DrawPropertyField(property);
                return;
            }

            if (options == NestOption.ContentOnly)
                drawer.OnGUI(property.serializedObject);
            else
            {
                if (!options.Contains(NestOption.HideLabel))
                {
                    if (options.Contains(NestOption.DontFold))
                        DrawLabel(property, (FieldOption) options);
                    else if (!MightyGUIUtilities.DrawFoldout(property,
                        options.Contains(NestOption.BoldLabel) ? MightyStyles.BoldFoldout : EditorStyles.foldout))
                        return;
                }

                if (!options.Contains(NestOption.DontIndent)) EditorGUI.indentLevel++;
                drawer.OnGUI(property.serializedObject);
                if (!options.Contains(NestOption.DontIndent)) EditorGUI.indentLevel--;
            }
        }

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, NestAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawNest(mightyMember, property, mightyMember.GetElementIndex(property), attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawNest(serializedField, serializedField.GetElement(index), index, (NestAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawNest(serializedField, serializedField.GetElement(index), index, (NestAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawNest(serializedField, serializedField.GetElement(index), index, (NestAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

        private void DrawNest(MightySerializedField serializedField, SerializedProperty property, int index, NestAttribute attribute,
            GUIContent label = null)
        {
            if (!serializedField.PropertyType.IsSerializableClassOrStruct())
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" type should be a Serializable class");
                return;
            }

            if (!m_nestCache.Contains(serializedField) || !m_optionsCache.Contains(serializedField))
                EnableDrawer(serializedField, attribute);
            var (_, drawers, size) = m_nestCache[serializedField];

            if (index >= size) return;

            if (!drawers[index].HasMightyMembers)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                return;
            }

            var options = m_optionsCache[serializedField].Value;

            if (options == NestOption.ContentOnly)
                drawers[index].OnGUI(property.serializedObject);
            else
            {
                if (!options.Contains(NestOption.HideLabel))
                {
                    if (options.Contains(NestOption.DontFold))
                        DrawLabel(property, (FieldOption) options, label);
                    else if (!MightyGUIUtilities.DrawFoldout(property, label ?? EditorGUIUtility.TrTextContent(property.displayName),
                        options.Contains(NestOption.BoldLabel) ? MightyStyles.BoldFoldout : EditorStyles.foldout))
                        return;
                }

                if (!options.Contains(NestOption.DontIndent)) EditorGUI.indentLevel++;
                drawers[index].OnGUI(property.serializedObject);
                if (!options.Contains(NestOption.DontIndent)) EditorGUI.indentLevel--;
            }
        }

        public void ApplyAutoValues(BaseMightyMember mightyMember, NestAttribute attribute)
        {
            if (!m_nestCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            foreach (var drawer in m_nestCache[mightyMember].Item2) drawer.ApplyAutoValues();
        }

        public NestOption GetOptionsForMember(BaseMightyMember mightyMember, NestAttribute mightyAttribute)
        {
            if (!m_optionsCache.Contains(mightyMember))
                EnableDrawer(mightyMember, mightyAttribute);

            return m_optionsCache[mightyMember].Value;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, NestAttribute attribute)
        {
            if (!serializedField.PropertyType.IsSerializableClassOrStruct())
                return;

            var property = serializedField.Property;
            var context = serializedField.Context.Object;

            var isArray = property.IsCollection();
            var size = isArray ? property.arraySize : 1;
            var classReferences = new object[size];
            var drawers = new MightyDrawer[size];
            MightyInfo<NestOption> optionsInfo = null;

            if (m_nestCache.TryGetValue(serializedField, out var cache) &&
                m_optionsCache.TryGetValue(serializedField, out optionsInfo))
            {
                var (cacheClasses, cacheDrawers, cacheSize) = cache;
                for (var i = 0; i < size; i++)
                {
                    if (cacheSize < size) continue;
                    classReferences[i] = cacheClasses[i];
                    drawers[i] = cacheDrawers[i];
                }
            }

            if (isArray)
                for (var i = 0; i < size; i++)
                {
                    var element = property.GetArrayElementAtIndex(i);
                    if (classReferences[i] == null) classReferences[i] = element.GetSerializableClassReference();
                    if (drawers[i] == null)
                        (drawers[i] = new MightyDrawer()).OnEnableSerializableClass(context, classReferences[i], element);
                }
            else
            {
                if (classReferences[0] == null) classReferences[0] = property.GetSerializableClassReference();
                if (drawers[0] == null)
                    (drawers[0] = new MightyDrawer()).OnEnableSerializableClass(context, classReferences[0], property);
            }


            if (optionsInfo == null)
            {
                if (!serializedField.GetInfoFromMember(attribute.Target, attribute.OptionsCallback, out optionsInfo))
                    optionsInfo = new MightyInfo<NestOption>(attribute.NestOptions);
            }

            m_nestCache[serializedField] = (classReferences, drawers, size);
            m_optionsCache[serializedField] = optionsInfo;
        }

        protected override void ClearCache()
        {
            foreach (var (_, drawers, _) in m_nestCache.Values)
            foreach (var drawer in drawers)
                drawer.OnDisable();

            m_nestCache.ClearCache();
            m_optionsCache.ClearCache();
        }

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_nestCache.Contains(mightyMember) || !m_optionsCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (_, _, arraySize) = m_nestCache[mightyMember];

            var property = ((MightySerializedField) mightyMember).Property;
            if (property.IsCollection() && property.arraySize != arraySize)
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_optionsCache[mightyMember].RefreshValue();
        }
    }
}
#endif