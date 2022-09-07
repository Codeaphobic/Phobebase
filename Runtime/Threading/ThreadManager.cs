using Phobebase.Common;
using System;
using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;

namespace Phobebase.Threading
{
	// Probably would recommend writing your own less Generalised Method as this one is optimised for simplisity
	// And wont be as readable as a custom implementation for your specific needs.

	public class ThreadManager : Singleton<ThreadManager>
	{
		private Dictionary<JobHandle, NativeArray<Threading.ThreadResult>> workQueue = new Dictionary<JobHandle, NativeArray<ThreadResult>>();
		private Dictionary<JobHandle, Action<ThreadResult>> workCallbacks = new Dictionary<JobHandle, Action<ThreadResult>>();

		// Generalised Struct for Unity JOBS system to execute a Func
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

		// Starts a work on the unity JOBs system which can be completed over multiple frames
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

		// Checks if there are any JOBs that are complete and send data where it needs to go
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