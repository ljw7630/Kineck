using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace LibKineck
{
	/// <summary>
	/// This class is used for calculating the head swing angle using Quaternion
	/// </summary>
	public class AngleCalculator
	{
		/// <summary>
		/// Calcuate the length of the vector
		/// </summary>
		/// <param name="vec">A 4-element(X, Y, Z, W) vector</param>
		/// <returns>a floating point represents the length of the vector</returns>
		static float GetLength(ref Vector4 vec)
		{
			return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="quat">A 4-element(X,Y,Z,W) vector about Head-ShoulderCenter</param>
		/// <returns>A floating point represents the angle the head swing in X-axis (Y-axis is the reference axis)</returns>
        static float GetYAngleDegreeOfXAxis(Vector4 quat)
        {
            float fTx = 2.0f * quat.X;
            float fTy = 2.0f * quat.Y;
            float fTz = 2.0f * quat.Z;
            float fTwx = fTx * quat.W;
            float fTwz = fTz * quat.W;
            float fTxx = fTx * quat.X;
            float fTxy = fTy * quat.X;
            float fTyz = fTz * quat.Y;
            float fTzz = fTz * quat.Z;

            Vector4 yAxis = new Vector4();
            yAxis.X = fTxy - fTwz;
            yAxis.Y = 1.0f - (fTxx + fTzz);
            yAxis.Z = fTyz + fTwx;

			Vector4 dest = new Vector4();
			dest.X = 1;
			dest.Y = 0;
			dest.Z = 0;

			float lenProduct = GetLength(ref yAxis) * GetLength(ref dest);
			if (lenProduct < 1e-6f)
			{
				lenProduct = 1e-6f;
			}

			float dotPro = yAxis.X * dest.X + yAxis.Y * dest.Y + yAxis.Z * dest.Z;
			float f = dotPro / lenProduct;

			f = Math.Max(Math.Min(f, 1.0f), -1.0f);
			float ret = (float)(Math.Acos(f) * 180.0f / Math.PI);
			return 90 - ret;
        }

		/// <summary>
		/// A threshold value to decide when to trigger HeadRotating event
		/// </summary>
		private static readonly float threshold = 10f;

		/// <summary>
		/// Calculate the head rotation by skeleton
		/// </summary>
		/// <param name="skeleton">The skeleton of the person</param>
		/// <returns>Whether this person's head is rotating</returns>
		public static HeadRotation GetHeadRotation(ref Skeleton skeleton)
		{
			BoneOrientation head = null;
			foreach (BoneOrientation bone in skeleton.BoneOrientations)
			{
				if (bone.EndJoint == JointType.Head)
				{
					head = bone;
					break;
				}
			}
			float degree = GetYAngleDegreeOfXAxis(head.AbsoluteRotation.Quaternion);
			
			if (degree > threshold)
			{
				return HeadRotation.LEFT;
			}
			else if (degree < -threshold)
			{
				return HeadRotation.LEFT;
			}
			else
			{
				return HeadRotation.RIGHT;
			}
		}
    }
}
