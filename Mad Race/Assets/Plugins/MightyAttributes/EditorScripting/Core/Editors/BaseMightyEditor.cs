#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public abstract class BaseMightyEditor : UnityEditor.Editor
    {
        protected MightyDrawer m_drawer;

        #region Unity Events

        private void OnEnable()
        {
            if (MightySettingsServices.Activated) Enable();
        }

        private void OnDisable()
        {
            if (MightySettingsServices.Activated) Disable();
        }

        public override void OnInspectorGUI()
        {
            try
            {
                if (MightySettingsServices.Activated) InspectorGUI();
                else base.OnInspectorGUI();
            }
            catch (Exception ex)
            {
                if (MightyExceptionUtilities.IsExitGUIException(ex)) return;

                if (ex is BaseAbortInspectorGUIException abortException)
                    abortException.OnInspectorAbort(m_drawer);
                else
                    Debug.LogException(ex);
            }
        }

        #endregion /Unity Events

        #region Core

        protected bool Enable(bool force = false)
        {
            try
            {
                if (!force)
                {
                    switch (target)
                    {
                        case MonoBehaviour monoBehaviour when Selection.activeObject != monoBehaviour.gameObject:
                        case ScriptableObject _ when Selection.activeObject != target:
                            return false;
                    }
                }

                m_drawer = new MightyDrawer();
                m_drawer.OnEnableMonoScript(target, serializedObject);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name != "SerializedObjectNotCreatableException") 
                    // This is a terrible fix, but it'll be enough until I find what's actually triggering this...
                    // It might have to do with additive scenes or prefabs
                    Debug.LogException(ex);

                return false;
            }

            return true;
        }

        protected void Disable()
        {
            m_drawer?.OnDisable();
            m_drawer = null;
            MightyDrawersDatabase.ClearCachesNotOfTypes(typeof(BaseHierarchyAttribute), typeof(BaseReloadScriptsAttribute));
        }

        protected void InspectorGUI()
        {
            if (m_drawer == null && !Enable() || !m_drawer.HasMightyMembers)
            {
                base.OnInspectorGUI();
                return;
            }

            m_drawer.BeginOnGUI();
            m_drawer.ManageMembers(out var valueChanged);

            if (valueChanged)
            {
                serializedObject.ManageValueChanged();
                m_drawer.ApplyAutoValues();
                m_drawer.RefreshAllDrawers();
                serializedObject.ManageValueChanged();
            }

            m_drawer.EndOnGUI();
        }

        #endregion /Core

        public void ApplyAutoValues()
        {
            if (!MightySettingsServices.Activated) return;

            if (m_drawer == null && !Enable(true)) return;
            m_drawer.ApplyAutoValues();
            Disable();
        }
    }
}
#endif