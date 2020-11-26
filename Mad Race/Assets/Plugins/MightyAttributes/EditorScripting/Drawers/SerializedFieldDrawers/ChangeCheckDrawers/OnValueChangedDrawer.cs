#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class OnValueChangedDrawer : BaseChangeCheckDrawer<OnValueChangedAttribute>
    {
        private readonly CallbackSignature m_onValueChangedSignature = CallbackSignature.AnyTypeNoParams;

        private readonly MightyCache<(bool, MightyVoidMethod)> m_onValueChangedCache = new MightyCache<(bool, MightyVoidMethod)>();

        protected override void BeginChangeCheck(MightySerializedField mightyMember, OnValueChangedAttribute attribute)
        {
        }

        protected override void EndChangeCheck(bool changed, MightySerializedField mightyMember, OnValueChangedAttribute attribute)
        {
            if (!changed) return;

            if (!m_onValueChangedCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, onValueChangedCallback) = m_onValueChangedCache[mightyMember];
            if (valid)
            {
                mightyMember.Property.serializedObject.ApplyModifiedProperties();
                onValueChangedCallback.Invoke();
            }
            else
                MightyGUIUtilities.DrawHelpBox(
                    $"Callback is invalid, it should be like this: \"[Any Type] {attribute.ValueChangedCallback}()\"");
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, OnValueChangedAttribute attribute)
        {
            var valid = serializedField.GetMightyVoidMethod(attribute.Target, attribute.ValueChangedCallback, m_onValueChangedSignature,
                out var onValueChangedCallback);

            m_onValueChangedCache[serializedField] = (valid, onValueChangedCallback);
        }

        protected override void ClearCache() => m_onValueChangedCache.ClearCache();
    }
}
#endif