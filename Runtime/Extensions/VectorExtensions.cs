using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Extensions
{
    public static class VectorRound
    {
		// A round extension for all the Vector types

		public static Vector4 Round(this Vector4 vector4, int decimalPlaces = 2)
		{
			float multiplier = 1;
			for (int i = 0; i < decimalPlaces; i++)
			{
				multiplier *= 10f;
			}
			return new Vector4(
				Mathf.Round(vector4.x * multiplier) / multiplier,
				Mathf.Round(vector4.y * multiplier) / multiplier,
				Mathf.Round(vector4.z * multiplier) / multiplier,
				Mathf.Round(vector4.w * multiplier) / multiplier);
		}

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
    }
}