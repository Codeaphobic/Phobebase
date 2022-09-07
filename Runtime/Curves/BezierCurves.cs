using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Curves
{
	// Simple Little Bezier Curve Classes 
	// mostly so you dont have to look up the function every time u wanna use it.

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

		public Vector3 PointOnCurve(float t)
		{
			return weight1 + (1f - t) * (1f - t) * (point1 - weight1) + t * t * (point2 - weight1);
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

		public Vector3 PointOnCurve(float t)
		{
			return (1f - t) * (1f - t) * (1f - t) * point1 + 3 * ((1f - t) * (1f - t)) * t
				* weight1 + 3 * (1f - t) * (t * t) * weight2 + t * t * t * point2;
		}
	}
}