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
}