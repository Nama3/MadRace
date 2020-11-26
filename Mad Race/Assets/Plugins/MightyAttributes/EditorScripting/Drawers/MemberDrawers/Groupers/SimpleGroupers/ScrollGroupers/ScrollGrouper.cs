#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ScrollGrouper : BaseScrollGrouper<SimpleScrollGroupAttribute>
    {
        public override GUIStyle GetGroupStyle(int indentLevel) => MightyStyleUtilities.GetBox(indentLevel);
    }
}
#endif