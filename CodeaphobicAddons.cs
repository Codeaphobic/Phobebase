using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Codeaphobic
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		public static T instance { get; private set; }

		protected virtual void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(this);
				return;
			}
			instance = (T)this;
		}
	}

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

	namespace Curves
	{
		public class BezierCurve2
		{
			public Vector3 point1;
			public Vector3 point2;
			public Vector3 weight1;

			public BezierCurve2(Vector3 point1, Vector3 point2, Vector3 weight1)
			{
				this.point1 = point1;
				this.point2 = point2;
				this.weight1 = weight1;
			}

			public Vector3 PointOnCurve(float t)
			{
				return weight1 + (1f - t) * (1f - t) * (point1 - weight1) + t * t * (point2 - weight1);
			}
		}

		public class BezierCurve3
		{
			public Vector3 point1;
			public Vector3 point2;
			public Vector3 weight1;
			public Vector3 weight2;

			public BezierCurve3(Vector3 point1, Vector3 point2, Vector3 weight1, Vector3 weight2)
			{
				this.point1 = point1;
				this.point2 = point2;
				this.weight1 = weight1;
				this.weight2 = weight2;
			}

			public Vector3 PointOnCurve(float t)
			{
				return (1f - t) * (1f - t) * (1f - t) * point1 + 3 * ((1f - t) * (1f - t)) * t
					* weight1 + 3 * (1f - t) * (t * t) * weight2 + t * t * t * point2;
			}
		}
	}

	public class GPUCompute
	{
		#region Variables
		private ComputeShader m_shader;
		public ComputeShader shader
		{
			get { return m_shader; }
			private set { m_shader = value; }
		}

		private Dictionary<string, ComputeBuffer> m_buffers = new Dictionary<string, ComputeBuffer>();
		public Dictionary<string, ComputeBuffer> buffers
		{
			get { return m_buffers; }
			private set { m_buffers = value; }
		}

		public GPUCompute Create(ComputeShader shader)
		{
			this.shader = shader;
			return this;
		}
		#endregion
		#region Add Returnable Data

		public GPUCompute AddTexture(ref RenderTexture texture, string textureName)
		{
			shader.SetTexture(0, textureName, texture);
			return this;
		}

		public GPUCompute CreateTexture(string textureName, Vector2Int dimensions, int depth, out RenderTexture texture)
		{
			texture = new RenderTexture(dimensions.x, dimensions.y, depth);
			texture.enableRandomWrite = true;
			texture.Create();

			shader.SetTexture(0, textureName, texture);
			return this;
		}

		public GPUCompute AddTexture(ref RenderTexture texture, string textureName, int bufferLocation)
		{
			shader.SetTexture(bufferLocation, textureName, texture);
			return this;
		}

		public GPUCompute CreateTexture(string textureName, Vector2Int dimensions, int depth, int bufferLocation, out RenderTexture texture)
		{
			texture = new RenderTexture(dimensions.x, dimensions.y, depth);
			texture.enableRandomWrite = true;
			texture.Create();

			shader.SetTexture(bufferLocation, textureName, texture);
			return this;
		}

		public GPUCompute AddBuffer<T>(string name, ref T[] bufferObjects, int bufferObjectSize)
		{
			ComputeBuffer buffer = new ComputeBuffer(bufferObjects.Length, bufferObjectSize);
			buffer.SetData(bufferObjects);
			buffers.Add(name, buffer);

			shader.SetBuffer(0, name, buffer);
			return this;
		}

		public GPUCompute AddBuffer<T>(string name, ref T[] bufferObjects, int bufferObjectSize, int bufferLocation)
		{
			ComputeBuffer buffer = new ComputeBuffer(bufferObjects.Length, bufferObjectSize);
			buffer.SetData(bufferObjects);
			buffers.Add(name, buffer);

			shader.SetBuffer(bufferLocation, name, buffer);
			return this;
		}

		#endregion
		#region Adding Data

		public GPUCompute AddFloat(string name, float value)
		{
			shader.SetFloat(name, value);
			return this;
		}

		public GPUCompute AddFloats(string name, float[] values)
		{
			shader.SetFloats(name, values);
			return this;
		}

		public GPUCompute AddInt(string name, int value)
		{
			shader.SetInt(name, value);
			return this;
		}

		public GPUCompute AddInts(string name, int[] values)
		{
			shader.SetInts(name, values);
			return this;
		}

		public GPUCompute AddVector(string name, Vector4 value)
		{
			shader.SetVector(name, value);
			return this;
		}

		public GPUCompute AddVectorArray(string name, Vector4[] values)
		{
			shader.SetVectorArray(name, values);
			return this;
		}

		public GPUCompute AddMatrix(string name, Matrix4x4 value)
		{
			shader.SetMatrix(name, value);
			return this;
		}

		public GPUCompute AddMatrixArray(string name, Matrix4x4[] values)
		{
			shader.SetMatrixArray(name, values);
			return this;
		}

		public GPUCompute AddBool(string name, bool value)
		{
			shader.SetBool(name, value);
			return this;
		}

		#endregion
		#region Retreive Data

		public GPUCompute Compute(Vector2Int parallelizationAxis)
		{
			shader.Dispatch(0, parallelizationAxis.x, parallelizationAxis.y, 1);
			return this;
		}

		public GPUCompute GetData<T>(string name, ref T[] bufferObjects)
		{
			buffers[name].GetData(bufferObjects);
			return this;
		}

		#endregion
		#region Dispose Data
		public GPUCompute DisposeBuffer(string name)
		{
			buffers[name].Dispose();
			buffers.Remove(name);
			return this;
		}

		public GPUCompute DisposeBuffers()
		{
			foreach (ComputeBuffer buffer in buffers.Values)
			{
				buffer.Dispose();
			}
			buffers.Clear();
			return this;
		}
		#endregion
	}

	public static class Extensions
	{
		public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
		{
			float multiplier = 1;
			for (int i = 0; i < decimalPlaces; i++)
			{
				multiplier *= 10f;
			}
			return new Vector3(
				Mathf.Round(vector3.x * multiplier) / multiplier,
				Mathf.Round(vector3.y * multiplier) / multiplier,
				Mathf.Round(vector3.z * multiplier) / multiplier);
		}

		public static Vector2 Round(this Vector2 vector2, int decimalPlaces = 2)
		{
			float multiplier = 1;
			for (int i = 0; i < decimalPlaces; i++)
			{
				multiplier *= 10f;
			}
			return new Vector2(
				Mathf.Round(vector2.x * multiplier) / multiplier,
				Mathf.Round(vector2.y * multiplier) / multiplier);
		}

		public static T[] Concat<T>(this T[] first, T[] second)
		{
			if (first == null)
			{
				return second;
			}
			if (second == null)
			{
				return first;
			}

			T[] result = new T[first.Length + second.Length];
			first.CopyTo(result, 0);
			second.CopyTo(result, first.Length);

			return result;
		}
	}

	public class Hashing
	{
		public static byte[] Hash(string data)
		{
			byte[] byteData = Encoding.UTF8.GetBytes(data);
			SHA256Managed hasher = new SHA256Managed();

			return hasher.ComputeHash(byteData);
		}

		public static byte[] Hash(byte[] data)
		{
			SHA256Managed hasher = new SHA256Managed();

			return hasher.ComputeHash(data);
		}

		public static string GetHexStringFromHash(byte[] hash)
		{
			string hexString = "";

			foreach (byte b in hash) hexString += b.ToString("x2");

			return hexString;
		}
	}

	namespace Threading
	{
		public struct ThreadResult
		{
			public bool complete { get; set; }
			public object result { get; set; }

			public ThreadResult(bool complete, object result)
			{
				this.complete = complete;
				this.result = result;
			}
		}
	}
}