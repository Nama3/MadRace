#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IDrawAnywhereDecorator
    {
        void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute);
        void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute);
    }
}
#endif