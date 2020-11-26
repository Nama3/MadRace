#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true)]
    internal class ScriptableObjectEditor : BaseMightyEditor
    {
    }
}
#endif