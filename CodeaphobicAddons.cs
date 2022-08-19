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

	public class TransformExtension
	{
		private static Dictionary<Transform, List<Task>> _activeTweens = new Dictionary<Transform, List<Task>>();
		private static Dictionary<Task, CancellationTokenSource> _cancelTokens = new Dictionary<Task, CancellationTokenSource>();

		#if UNITY_EDITOR
				[RuntimeInitializeOnLoadMethod]
				static void RuntimeStart()
				{
					EditorApplication.playModeStateChanged += StopAllTweens;
				}
		#endif
		public static void TweenTo(this Transform transform, Vector3 lerpPosition, Quaternion lerpRotation, Vector3 lerpScale, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = TransformTween(transform, lerpPosition, lerpRotation, lerpScale, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenTo(this Transform transform, Vector3 lerpPosition, Quaternion lerpRotation, Vector3 lerpScale, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = TransformTween(transform, lerpPosition, lerpRotation, lerpScale, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenTo(this Transform transform, Transform lerpTransform, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = TransformTween(transform, transform, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenTo(this Transform transform, Transform lerpTransform, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = TransformTween(transform, transform, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenPosition(this Transform transform, Vector3 lerpPosition, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = PositionTween(transform, transform.position, lerpPosition, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenPosition(this Transform transform, Vector3 lerpPosition, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = PositionTween(transform, transform.position, lerpPosition, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenPosition(this Transform transform, float x, float y, float z, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = PositionTween(transform, transform.position, new Vector3(x, y, z), time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenPosition(this Transform transform, float x, float y, float z, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = PositionTween(transform, transform.position, new Vector3(x, y, z), time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenRotation(this Transform transform, Quaternion lerpRotation, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = RotationTween(transform, transform.rotation, lerpRotation, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenRotation(this Transform transform, Quaternion lerpRotation, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = RotationTween(transform, transform.rotation, lerpRotation, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenRotation(this Transform transform, float x, float y, float z, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = RotationTween(transform, transform.rotation, Quaternion.Euler(x, y, z), time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenRotation(this Transform transform, float x, float y, float z, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = RotationTween(transform, transform.rotation, Quaternion.Euler(x, y, z), time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenScale(this Transform transform, Vector3 lerpScale, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = ScaleTween(transform, transform.localScale, lerpScale, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenScale(this Transform transform, Vector3 lerpScale, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = ScaleTween(transform, transform.localScale, lerpScale, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenScale(this Transform transform, float x, float y, float z, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = ScaleTween(transform, transform.localScale, new Vector3(x, y, z), time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenScale(this Transform transform, float x, float y, float z, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = ScaleTween(transform, transform.localScale, new Vector3(x, y, z), time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenAlong(this Transform transform, BezierCurve2 bezier, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = BezierTween(transform, bezier, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}

		public static void TweenAlong(this Transform transform, BezierCurve2 bezier, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = BezierTween(transform, bezier, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}
		// Animates a transform along cubic bezier path
		public static void TweenAlong(this Transform transform, BezierCurve3 bezier, float time, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = BezierTween(transform, bezier, time, curve, ct);

			AddTweenToCache(transform, task, cts);
		}
		// Animates a transform along cubic bezier path and calls action when finished
		public static void TweenAlong(this Transform transform, BezierCurve3 bezier, float time, Action finishEvent, AnimationCurve curve = null)
		{
			if (curve == null) curve = AnimationCurve.Linear(0, 0, 1, 1);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			Task task = BezierTween(transform, bezier, time, curve, ct, finishEvent);

			AddTweenToCache(transform, task, cts);
		}

		private static void AddTweenToCache(Transform transform, Task task, CancellationTokenSource cts)
		{
			if (!_activeTweens.ContainsKey(transform))
			{
				_activeTweens.Add(transform, new List<Task>() { task });
				_cancelTokens.Add(task, cts);
				return;
			}

			_activeTweens[transform].Add(task);
			_cancelTokens.Add(task, cts);
		}

		private static async Task TransformTween(Transform obj, Transform to, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			Vector3 startPosition = obj.position;
			Quaternion startRotation = obj.rotation;
			Vector3 startScale = obj.localScale;

			while (lerped0to1 < 1)
			{
				obj.position = Vector3.Lerp(startPosition, to.position, curve.Evaluate(lerped0to1));
				obj.rotation = Quaternion.Slerp(startRotation, to.rotation, curve.Evaluate(lerped0to1));
				obj.localScale = Vector3.Lerp(startScale, to.localScale, curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom position: {obj.position}, tranform rotation: {obj.rotation.eulerAngles}, transform scale: {obj.localScale}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}
		private static async Task TransformTween(Transform obj, Vector3 lerpPosition, Quaternion lerpRotation, Vector3 lerpScale, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			Vector3 startPosition = obj.position;
			Quaternion startRotation = obj.rotation;
			Vector3 startScale = obj.localScale;

			while (lerped0to1 < 1)
			{
				obj.position = Vector3.Lerp(startPosition, lerpPosition, curve.Evaluate(lerped0to1));
				obj.rotation = Quaternion.Slerp(startRotation, lerpRotation, curve.Evaluate(lerped0to1));
				obj.localScale = Vector3.Lerp(startScale, lerpScale, curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom position: {obj.position}, tranform rotation: {obj.rotation.eulerAngles}, transform scale: {obj.localScale}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}
		private static async Task PositionTween(Transform obj, Vector3 from, Vector3 to, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			while (lerped0to1 < 1)
			{
				lerped0to1 = Mathf.MoveTowards(lerped0to1, 1f, time * Time.deltaTime);

				obj.position = Vector3.Lerp(from, to, curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom scale: {obj.position}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}
		private static async Task RotationTween(Transform obj, Quaternion from, Quaternion to, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			while (lerped0to1 < 1)
			{
				lerped0to1 = Mathf.MoveTowards(lerped0to1, 1f, time * Time.deltaTime);

				obj.rotation = Quaternion.Slerp(from, to, curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom rotation: {obj.rotation.eulerAngles}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}
		private static async Task ScaleTween(Transform obj, Vector3 from, Vector3 to, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			while (lerped0to1 < 1)
			{
				lerped0to1 = Mathf.MoveTowards(lerped0to1, 1f, time * Time.deltaTime);

				obj.localScale = Vector3.Lerp(from, to, curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom scale: {obj.localScale}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}
		private static async Task BezierTween(Transform obj, BezierCurve2 bezier, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			while (lerped0to1 < 1)
			{
				lerped0to1 = Mathf.MoveTowards(lerped0to1, 1f, time * Time.deltaTime);

				obj.position = bezier.PointOnCurve(curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom position: {obj.position}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}
		private static async Task BezierTween(Transform obj, BezierCurve3 bezier, float time, AnimationCurve curve, CancellationToken ct, Action finishEvent = null)
		{
			float lerped0to1 = 0f;

			time = 1f / time;

			while (lerped0to1 < 1)
			{
				lerped0to1 = Mathf.MoveTowards(lerped0to1, 1f, time * Time.deltaTime);

				obj.position = bezier.PointOnCurve(curve.Evaluate(lerped0to1));
				Debug.Log($"transfrom position: {obj.position}");
				await System.Threading.Tasks.Task.Delay(10);
				if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
			}

			if (finishEvent != null) finishEvent.Invoke();
		}

		public static int TweenCount(this Transform transform)
		{
			if (!_activeTweens.ContainsKey(transform)) return 0;
			return _activeTweens[transform].Count;
		}

		public static async void StopTweens(this Transform transform)
		{
			if (!_activeTweens.ContainsKey(transform)) return;

			foreach (Task task in _activeTweens[transform])
			{
				_cancelTokens[task].Cancel();

				try
				{
					await task;
				}
				finally
				{
					_cancelTokens[task].Dispose();
				}
			}
		}

		public static async void StopAllTweens(PlayModeStateChange playModeState)
		{
			if (playModeState == PlayModeStateChange.ExitingPlayMode)
			{
				foreach (List<Task> tasks in _activeTweens.Values)
				{
					foreach (Task task in tasks)
					{
						_cancelTokens[task].Cancel();

						try
						{
							await task;
						}
						finally
						{
							_cancelTokens[task].Dispose();
						}
					}
				}

				EditorApplication.playModeStateChanged -= StopAllTweens;
			}
		}
	}

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