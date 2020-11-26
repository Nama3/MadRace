#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public abstract class BaseMightyException : Exception
    {
    }

    public abstract class BaseAbortInspectorGUIException : BaseMightyException
    {
        public abstract void OnInspectorAbort(MightyDrawer drawer);
    }

    public class AbortAfterConfirmDialogException : BaseAbortInspectorGUIException
    {
        public override void OnInspectorAbort(MightyDrawer drawer)
        {
            if (MightyEditorUtilities.HasEditorChanged() && drawer != null)
            {
                drawer.ApplyAutoValues();
                drawer.RefreshAllDrawers();
            }

            GUILayout.BeginVertical();
        }
    }

    public class AbortAfterReorderMembersException : BaseAbortInspectorGUIException
    {
        public override void OnInspectorAbort(MightyDrawer drawer)
        {
            if (!MightyEditorUtilities.HasEditorChanged() || drawer == null) return;
            
            drawer.ApplyAutoValues();
            drawer.RefreshAllDrawers();
        }
    }

    public static class MightyExceptionUtilities
    {
        public static readonly AbortAfterConfirmDialogException AbortAfterConfirmDialog = new AbortAfterConfirmDialogException();
        public static readonly AbortAfterReorderMembersException AbortAfterReorderMembers = new AbortAfterReorderMembersException();

        public static bool IsExitGUIException(Exception exception)
        {
            while (exception is TargetInvocationException && exception.InnerException != null) 
                exception = exception.InnerException;
            
            return exception is ExitGUIException;
        }
    }
}
#endif