using System;
#if UNITY_EDITOR
using EncryptionTool.Editor;
#endif
using UnityEngine;

namespace EncryptionTool
{
    [Serializable]
    public class EncryptionData
    {
        [SerializeField] private string _password;
        [SerializeField] private byte[] _keySalt;
        [SerializeField] private byte[] _ivSalt;

        public string Password => _password;
        public byte[] KeySalt => _keySalt;
        public byte[] IvSalt => _ivSalt;

#if UNITY_EDITOR
        public EncryptionData()
        {
        }

        public EncryptionData(EncryptionData clone)
        {
            Create(clone._password, clone._keySalt, clone._ivSalt);
        }

        public EncryptionData(string password, byte[] keySalt, byte[] ivSalt = null)
        {
            Create(password, keySalt, ivSalt);
        }

        public EncryptionData(SerializableEncryptionData serializableEncryptionData)
        {
            Create(serializableEncryptionData.Password, serializableEncryptionData.KeySalt.ToArray(),
                serializableEncryptionData.IvSalt.ToArray());
        }

        private void Create(string password, byte[] keySalt, byte[] ivSalt = null)
        {
            _password = password;
            _keySalt = keySalt;
            _ivSalt = ivSalt ?? keySalt;
        }

        public bool Compare(EncryptionData other) => other._password == _password &&
                                                     other._keySalt.CompareArrayItems(_keySalt) &&
                                                     other._ivSalt.CompareArrayItems(_ivSalt);

        public bool IsEmpty() => (_password.Length == 0 || _password.Equals(string.Empty)) && _keySalt.Length == 0 && _ivSalt.Length == 0;
#endif
    }
}