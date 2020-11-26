#if UNITY_EDITOR
using System;

namespace MightyAttributes.Editor
{
    public interface IClassDrawer : IMightyDrawer
    {
        void OnEnableClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute);
        void OnDisableClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute);
        
        void BeginDrawClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute);
        void EndDrawClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute);
    }

    public abstract class BaseClassDrawer<T> : BaseMightyDrawer<T> , IClassDrawer where T : BaseClassAttribute
    {
        public void OnEnableClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute) => 
            OnEnableClass(mightyType, (T) baseAttribute);

        public void OnDisableClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute) => 
            OnDisableClass(mightyType, (T) baseAttribute);

        public void BeginDrawClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute) => 
            BeginDrawClass(mightyType, (T) baseAttribute);

        public void EndDrawClass(MightyMember<Type> mightyType, BaseClassAttribute baseAttribute) => 
            EndDrawClass(mightyType, (T) baseAttribute);

        protected abstract void OnEnableClass(MightyMember<Type> mightyType, T attribute);
        protected abstract void OnDisableClass(MightyMember<Type> mightyType, T attribute);

        protected abstract void BeginDrawClass(MightyMember<Type> mightyType, T attribute);
        protected abstract void EndDrawClass(MightyMember<Type> mightyType, T attribute);
    }
}
#endif