using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Phobebase.Common
{
    public class Serialization
	{
		#region Binary File Save System
		
		public static bool SaveToBinaryFile(string saveSubPath, string saveName, object saveData)
		{
			saveSubPath = (saveSubPath[0] == '/') ? saveSubPath : "/" + saveSubPath;
			saveSubPath = (saveSubPath[saveSubPath.Length - 1] == '/') ? saveSubPath : saveSubPath + "/";
			string path = Application.persistentDataPath + saveSubPath;

			#if UNITY_EDITOR

				path += "dev/";

			#endif

			BinaryFormatter formatter = GetBinaryFormatter();

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			FileStream file = File.Create(path + saveName);

			formatter.Serialize(file, saveData);

			file.Close();

			return true;
		}

		public static object LoadBinaryFile(string folderPath, string filename)
		{
			folderPath = (folderPath[0] == '/') ? folderPath : "/" + folderPath;
			folderPath = (folderPath[folderPath.Length - 1] == '/') ? folderPath : folderPath + "/";
			string path = Application.persistentDataPath + folderPath;

			#if UNITY_EDITOR

				path += "dev/";

			#endif

			path += filename;

			if (!File.Exists(path)) return null;

			BinaryFormatter formatter = GetBinaryFormatter();

			FileStream file = File.Open(path, FileMode.Open);

			try
			{
				object save = formatter.Deserialize(file);
				file.Close();
				return save;
			}
			catch
			{
				Debug.LogErrorFormat("failed to load file at {0}", path);
				file.Close();
				return null;
			}
		}

		public static BinaryFormatter GetBinaryFormatter()
		{
			BinaryFormatter formatter = new BinaryFormatter();

			return formatter;
		}
		#endregion

		#region Json File Save System

		public static bool SaveToJsonFile(string saveSubPath, string saveName, object saveData)
		{
			saveSubPath = (saveSubPath[0] == '/') ? saveSubPath : "/" + saveSubPath;
			saveSubPath = (saveSubPath[saveSubPath.Length - 1] == '/') ? saveSubPath : saveSubPath + "/";
			string path = Application.persistentDataPath + saveSubPath;

			#if UNITY_EDITOR

				path += "dev/";

			#endif

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			string json = JsonUtility.ToJson(saveData, true);

			File.WriteAllText(path + saveName, json);

			return true;
		}

		public static T LoadJsonFile<T>(string folderPath, string filename)
		{
			folderPath = (folderPath[0] == '/') ? folderPath : "/" + folderPath;
			folderPath = (folderPath[folderPath.Length - 1] == '/') ? folderPath : folderPath + "/";
			string path = Application.persistentDataPath + folderPath;

			#if UNITY_EDITOR

				path += "dev/";

			#endif

			path += filename;

			if (!File.Exists(path)) return default(T);

			string json = File.ReadAllText(path);

			return JsonUtility.FromJson<T>(json);
		}
		#endregion

		#region Tamper Resistent Json File Save System

		public static bool SaveToProtectedJsonFile(string saveSubPath, string saveName, object saveData, string key = "J34$%GWJ68#DW")
		{
			saveSubPath = (saveSubPath[0] == '/') ? saveSubPath : "/" + saveSubPath;
			saveSubPath = (saveSubPath[saveSubPath.Length - 1] == '/') ? saveSubPath : saveSubPath + "/";
			string path = Application.persistentDataPath + saveSubPath;

			#if UNITY_EDITOR

				path += "dev/";

			#endif

			if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			byte[] saveDataBytes = Encoding.UTF8.GetBytes(EncryptDecrypt(JsonUtility.ToJson(saveData, false), key));
			byte[] data = Hashing.Hash(saveDataBytes);

			Debug.Log($"Hash: {Hashing.GetHexStringFromHash(data)}, Data: {Hashing.GetHexStringFromHash(saveDataBytes)}");

			data = data.Concat(saveDataBytes);

			File.WriteAllBytes(path + saveName, data);

			return true;
		}

		public static T LoadProtectedJsonFile<T>(string folderPath, string filename, string key = "J34$%GWJ68#DW")
		{
			folderPath = (folderPath[0] == '/') ? folderPath : "/" + folderPath;
			folderPath = (folderPath[folderPath.Length - 1] == '/') ? folderPath : folderPath + "/";
			string path = Application.persistentDataPath + folderPath;

			#if UNITY_EDITOR

				path += "dev/";

			#endif

			path += filename;

			if (!File.Exists(path)) return default(T);

			byte[] data = File.ReadAllBytes(path);

			Debug.Log(data.Length);

			byte[] savedhash = data[0..32];
			data = data[32..];

			byte[] hash = Hashing.Hash(data);

			Debug.Log($"SavedHash: {Hashing.GetHexStringFromHash(savedhash)}, Data: {Hashing.GetHexStringFromHash(data)}");
			Debug.Log($"New Hash:  {Hashing.GetHexStringFromHash(hash)}");

			if (!savedhash.SequenceEqual(hash)) return default(T);

			return JsonUtility.FromJson<T>(EncryptDecrypt(Encoding.UTF8.GetString(data), key));
		}

		public static string EncryptDecrypt(string value, string key)
		{
			string result = "";

			for (int i = 0; i < value.Length; i++)
			{
				result += (char)(value[i] ^ key[i % key.Length]);
			}

			return result;
		}
		#endregion
	}
}