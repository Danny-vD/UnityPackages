using System;
using System.Linq;
using Attributes;
using UnityEngine;
using VDFramework.Extensions;

namespace UtilityPackage.Utility.MathUtil
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
		/// <seealso cref="Evaluate(float)"/>
		/// <seealso cref="EvaluateNormalized(float)"/>
		/// <seealso cref="EvaluateCurve(float)"/>
		/// <seealso cref="EvaluateCurveNormalized(float)"/>
		/// <seealso cref="AnimationCurve"/>
		public AnimationCurve Curve
		{
			get => curve;
			set
			{
				curve = value;
				RecalculateValues();
			}
		}

		[SerializeField, ReadOnly]
		private float minTime;
		
		[SerializeField, ReadOnly]
		private float maxTime;
		
		[SerializeField, ReadOnly]
		private float minValue;
		
		[SerializeField, ReadOnly]
		private float maxValue;
		
		/// <summary>
		/// The earliest time in the curve
		/// </summary>
		public float MinTime
		{
			get
			{
				if (!hasCalculatedValues)
				{
					RecalculateValues();
				}

				return minTime;
			}
		}
		
		/// <summary>
		/// The latest time in the curve
		/// </summary>
		public float MaxTime
		{
			get
			{
				if (!hasCalculatedValues)
				{
					RecalculateValues();
				}

				return maxTime;
			}
		}
		
		/// <summary>
		/// The lowest value in the curve
		/// </summary>
		public float MinValue
		{
			get
			{
				if (!hasCalculatedValues)
				{
					RecalculateValues();
				}

				return minValue;
			}
		}
		
		/// <summary>
		/// The highest value in the curve
		/// </summary>
		public float MaxValue
		{
			get
			{
				if (!hasCalculatedValues)
				{
					RecalculateValues();
				}

				return maxValue;
			}
		}

		/// <summary>
		/// The length of the curve between the first and last keyframe
		/// </summary>
		public float TotalCurveTime => MaxTime - MinTime;

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

		private bool hasCalculatedValues = false;

		public InterpolationAlongCurve(AnimationCurve animationCurve, float valueA, float valueB)
		{
			curve = animationCurve;

			if (curve != null)
			{
				RecalculateValues();
			}

			ValueA = valueA;
			ValueB = valueB;
		}
		
		public InterpolationAlongCurve(AnimationCurve animationCurve)
		{
			curve = animationCurve;

			if (curve != null)
			{
				RecalculateValues();
				
				ValueA = MinValue;
				ValueB = MaxValue;
			}
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
			return Mathf.LerpUnclamped(valueA, valueB, EvaluateCurve(time));
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
		/// <param name="normalizedTime">a normalized value that represents the % of the <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float EvaluateNormalized(float normalizedTime, float valueA, float valueB)
		{
			return Evaluate(MinTime + normalizedTime * TotalCurveTime, valueA, valueB);
		}

		/// <summary>
		/// Returns an interpolated value between <see cref="ValueA"/> and <see cref="ValueB"/> based on the result of the <see cref="Curve"/> at <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="normalizedTime">a normalized value that represents the % of the <see cref="MaxTime"/></param>
		/// <returns>An (unclamped) interpolated value between A and B</returns>
		public float EvaluateNormalized(float normalizedTime)
		{
			return EvaluateNormalized(normalizedTime, ValueA, ValueB);
		}
		
		/// <summary>
		/// Returns the value of the <see cref="Curve"/> at <paramref name="time"/>
		/// </summary>
		/// <param name="time">The time to evaluate the curve</param>
		public float EvaluateCurve(float time)
		{
			return curve.Evaluate(time);
		}
		
		/// <summary>
		/// Returns the value of the <see cref="Curve"/> at <paramref name="normalizedTime"/>
		/// </summary>
		/// <param name="normalizedTime">a normalized value that represents the % of the <see cref="MaxTime"/></param>
		public float EvaluateCurveNormalized(float normalizedTime)
		{
			return EvaluateCurve(MinTime + normalizedTime * TotalCurveTime);
		}

		private void RecalculateValues()
		{
			Keyframe[] curveKeys = curve.keys;

			curveKeys.Select(key => key.time).GetMinMax(out minTime, out maxTime);

			curveKeys.Select(key => key.value).GetMinMax(out minValue, out maxValue);

			hasCalculatedValues = true;
		}
	}
}