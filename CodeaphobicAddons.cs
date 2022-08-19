using System;
using System.Collections;
using UnityEngine;

namespace Codeaphobic
{
    public class Singleton : MonoBehaviour 
    {
        public static Singleton instance { get; protected set; }

        private virtual void Awake() {
            if (Singleton.instance != null && instance != this)
            {
                Destory(this);
            }
            else 
            {
                instance = this;
            }
        }
    }

    public class Serialization 
    {
        public static bool SaveToFile(string saveSubPath, object saveData)
        {
            saveSubPath = (saveSubPath.Substring()[0] == "/") ? saveSubPath : "/" + saveSubPath;

            string path = Application.persistentDataPath + saveSubPath;

            BinaryFormatter formatter = GetBinaryFormatter();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileStream file = File.Create(path);

            formatter.Serialize(file, saveData);

            file.Close();

            return true;
        }

        public static object Load(string path)
        {
            if (!File.Exists(path)) { return null; }

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
    }

	public class Curves 
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

			public Vector3 PointOnCurve(float time)
			{
				return weight1 + (1f - time) * (1f - time) * (point1 - weight1) + time * time * (point2 - weight1);
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

			public Vector3 PointOnCurve(float time)
			{
				return (1f - time) * (1f - time) * (1f - time) * point1 + 3 * ((1f - time) * (1f - time)) * time * weight1 + 3 * (1f - time) * (time * time) * weight2 + time * time * time * point2;
			}
		}
	}

	public class GPUCompute
	{
		private ComputeShader m_shader;
		public ComputeShader shader {
			get { return m_shader; }
			private set { m_shader = value; }
		}

		private Dictionary<string, ComputeBuffer> m_buffers = new List<ComputeBuffer>();
		public Dictionary<string, ComputeBuffer> buffers {
			get { return m_buffers; }
			private set { m_buffers = value; }
		}

		private int m_bufferLocation = 0;

		public static GPUCompute Create(ComputeShader shader)
		{
			this.shader = shader
			return this;
		}

		#region Add Returnable Data

		public GPUCompute AddTexture(ref RenderTexture texture, string textureName)
		{
			shader.SetTexture(bufferLocation, textureName, texture);
			return this;
		}

		public GPUCompute CreateTexture(string textureName, Vector2 dimensions, int depth, out RenderTexture texture)
		{
			texture = new RenderTexture(dimensions.x, dimensions.y, depth);
			texture.enableRandomWrite = true;
			texture.Create();

			shader.SetTexture(bufferLocation, textureName, texture);
			return this;
		}

		public GPUCompute AddTexture(ref RenderTexture texture, string textureName, int bufferLocation)
		{
			shader.SetTexture(bufferLocation, textureName, texture);
			return this;
		}

		public GPUCompute CreateTexture(string textureName, Vector2 dimensions, int depth, int bufferLocation, out RenderTexture texture)
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
			texture.SetFloat(name, value);
			return this;
		}

		public GPUCompute AddFloats(string name, float[] values)
		{
			texture.SetFloats(name, values);
			return this;
		}

		public GPUCompute AddInt(string name, int value)
		{
			texture.SetInt(name, value);
			return this;
		}

		public GPUCompute AddInts(string name, int[] values)
		{
			texture.SetInts(values);
			return this;
		}

		public GPUCompute AddVector(string name, Vector4 value)
		{
			texture.SetVector(name, value);
			return this;
		}

		public GPUCompute AddVectorArray(string name, Vector4[] values)
		{
			texture.SetVectorArray(name, values);
			return this;
		}

		public GPUCompute AddMatrix(string name, Matrix4x4 value)
		{
			texture.SetMatrix(name, value);
			return this;
		}

		public GPUCompute AddMatrixArray(string name, Matrix4x4[] values)
		{
			texture.SetMatrixArray(name, values);
			return this;
		}

		public GPUCompute AddBool(string name, bool value)
		{
			texture.SetBool(name, value);
			return this;
		}

		#endregion
		#region Retreive Data

		public GPUCompute Compute(Vector2 parallelizationAxis)
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

		public GPUCompute DisposeBuffer(string name) 
		{
			buffers[name].Dispose();
			buffers.Remove(name);
		}

		public GPUCompute DisposeBuffers()
		{
			foreach (ComputeBuffer buffer in buffers) 
			{
				buffer.Dispose();
			}
			buffers.Clear();
			return this;
		}
	}
}