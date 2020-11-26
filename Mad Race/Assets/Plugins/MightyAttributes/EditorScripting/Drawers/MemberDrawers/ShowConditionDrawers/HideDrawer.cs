#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class HideDrawer : BaseShowConditionDrawer<HideAttribute>
    {
        protected override bool CanDraw(BaseMightyMember mightyMember, HideAttribute attribute) => false;
        
        protected override void Enable(BaseMightyMember mightyMember, HideAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif