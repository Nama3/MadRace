using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionTool
{
    public class Encryption
    {
        private static readonly Encryption Instance = new Encryption();

        #region Data

        private bool m_isInit;
        private RijndaelManaged m_rijndael;
        private ICryptoTransform m_encryptor;
        private ICryptoTransform m_decryptor;

        private void InitInstance(string password, byte[] keySalt, byte[] ivSalt)
        {
            if (m_isInit || m_rijndael != null && m_encryptor != null && m_decryptor != null) return;
            if (ivSalt == null)
                ivSalt = keySalt;

            m_rijndael = new RijndaelManaged
            {
                Key = CreateKey(password, keySalt),
                IV = CreateIv(password, ivSalt),
                Padding = PaddingMode.PKCS7
            };
            m_decryptor = m_rijndael.CreateDecryptor();
            m_encryptor = m_rijndael.CreateEncryptor();
            m_isInit = true;
        }

        private void ResetInstance()
        {
            m_isInit = false;
            m_rijndael = null;
            m_decryptor = null;
            m_encryptor = null;
        }

        #endregion /Data

        #region Statics

        public static bool IsInit => Instance.m_isInit;

        public static void Init(EncryptionInitializer encryptionInitializer) =>
            Init(encryptionInitializer.Password, encryptionInitializer.KeySalt, encryptionInitializer.IvSalt);

        public static void Init(string password, byte[] keySalt, byte[] ivSalt) => Instance.InitInstance(password, keySalt, ivSalt);

        public static void Reset() => Instance.ResetInstance();

        public static string Encrypt(string toEncrypt)
        {
            if (!IsInit) return toEncrypt;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            var resultArray = Instance.m_encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string toDecrypt)
        {
            if (!IsInit) return toDecrypt;
            var toDecryptArray = Convert.FromBase64String(toDecrypt);
            var resultArray = Instance.m_decryptor.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        public static byte[] CreateKey(string password, byte[] salt) => new Rfc2898DeriveBytes(password, salt).GetBytes(32);

        public static byte[] CreateIv(string password, byte[] salt) => new Rfc2898DeriveBytes(password, salt).GetBytes(16);

        #endregion /Statics
    }
}