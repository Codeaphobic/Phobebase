using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Extensions
{
    public class Concat
    {
		// Add an Array onto the back of another Array
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
}