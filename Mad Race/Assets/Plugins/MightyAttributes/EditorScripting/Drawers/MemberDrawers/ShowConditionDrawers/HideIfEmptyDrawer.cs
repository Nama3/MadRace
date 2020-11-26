#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class HideIfEmptyDrawer : BaseShowConditionDrawer<HideIfEmptyAttribute>
    {
        protected override bool CanDraw(BaseMightyMember mightyMember, HideIfEmptyAttribute attribute) =>
            !(mightyMember is MightySerializedField serializedField) || 
            !serializedField.Property.IsCollection() || serializedField.ArraySize != 0;

        protected override void Enable(BaseMightyMember mightyMember, HideIfEmptyAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif