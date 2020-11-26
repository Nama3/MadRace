#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Random = UnityEngine.Random;

namespace EncryptionTool.Editor
{
	public static class EncryptionHelpers {

		public static bool CompareArrayItems<T>(this T[] source, T[] other) =>
			source.Length == other.Length && !source.Where((t, i) => !t.Equals(other[i])).Any();
    
		public static string XmlSerialize<T>(this T obj)
		{
			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(stream, Encoding.UTF8))
			{
				new XmlSerializer(typeof(T)).Serialize(writer, obj);
				return  Encoding.UTF8.GetString(stream.ToArray());
			}
		}

		public static T XmlDeserialize<T>(this string serialized)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(serialized)))
			using (new StreamReader(stream, Encoding.UTF8))
				return (T) new XmlSerializer(typeof(T)).Deserialize(stream);
		}
	
		public static T[] Shuffled<T>(this T[] array, int? seed = null)
		{
			var newArray = new T[array.Length];
			Array.Copy(array, newArray, array.Length);
			newArray.Shuffle(seed);
			return newArray;
		}
		
		public static void Shuffle<T>(this T[] array, int? seed = null)
		{
			if (seed != null)
				Random.InitState((int) seed);
			for (var i = 0; i < array.Length; i++)
				Swap(ref array[i], ref array[Random.Range(i, array.Length)]);
		}
		
		public static void Swap<T>(ref T item1, ref T item2)
		{
			var tmp = item1;
			item1 = item2;
			item2 = tmp;
		}
		
		public static byte GenerateByte(byte min = byte.MinValue, byte max = byte.MaxValue) => (byte) Random.Range(min, max);

		public static byte[] GenerateByteArray(int size, byte min = byte.MinValue, byte max = byte.MaxValue)
		{
			var byteArray = new byte[size];
			for (var i = 0; i < size; i++)
				byteArray[i] = GenerateByte(min, max);
			return byteArray;
		}

		public static string GenerateString(int size)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var shuffledChars = chars.ToCharArray().Shuffled();
			return new string(Enumerable.Repeat(shuffledChars, size).Select(s => s[Random.Range(0, shuffledChars.Length)]).ToArray());
		}
	}
}
#endif
