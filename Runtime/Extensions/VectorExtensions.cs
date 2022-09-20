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

		#region Vector4

		// Functions 

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

		// Handles for using RGBA references

		public static float r(this Vector4 aVector)
		{
			return aVector.x;
		}

		public static float g(this Vector4 aVector)
		{
			return aVector.y;
		}

		public static float b(this Vector4 aVector)
		{
			return aVector.z;
		}

		public static float a(this Vector4 aVector)
		{
			return aVector.w;
		}

		// Short Cuts for getting permutations of the vector

		public static Vector2 xy(this Vector4 aVector)
		{
			return new Vector2(aVector.x,aVector.y);
		}
		public static Vector2 xz(this Vector4 aVector)
		{
			return new Vector2(aVector.x,aVector.z);
		}
		public static Vector2 xw(this Vector4 aVector)
		{
			return new Vector2(aVector.x,aVector.w);
		}
		public static Vector2 yx(this Vector4 aVector)
		{
			return new Vector2(aVector.y,aVector.x);
		}
		public static Vector2 yz(this Vector4 aVector)
		{
			return new Vector2(aVector.y,aVector.z);
		}
		public static Vector2 yw(this Vector4 aVector)
		{
			return new Vector2(aVector.y,aVector.w);
		}
		public static Vector2 zx(this Vector4 aVector)
		{
			return new Vector2(aVector.z,aVector.x);
		}
		public static Vector2 zy(this Vector4 aVector)
		{
			return new Vector2(aVector.z,aVector.y);
		}
		public static Vector2 zw(this Vector4 aVector)
		{
			return new Vector2(aVector.z,aVector.w);
		}
		public static Vector2 wx(this Vector4 aVector)
		{
			return new Vector2(aVector.w,aVector.x);
		}
		public static Vector2 wy(this Vector4 aVector)
		{
			return new Vector2(aVector.w,aVector.y);
		}
		public static Vector2 wz(this Vector4 aVector)
		{
			return new Vector2(aVector.w,aVector.z);
		}
		public static Vector3 xyz(this Vector4 aVector)
		{
			return new Vector3(aVector.x,aVector.y,aVector.z);
		}
		public static Vector3 xyw(this Vector4 aVector)
		{
			return new Vector3(aVector.x,aVector.y,aVector.w);
		}
		public static Vector3 xzy(this Vector4 aVector)
		{
			return new Vector3(aVector.x,aVector.z,aVector.y);
		}
		public static Vector3 xzw(this Vector4 aVector)
		{
			return new Vector3(aVector.x,aVector.z,aVector.w);
		}
		public static Vector3 xwy(this Vector4 aVector)
		{
			return new Vector3(aVector.x,aVector.w,aVector.y);
		}
		public static Vector3 xwz(this Vector4 aVector)
		{
			return new Vector3(aVector.x,aVector.w,aVector.z);
		}
		public static Vector3 yxz(this Vector4 aVector)
		{
			return new Vector3(aVector.y,aVector.x,aVector.z);
		}
		public static Vector3 yxw(this Vector4 aVector)
		{
			return new Vector3(aVector.y,aVector.x,aVector.w);
		}
		public static Vector3 yzx(this Vector4 aVector)
		{
			return new Vector3(aVector.y,aVector.z,aVector.x);
		}
		public static Vector3 yzw(this Vector4 aVector)
		{
			return new Vector3(aVector.y,aVector.z,aVector.w);
		}
		public static Vector3 ywx(this Vector4 aVector)
		{
			return new Vector3(aVector.y,aVector.w,aVector.x);
		}
		public static Vector3 ywz(this Vector4 aVector)
		{
			return new Vector3(aVector.y,aVector.w,aVector.z);
		}
		public static Vector3 zxy(this Vector4 aVector)
		{
			return new Vector3(aVector.z,aVector.x,aVector.y);
		}
		public static Vector3 zxw(this Vector4 aVector)
		{
			return new Vector3(aVector.z,aVector.x,aVector.w);
		}
		public static Vector3 zyx(this Vector4 aVector)
		{
			return new Vector3(aVector.z,aVector.y,aVector.x);
		}
		public static Vector3 zyw(this Vector4 aVector)
		{
			return new Vector3(aVector.z,aVector.y,aVector.w);
		}
		public static Vector3 zwx(this Vector4 aVector)
		{
			return new Vector3(aVector.z,aVector.w,aVector.x);
		}
		public static Vector3 zwy(this Vector4 aVector)
		{
			return new Vector3(aVector.z,aVector.w,aVector.y);
		}
		public static Vector3 wxy(this Vector4 aVector)
		{
			return new Vector3(aVector.w,aVector.x,aVector.y);
		}
		public static Vector3 wxz(this Vector4 aVector)
		{
			return new Vector3(aVector.w,aVector.x,aVector.z);
		}
		public static Vector3 wyx(this Vector4 aVector)
		{
			return new Vector3(aVector.w,aVector.y,aVector.x);
		}
		public static Vector3 wyz(this Vector4 aVector)
		{
			return new Vector3(aVector.w,aVector.y,aVector.z);
		}
		public static Vector3 wzx(this Vector4 aVector)
		{
			return new Vector3(aVector.w,aVector.z,aVector.x);
		}
		public static Vector3 wzy(this Vector4 aVector)
		{
			return new Vector3(aVector.w,aVector.z,aVector.y);
		}

		// rgba permutations

		public static Vector2 rg(this Vector4 aVector)
		{
			return new Vector2(aVector.r,aVector.g);
		}
		public static Vector2 rb(this Vector4 aVector)
		{
			return new Vector2(aVector.r,aVector.b);
		}
		public static Vector2 ra(this Vector4 aVector)
		{
			return new Vector2(aVector.r,aVector.a);
		}
		public static Vector2 gr(this Vector4 aVector)
		{
			return new Vector2(aVector.g,aVector.r);
		}
		public static Vector2 gb(this Vector4 aVector)
		{
			return new Vector2(aVector.g,aVector.b);
		}
		public static Vector2 ga(this Vector4 aVector)
		{
			return new Vector2(aVector.g,aVector.a);
		}
		public static Vector2 br(this Vector4 aVector)
		{
			return new Vector2(aVector.b,aVector.r);
		}
		public static Vector2 bg(this Vector4 aVector)
		{
			return new Vector2(aVector.b,aVector.g);
		}
		public static Vector2 ba(this Vector4 aVector)
		{
			return new Vector2(aVector.b,aVector.a);
		}
		public static Vector2 ar(this Vector4 aVector)
		{
			return new Vector2(aVector.a,aVector.r);
		}
		public static Vector2 ag(this Vector4 aVector)
		{
			return new Vector2(aVector.a,aVector.g);
		}
		public static Vector2 ab(this Vector4 aVector)
		{
			return new Vector2(aVector.a,aVector.b);
		}
		public static Vector3 rgb(this Vector4 aVector)
		{
			return new Vector3(aVector.r,aVector.g,aVector.b);
		}
		public static Vector3 rga(this Vector4 aVector)
		{
			return new Vector3(aVector.r,aVector.g,aVector.a);
		}
		public static Vector3 rbg(this Vector4 aVector)
		{
			return new Vector3(aVector.r,aVector.b,aVector.g);
		}
		public static Vector3 rba(this Vector4 aVector)
		{
			return new Vector3(aVector.r,aVector.b,aVector.a);
		}
		public static Vector3 rag(this Vector4 aVector)
		{
			return new Vector3(aVector.r,aVector.a,aVector.g);
		}
		public static Vector3 rab(this Vector4 aVector)
		{
			return new Vector3(aVector.r,aVector.a,aVector.b);
		}
		public static Vector3 grb(this Vector4 aVector)
		{
			return new Vector3(aVector.g,aVector.r,aVector.b);
		}
		public static Vector3 gra(this Vector4 aVector)
		{
			return new Vector3(aVector.g,aVector.r,aVector.a);
		}
		public static Vector3 gbr(this Vector4 aVector)
		{
			return new Vector3(aVector.g,aVector.b,aVector.r);
		}
		public static Vector3 gba(this Vector4 aVector)
		{
			return new Vector3(aVector.g,aVector.b,aVector.a);
		}
		public static Vector3 gar(this Vector4 aVector)
		{
			return new Vector3(aVector.g,aVector.a,aVector.r);
		}
		public static Vector3 gab(this Vector4 aVector)
		{
			return new Vector3(aVector.g,aVector.a,aVector.b);
		}
		public static Vector3 brg(this Vector4 aVector)
		{
			return new Vector3(aVector.b,aVector.r,aVector.g);
		}
		public static Vector3 bra(this Vector4 aVector)
		{
			return new Vector3(aVector.b,aVector.r,aVector.a);
		}
		public static Vector3 bgr(this Vector4 aVector)
		{
			return new Vector3(aVector.b,aVector.g,aVector.r);
		}
		public static Vector3 bga(this Vector4 aVector)
		{
			return new Vector3(aVector.b,aVector.g,aVector.a);
		}
		public static Vector3 bar(this Vector4 aVector)
		{
			return new Vector3(aVector.b,aVector.a,aVector.r);
		}
		public static Vector3 bag(this Vector4 aVector)
		{
			return new Vector3(aVector.b,aVector.a,aVector.g);
		}
		public static Vector3 arg(this Vector4 aVector)
		{
			return new Vector3(aVector.a,aVector.r,aVector.g);
		}
		public static Vector3 arb(this Vector4 aVector)
		{
			return new Vector3(aVector.a,aVector.r,aVector.b);
		}
		public static Vector3 agr(this Vector4 aVector)
		{
			return new Vector3(aVector.a,aVector.g,aVector.r);
		}
		public static Vector3 agb(this Vector4 aVector)
		{
			return new Vector3(aVector.a,aVector.g,aVector.b);
		}
		public static Vector3 abr(this Vector4 aVector)
		{
			return new Vector3(aVector.a,aVector.b,aVector.r);
		}
		public static Vector3 abg(this Vector4 aVector)
		{
			return new Vector3(aVector.a,aVector.b,aVector.g);
		}

		#endregion

		#region Vector3

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

		// Handles for using RGB references

		public static float r(this Vector4 aVector)
		{
			return aVector.x;
		}

		public static float g(this Vector4 aVector)
		{
			return aVector.y;
		}

		public static float b(this Vector4 aVector)
		{
			return aVector.z;
		}

		// Grabing permutations from Vector3

		public static Vector2 xy(this Vector3 aVector)
		{
			return new Vector2(aVector.x,aVector.y);
		}
		public static Vector2 xz(this Vector3 aVector)
		{
			return new Vector2(aVector.x,aVector.z);
		}
		public static Vector2 yz(this Vector3 aVector)
		{
			return new Vector2(aVector.y,aVector.z);
		}
		public static Vector2 yx(this Vector3 aVector)
		{
			return new Vector2(aVector.y,aVector.x);
		}
		public static Vector2 zx(this Vector3 aVector)
		{
			return new Vector2(aVector.z,aVector.x);
		}
		public static Vector2 zy(this Vector3 aVector)
		{
			return new Vector2(aVector.z,aVector.y);
		}

		// rgb permutations

		public static Vector2 rg(this Vector4 aVector)
		{
			return new Vector2(aVector.r,aVector.g);
		}
		public static Vector2 rb(this Vector4 aVector)
		{
			return new Vector2(aVector.r,aVector.b);
		}
		public static Vector2 gr(this Vector4 aVector)
		{
			return new Vector2(aVector.g,aVector.r);
		}
		public static Vector2 gb(this Vector4 aVector)
		{
			return new Vector2(aVector.g,aVector.b);
		}
		public static Vector2 br(this Vector4 aVector)
		{
			return new Vector2(aVector.b,aVector.r);
		}
		public static Vector2 bg(this Vector4 aVector)
		{
			return new Vector2(aVector.b,aVector.g);
		}

		#endregion

		#region Vector2

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

		// 

		#endregion
    }
}