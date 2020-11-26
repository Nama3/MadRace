#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class MightyDebugUtilities
    {
        public enum LogType
        {
            AutoValues,    
            ReloadScripts,
        }
        
        public static void MightyDebug(string message, LogType logType, MessageType type = MessageType.Info, Object context = null)
        {
            if (!MightySettingsServices.CanDisplayLog(logType)) return;
            
            message = $"[Mighty]Attributes - {message}";

            switch (type)
            {
                case MessageType.None:
                case MessageType.Info:
                    Debug.Log(message, context);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message, context);
                    break;
                case MessageType.Error:
                    Debug.LogError(message, context);
                    break;
            }
        }
        
        public static void MightyDebug(string message, MessageType type = MessageType.Info, Object context = null)
        {
            message = $"[Mighty]Attributes - {message}";

            switch (type)
            {
                case MessageType.None:
                case MessageType.Info:
                    Debug.Log(message, context);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message, context);
                    break;
                case MessageType.Error:
                    Debug.LogError(message, context);
                    break;
            }
        }
    }
}
#endif