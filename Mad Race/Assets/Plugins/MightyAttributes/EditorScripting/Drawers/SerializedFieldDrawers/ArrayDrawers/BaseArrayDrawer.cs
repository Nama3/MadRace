#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IArrayDrawer : ISerializedFieldDrawer
    {
        void DrawArray(MightySerializedField serializedField, BaseArrayAttribute attribute, IArrayElementDrawer drawerAttributeDrawer, 
            BasePropertyDrawerAttribute drawerAttribute);

        ArrayOption GetOptionsForMember(BaseMightyMember serializedField, BaseArrayAttribute attribute);
    }

    public abstract class BaseArrayDrawer<T> : BaseSerializeFieldDrawer<T>, IArrayDrawer, IRefreshDrawer where T : BaseArrayAttribute
    {
        private readonly MightyCache<MightyInfo<ArrayOption>> m_optionsCache = new MightyCache<MightyInfo<ArrayOption>>();

        private readonly MightyCache<BaseArrayDecoratorAttribute[]> m_decorationsCache = new MightyCache<BaseArrayDecoratorAttribute[]>();

        public void DrawArray(MightySerializedField serializedField, BaseArrayAttribute attribute,
            IArrayElementDrawer drawerAttributeDrawer, BasePropertyDrawerAttribute drawerAttribute) =>
            DrawArray(serializedField, (T) attribute, drawerAttributeDrawer, drawerAttribute);

        public void DrawArray(MightySerializedField serializedField, T attribute, IArrayElementDrawer drawer,
            BasePropertyDrawerAttribute drawerAttribute)
        {
            var property = serializedField.Property;
            if (!property.IsCollection())
            {
                MightyGUIUtilities.DrawHelpBox($"{attribute.GetType().Name} can be used only on arrays or lists");

                MightyGUIUtilities.DrawPropertyField(property);
                return;
            }

            var options = GetOptionsForMember(serializedField, attribute);

            var decoratorAttributes = GetDecorationsForMember(serializedField, attribute);

            DrawArrayImpl(serializedField, attribute, options, decoratorAttributes, drawer, drawerAttribute);
        }

        protected abstract void DrawArrayImpl(MightySerializedField serializedField, T attribute, ArrayOption options,
            BaseArrayDecoratorAttribute[] decoratorAttributes, IArrayElementDrawer drawer, BasePropertyDrawerAttribute drawerAttribute);

        public ArrayOption GetOptionsForMember(BaseMightyMember mightyMember, BaseArrayAttribute attribute)
        {
            if (!m_optionsCache.Contains(mightyMember))
                EnableDrawer(mightyMember, attribute);

            return m_optionsCache[mightyMember].Value;
        }

        protected BaseArrayDecoratorAttribute[] GetDecorationsForMember(BaseMightyMember mightyMember, T attribute)
        {
            if (!m_decorationsCache.Contains(mightyMember))
                EnableDrawer(mightyMember, attribute);

            return m_decorationsCache[mightyMember];
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, T attribute)
        {
            var property = serializedField.Property;

            if (!serializedField.GetInfoFromMember<ArrayOption>(attribute.Target, attribute.OptionsCallback, out var optionsInfo))
                optionsInfo = new MightyInfo<ArrayOption>(attribute.Options);

            if (!serializedField.TryGetAttributes<BaseArrayDecoratorAttribute>(out var decoratorAttributes))
                decoratorAttributes = new BaseArrayDecoratorAttribute[0];

            m_optionsCache[serializedField] = optionsInfo;
            m_decorationsCache[serializedField] = decoratorAttributes;
        }

        protected override void ClearCache() => m_optionsCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_optionsCache.Contains(mightyMember) || !m_decorationsCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_optionsCache[mightyMember].RefreshValue();
        }
    }
}
#endif