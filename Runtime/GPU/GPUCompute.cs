using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.GPU
{
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
}