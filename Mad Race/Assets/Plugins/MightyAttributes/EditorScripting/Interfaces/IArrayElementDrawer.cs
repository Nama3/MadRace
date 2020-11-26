#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IArrayElementDrawer
    {
        void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute);

        void DrawElement(GUIContent label, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute);

        void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute);

        float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute);
    }
}
#endif