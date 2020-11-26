#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace EncryptionTool.Editor
{
    [Serializable]
    public class SerializableEncryptionDataBackup
    {
        public int RestoreIndex { get; set; }
        
        public SerializableEncryptionData EncryptionData { get; set; }

        public List<SerializableEncryptionData> BackupEncryptionData  { get; set; }

        public SerializableEncryptionDataBackup()
        {
        }

        public SerializableEncryptionDataBackup(EncryptionData encryptionData, List<EncryptionData> backupEncryptionData)
        {
            EncryptionData = new SerializableEncryptionData(encryptionData);
            BackupEncryptionData = new List<SerializableEncryptionData>();
            foreach (var backup in backupEncryptionData)
                BackupEncryptionData.Add(new SerializableEncryptionData(backup));
        }
    }
}
#endif