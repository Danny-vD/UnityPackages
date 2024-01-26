using System;
using System.Collections.Generic;
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
	public class InterpolationAlongCurve : ISerializationCallbackReceiver
	{
		[SerializeField]
		private AnimationCurve curve;

		/// <summary>
		/// The current curve used for retrieving the interpolation value between <see cref="ValueA"/> and <see cref="ValueB"/>
		/// </summary>
		/// <seealso cref="Evaluate(float)"/>
		/// <seealso cref="EvaluateNormalized(float)"/>
		/// <seealso cref="AnimationCurve"/>
		public AnimationCurve Curve
		{
			get => curve;
			set
			{
				curve = value;
				RecalculateMinMaxTime();
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
				if (!hasCalculatedTime)
				{
					RecalculateMinMaxTime();
				}

				return maxTime;
			}
		}
		
		[SerializeField, ReadOnly]
		private float minTime;

		/// <summary>
		/// The latest time in the curve
		/// </summary>
		public float MinTime
		{
			get
			{
				if (!hasCalculatedTime)
				{
					RecalculateMinMaxTime();
				}

				return minTime;
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

		private bool hasCalculatedTime = false;

		public InterpolationAlongCurve(AnimationCurve animationCurve, float valueA, float valueB)
		{
			curve = animationCurve;

			RecalculateMinMaxTime();

			ValueA = valueA;
			ValueB = valueB;
		}

		/// <summary>
		/// Returns an interpolated value between <paramref name="valueA"/> and <paramref name="valueB"/> based on the result of the <see cref="Curve"/> at <paramref name="time"/>
		/// </summary>
		/// <param name="valueA"><see cref="ValueA"/></param>
		/// <param name="valueB"><see cref="ValueB"/></param>
		/// <param name="time">The time to evaluate the curve for the interpolation value</param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float Evaluate(float time, float valueA, float valueB)
		{
			return Mathf.LerpUnclamped(valueA, valueB, curve.Evaluate(time));
		}

		/// <summary>
		/// Returns an interpolated value between <see cref="ValueA"/> and <see cref="ValueB"/> based on the result of the <see cref="Curve"/> at <paramref name="time"/>
		/// </summary>
		/// <param name="time">The time to evaluate the curve for the interpolation value</param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float Evaluate(float time)
		{
			return Evaluate(time, ValueA, ValueB);
		}
		
		/// <summary>
		/// Returns an interpolated value between <paramref name="valueA"/> and <paramref name="valueB"/> based on the result of the <see cref="Curve"/> at <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="valueA"><see cref="ValueA"/></param>
		/// <param name="valueB"><see cref="ValueB"/></param>
		/// <param name="normalizedTime">a normalized value that represents the % between <see cref="MinTime"/> and <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float EvaluateNormalized(float normalizedTime, float valueA, float valueB)
		{
			return Evaluate(Mathf.Lerp(MinTime, MaxTime, normalizedTime), valueA, valueB);
		}
		
		/// <summary>
		/// Returns an interpolated value between <paramref name="valueA"/> and <paramref name="valueB"/> based on the result of the <see cref="Curve"/> at <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="valueA"><see cref="ValueA"/></param>
		/// <param name="valueB"><see cref="ValueB"/></param>
		/// <param name="normalizedTime">a normalized value that represents the % of the <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		/// /// <remarks>This function assumes that MinTime is 0 and does not take the actual start of the curve into account</remarks>
		public float EvaluateNormalizedPositive(float normalizedTime, float valueA, float valueB)
		{
			return Evaluate(normalizedTime * maxTime, valueA, valueB);
		}

		/// <summary>
		/// Returns an interpolated value between <see cref="ValueA"/> and <see cref="ValueB"/> based on the result of the <see cref="Curve"/> at <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="normalizedTime">a normalized value that represents the % between <see cref="MinTime"/> and <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float EvaluateNormalized(float normalizedTime)
		{
			return EvaluateNormalized(normalizedTime, ValueA, ValueB);
		}
		
		/// <summary>
		/// Returns an interpolated value between <see cref="ValueA"/> and <see cref="ValueB"/> based on the result of the <see cref="Curve"/> at <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="normalizedTime">a normalized value that represents the % of the <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		/// <remarks>This function assumes that MinTime is 0 and does not take the actual start of the curve into account</remarks>
		public float EvaluateNormalizedPositive(float normalizedTime)
		{
			return EvaluateNormalizedPositive(normalizedTime, ValueA, ValueB);
		}

		private void RecalculateMinMaxTime()
		{
			// Get the latest time
			IEnumerable<float> keyTimes = curve.keys.Select(key => key.time);

			minTime = float.MaxValue;
			maxTime = float.MinValue;
			
			foreach (float time in keyTimes)
			{
				if (time < minTime)
				{
					minTime = time;
				}

				if (time > maxTime)
				{
					maxTime = time;
				}
			}

			hasCalculatedTime = true;
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			RecalculateMinMaxTime();
		}
	}
}