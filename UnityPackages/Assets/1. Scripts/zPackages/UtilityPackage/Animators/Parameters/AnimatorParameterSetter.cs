using UnityEngine;
using VDFramework;
using VDFramework.Animators;

namespace UtilityPackage.Animators.Parameters
{
	public abstract class AnimatorParameterSetter : BetterMonoBehaviour
	{
		[SerializeField]
		protected Animator animator;

		[SerializeField]
		private string parameterName;

		protected int parameterID;
		
		private void Awake()
		{
			parameterID = AnimatorHashUtil.GetCachedID(parameterName);
		}

		/// <summary>
		/// Change the <see cref="Animator"/> on which the parameter will be set
		/// </summary>
		public void SetAnimator(Animator newAnimator)
		{
			animator = newAnimator;
		}
		
		/// <summary>
		/// Change the name of the parameter to set
		/// </summary>
		public void SetParameterName(string newName)
		{
			parameterName = name;
			parameterID   = AnimatorHashUtil.GetCachedID(parameterName);
		}
		
		/// <summary>
		/// Set the parameter with the given name in the Animator
		/// </summary>
		public abstract void SetParameter();
	}
}