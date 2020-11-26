using System.Collections.Generic;
#if UNITY_EDITOR
using EncryptionTool.Editor;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
#endif
using UnityEngine;

namespace EncryptionTool
{
    public class EncryptionInitializer : MonoBehaviour
    {
#if UNITY_EDITOR
        private const string FileName = "EncryptionBackup.xml";
#endif
        [SerializeField] private int _passwordSize = 10, _saltSize = 16;
        [SerializeField] private bool _keyAndIvIdentical;
        [SerializeField] private EncryptionData _encryptionData;

        public byte[] KeySalt => _encryptionData.KeySalt;
        public byte[] IvSalt => _encryptionData.IvSalt;
        public string Password => _encryptionData.Password;

#if UNITY_EDITOR
        private readonly List<EncryptionData> m_backupEncryptionData = new List<EncryptionData>();
        private SerializableEncryptionDataBackup m_serializedBackup;

        #region Editor

        private bool m_unfoldBackup;

        private void OnValidate()
        {
            if (_saltSize < 8) _saltSize = 8;
            if (_passwordSize < 8) _passwordSize = 8;
            AddCurrentToBackup();
        }

        private void AddCurrentToBackup()
        {
            if (m_serializedBackup != null && _encryptionData != null && !_encryptionData.IsEmpty() &&
                !m_backupEncryptionData.Any(x => x.Compare(_encryptionData)))
            {
                m_backupEncryptionData.Add(new EncryptionData(_encryptionData));
                m_serializedBackup.RestoreIndex = m_backupEncryptionData.Count - 1;
            }

            RemoveDuplicatesBackup();
            SyncWithFile();
        }

        private void RemoveDuplicatesBackup()
        {
            if (m_backupEncryptionData == null) return;

            var index = 0;
            while (true)
            {
                if (index >= m_backupEncryptionData.Count) return;
                var duplicateCount = 0;
                foreach (var backup in m_backupEncryptionData)
                    if (backup.Compare(m_backupEncryptionData[index]))
                        duplicateCount++;
                if (duplicateCount > 1)
                    m_backupEncryptionData.RemoveAt(index);
                else
                    index++;
            }
        }

        private void Generate()
        {
            if (!_encryptionData.IsEmpty() &&
                !EditorUtility.DisplayDialog("Generate Encryption Data", "Are you sure to overwrite current Encryption Data?",
                    "Overwrite", "Cancel"))
                return;
            AddCurrentToBackup();
            _encryptionData = new EncryptionData(EncryptionHelpers.GenerateString(_passwordSize),
                EncryptionHelpers.GenerateByteArray(_saltSize),
                _keyAndIvIdentical ? null : EncryptionHelpers.GenerateByteArray(_saltSize));
            AddCurrentToBackup();
        }

        private void RestorePrevious() => NavigateRestore(false);

        private void RestoreNext() => NavigateRestore(true);

        private void NavigateRestore(bool next)
        {
            if (m_backupEncryptionData == null || m_backupEncryptionData.Count == 0) return;
            if (m_serializedBackup.RestoreIndex > m_backupEncryptionData.Count - 1)
                m_serializedBackup.RestoreIndex = m_backupEncryptionData.Count - 1;

            if (_encryptionData.Compare(m_backupEncryptionData[m_serializedBackup.RestoreIndex]))
            {
                if (next)
                {
                    if (m_serializedBackup.RestoreIndex < m_backupEncryptionData.Count - 1)
                        _encryptionData = m_backupEncryptionData[++m_serializedBackup.RestoreIndex];
                    SyncWithFile();
                    return;
                }

                if (m_serializedBackup.RestoreIndex > 0)
                    _encryptionData = m_backupEncryptionData[--m_serializedBackup.RestoreIndex];
                SyncWithFile();
                return;
            }

            _encryptionData = m_backupEncryptionData[m_serializedBackup.RestoreIndex];

            if (next && m_serializedBackup.RestoreIndex < m_backupEncryptionData.Count - 1)
                m_serializedBackup.RestoreIndex++;
            else if (m_serializedBackup.RestoreIndex > 0)
                m_serializedBackup.RestoreIndex--;

            SyncWithFile();
        }

