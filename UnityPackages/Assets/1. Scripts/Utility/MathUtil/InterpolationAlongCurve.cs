using System;
using System.Linq;
using Attributes;
using UnityEngine;

namespace Utility.MathUtil
{
	/// <summary>
	/// A class that interpolates between <see cref="ValueA"/> and <see cref="ValueB"/> using an <see cref="AnimationCurve"/>
	/// </summary>
	/// <seealso cref="Curve"/>
	[Serializable]
	public class InterpolationAlongCurve
	{
		[SerializeField]
		private AnimationCurve curve;

		/// <summary>
		/// The current curve used for retrieving the interpolation value between <see cref="ValueA"/> and <see cref="ValueB"/>
		/// </summary>
		/// <seealso cref="Evaluate"/>
		/// <seealso cref="EvaluateNormalized"/>
		/// <seealso cref="AnimationCurve"/>
		public AnimationCurve Curve
		{
			get => curve;
			set
			{
				curve = value;
				RecalculateMaxTime();
			}
		}

		[SerializeField, ReadOnly]
		private float maxTime;

		/// <summary>
		/// The latest time in the curve
		/// </summary>
		public float MaxTime
		{
			get
			{
				if (!hasCalculatedMaxTime)
				{
					RecalculateMaxTime();
				}

				return maxTime;
			}
		}

		/// <summary>
		/// <para>The first value used for interpolation.</para>
		/// <para>This will be returned for an interpolation value of 0.</para>
		/// </summary>
		[Space, Header("Values")]
		public float ValueA;

		/// <summary>
		/// <para>The second value for interpolation.</para>
		/// <para>This will be returned for an interpolation value of 1.</para>
		/// </summary>
		public float ValueB;

		/// <summary>
		/// How the <see cref="AnimationCurve"/> reacts to evaluating a time before the first keyframe
		/// </summary>
		public WrapMode CurvePreWrapMode => curve.preWrapMode;

		/// <summary>
		/// How the <see cref="AnimationCurve"/> reacts to evaluating a time after the last keyframe
		/// </summary>
		/// <seealso cref="MaxTime"/>
		public WrapMode CurvePostWrapMode => curve.postWrapMode;

		private bool hasCalculatedMaxTime = false;

		public InterpolationAlongCurve(AnimationCurve animationCurve, float valueA, float valueB)
		{
			curve = animationCurve;

			RecalculateMaxTime();

			ValueA = valueA;
			ValueB = valueB;
		}

		/// <summary>
		/// Returns an interpolated value between <see cref="ValueA"/> and <see cref="ValueB"/> based on the result of the <see cref="Curve"/> at <paramref name="time"/>
		/// </summary>
		/// <param name="time">The time to evaluate the curve for the interpolation value</param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float Evaluate(float time)
		{
			return Mathf.LerpUnclamped(ValueA, ValueB, curve.Evaluate(time));
		}

		/// <summary>
		/// Returns an interpolated value between <see cref="ValueA"/> and <see cref="ValueB"/> based on the result of the <see cref="Curve"/> at normalizedTime <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="normalizedTime">a normalized value that represents the % of the <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float EvaluateNormalized(float normalizedTime)
		{
			return Evaluate(normalizedTime * MaxTime);
		}

		private void RecalculateMaxTime()
		{
			// Get the latest time
			maxTime = curve.keys.Select(key => key.time).Max();

			hasCalculatedMaxTime = true;
		}
	}
}