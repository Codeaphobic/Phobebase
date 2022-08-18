using Codeaphobic;
using System;
using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class ThreadManager : Singleton
{
	private Dictionary<JobHandle, NativeArray<object>> workQueue = new Dictionary<JobHandle, NativeArray<object>>();
	private Dictionary<JobHandle, Action<object>> workCallbacks = new Dictionary<JobHandle, Action<object>>;

	public struct ThreadedWork<T1, TResult>
	{
		public T1 arg;
		public Func<T1, TResult> function;
		public NativeArray<TResult> result;

		public void Execute()
		{
			result[0] = function.Invoke(arg);
		}
	}

	public TResult QueueBasicAsyncThread<T1, TResult>(Func<T1, TResult> function, T1 arg, Action<TResult> callback)
	{
		NativeArray<TResult> result = new NativeArray<TResult>(1, Allocator.TempJob);

		ThreadedWork<T1, TResult> threadedWork = new ThreadedWork<T1, TResult>()
		{
			arg = arg,
			function = function,
			result = result
		}

			JobHandle handle = threadedWork.Schedule();

		workQueue.Add(handle, result);
		workCallbacks(handle, callback);
	}

	void Update()
	{
		if (workQueue.Count == 0) return;

		foreach (JobHandle handle in workQueue.keys)
		{
			if (!handle.isComplete) continue;

			handle.Complete();

			workCallbacks[handle].Invoke(workQueue[handle][0]);

			workQueue[handle].Dispose();
			workQueue.Remove(handle);
			workCallbacks.Remove(handle);
		}
	}
}