#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace EncryptionTool.Editor
{
	[Serializable]
	public class SerializableEncryptionData 
	{
		public string Password { get; set; }
		public List<byte> KeySalt { get; set; }
		public List<byte> IvSalt { get; set; }

		public SerializableEncryptionData()
		{
		
		}

		public SerializableEncryptionData(EncryptionData encryptionData)
		{
			Password = encryptionData.Password;
			KeySalt = encryptionData.KeySalt.ToList();
			IvSalt = encryptionData.IvSalt.ToList();
		}
	
		public bool Compare(EncryptionData other) => other.Password == Password &&
		                                             other.KeySalt.CompareArrayItems(KeySalt.ToArray()) &&
		                                             other.IvSalt.CompareArrayItems(IvSalt.ToArray());

		public bool IsEmpty() => Password.Length == 0 && KeySalt.Count == 0 && IvSalt.Count == 0;
	}
}
#endif