        private void SyncWithFile()
        {
            var script = MonoScript.FromMonoBehaviour(this);
            var dirInfo = new DirectoryInfo(AssetDatabase.GetAssetPath(script));
            var path = dirInfo.Parent?.Parent?.FullName + "\\" + FileName;
            if (File.Exists(path))
            {
                m_serializedBackup = File.ReadAllText(path).XmlDeserialize<SerializableEncryptionDataBackup>();
                MergeBackups(m_backupEncryptionData, m_serializedBackup.BackupEncryptionData);
                if (!_encryptionData.IsEmpty())
                    m_serializedBackup.EncryptionData = new SerializableEncryptionData(_encryptionData);
                else if (!m_serializedBackup.EncryptionData.IsEmpty())
                    _encryptionData = new EncryptionData(m_serializedBackup.EncryptionData);
                File.WriteAllText(path, m_serializedBackup.XmlSerialize());
            }
            else
            {
                m_serializedBackup = new SerializableEncryptionDataBackup(_encryptionData, m_backupEncryptionData);
                File.WriteAllText(path, m_serializedBackup.XmlSerialize());
            }
        }

        private void MergeBackups([NotNull] List<EncryptionData> localBackup, [NotNull] List<SerializableEncryptionData> serializedBackup)
        {
            var localDifferences = new List<EncryptionData>();
            var serializedDifferences = new List<SerializableEncryptionData>();
            foreach (var backup in localBackup)
                if (!serializedBackup.Any(x => x.Compare(backup)))
                    localDifferences.Add(backup);
            foreach (var backup in serializedBackup)
                if (!localBackup.Any(x => backup.Compare(x)))
                    serializedDifferences.Add(backup);
            foreach (var difference in localDifferences)
                serializedBackup.Add(new SerializableEncryptionData(difference));
            foreach (var difference in serializedDifferences)
                localBackup.Add(new EncryptionData(difference));
        }

        [CustomEditor(typeof(EncryptionInitializer))]
        public class EncryptionInitializerInspector : UnityEditor.Editor
        {
            private EncryptionInitializer m_encryptionInitializer;

            private void OnEnable()
            {
                m_encryptionInitializer = (EncryptionInitializer) target;
                m_encryptionInitializer.SyncWithFile();
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                EditorGUILayout.Space();

                if (GUILayout.Button("Generate Encryption Data"))
                    m_encryptionInitializer.Generate();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();

                GUILayout.Label("Restore Backup:");
                if (GUILayout.Button("Previous", GUILayout.Width(100)))
                    m_encryptionInitializer.RestorePrevious();

                if (GUILayout.Button("Next", GUILayout.Width(100)))
                    m_encryptionInitializer.RestoreNext();

                GUILayout.Label($"Index: {m_encryptionInitializer.m_serializedBackup.RestoreIndex}");

                GUILayout.EndHorizontal();

                if (m_encryptionInitializer.m_unfoldBackup =
                    EditorGUILayout.Foldout(m_encryptionInitializer.m_unfoldBackup, "Backup Encryption Data", true) &&
                    m_encryptionInitializer.m_backupEncryptionData != null)
                    foreach (var backup in m_encryptionInitializer.m_backupEncryptionData)
                        GUILayout.Label(backup.Password);

                var script = MonoScript.FromMonoBehaviour(m_encryptionInitializer);
                var dirInfo = new DirectoryInfo(AssetDatabase.GetAssetPath(script));
                var path = dirInfo.Parent?.Parent?.FullName + "\\" + FileName;
                GUI.enabled = false;
                EditorGUILayout.LabelField(path);
                GUI.enabled = true;
            }
        }

        #endregion /Editor

#endif
    }
}