using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Common
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
}