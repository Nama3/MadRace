#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class ValidIfPropertyValidator : BaseValidatorDrawer<ValidIfAttribute>, IRefreshDrawer
    {
        private readonly CallbackSignature m_validateCallbackSignature = new CallbackSignature(typeof(bool));

        private readonly MightyCache<(bool, MightyMethod<bool>, MightyInfo<object>)> m_validateInputCache =
            new MightyCache<(bool, MightyMethod<bool>, MightyInfo<object>)>();

        protected override void ValidateProperty(MightySerializedField serializedField, ValidIfAttribute ifAttribute)
        {
            var property = serializedField.Property;

            if (!m_validateInputCache.Contains(serializedField)) EnableDrawer(serializedField, ifAttribute);
            var (valid, validateCallback, propertyFieldInfo) = m_validateInputCache[serializedField];

            if (valid)
            {
                if (!validateCallback.Invoke(propertyFieldInfo.Value))
                    MightyGUIUtilities.DrawHelpBox(string.IsNullOrEmpty(ifAttribute.Message)
                        ? $"{property.name} is not valid"
                        : ifAttribute.Message, MessageType.Error, property.GetTargetObject(), ifAttribute.LogToConsole);
            }
            else
                MightyGUIUtilities.DrawHelpBox(
                    $@"{nameof(ValidIfAttribute)
                        } needs a callback with boolean return type and a single parameter of type {
                            m_validateCallbackSignature.ParamTypes[0].Name}");
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, ValidIfAttribute ifAttribute)
        {
            var target = ifAttribute.Target;
            var property = serializedField.Property;

            var type = property.IsCollection() ? serializedField.PropertyType.MakeArrayType() : serializedField.PropertyType;

            m_validateCallbackSignature.SetParamsType(type);

            var valid = serializedField.GetMightyMethod<bool>(target, ifAttribute.ConditionCallback, m_validateCallbackSignature,
                out var validateCallback);

            MightyInfo<object> propertyFieldInfo = null;
            valid = valid && serializedField.GetInfoFromMember(target, property.name, out propertyFieldInfo);

            m_validateInputCache[serializedField] = (valid, validateCallback, propertyFieldInfo);
        }

        protected override void ClearCache() => m_validateInputCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_validateInputCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, propertyFieldInfo) = m_validateInputCache[mightyMember];

            if (valid) propertyFieldInfo.RefreshValue();
        }
    }
}
#endif