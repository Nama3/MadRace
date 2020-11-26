#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class DarkScrollGrouper : BaseScrollGrouper<DarkSimpleScrollGroupAttribute>
    {
        public override GUIStyle GetGroupStyle(int indentLevel) => MightyStyleUtilities.GetDarkBox(indentLevel);
    }
}
#endif