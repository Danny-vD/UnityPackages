using UnityEngine;

namespace UtilityPackage.Animators.Parameters
{
	public abstract class AnimatorValueSetter<TType> : AnimatorParameterSetter
	{
		[SerializeField]
		protected TType value;
		
		/// <summary>
		/// <para>Sets the value that this ParameterSetter will set.</para>
		/// <para>Does not set the value of the animator parameter. Use <see cref="AnimatorParameterSetter.SetParameter"/> for that</para>
		/// </summary>
		public void SetValue(TType newValue)
		{
			value = newValue;
		}
	}
}