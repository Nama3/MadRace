#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public static class SessionStateUtilities
    {
        public static bool GetBoolOnce(string key, bool defaultValue = false)
        {
            if (SessionState.GetBool(key, defaultValue)) return true;
            SessionState.SetBool(key, !defaultValue);
            return false;
        }
    }
}
#endif