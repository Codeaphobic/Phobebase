using Codeaphobic.Threading;
using System;
using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;

namespace Codeaphobic 
{
	public class ThreadManager : Singleton<ThreadManager>
	{
		private Dictionary<JobHandle, NativeArray<Threading.ThreadResult>> workQueue = new Dictionary<JobHandle, NativeArray<ThreadResult>>();
		private Dictionary<JobHandle, Action<ThreadResult>> workCallbacks = new Dictionary<JobHandle, Action<ThreadResult>>();

		public struct ThreadedWork<T1, TResult> : IJob
		{
			public T1 arg;
			public Func<T1, TResult> function;
			public NativeArray<ThreadResult> result;

			public void Execute()
			{
				ThreadResult threadResult = new ThreadResult(true, function.Invoke(arg));
				result[0] = threadResult;
			}
		}

		public void QueueBasicAsyncThread<T1, TResult>(Func<T1, ThreadResult> function, T1 arg, Action<ThreadResult> callback)
		{
			NativeArray<ThreadResult> result = new NativeArray<ThreadResult>(1, Allocator.TempJob);

			ThreadedWork<T1, ThreadResult> threadedWork = new ThreadedWork<T1, ThreadResult>()
			{
				arg = arg,
				function = function,
				result = result
			};

			JobHandle handle = threadedWork.Schedule();

			workQueue.Add(handle, result);
			workCallbacks.Add(handle, callback);
		}

		void Update()
		{
			if (workQueue.Count == 0) return;

			foreach (JobHandle handle in workQueue.Keys)
			{
				if (!handle.IsCompleted) continue;

				handle.Complete();

				workCallbacks[handle].Invoke(workQueue[handle][0]);

				workQueue[handle].Dispose();
				workQueue.Remove(handle);
				workCallbacks.Remove(handle);
			}
		}
	}
}