using UnityEngine;
using UtilityPackage.Utility.UnityFunctionHandlers.Enums;
using VDFramework;

namespace UtilityPackage.Utility.UnityFunctionHandlers
{
	/// <summary>
	/// Invoke the <see cref="ReactToEvent"/> function as a reaction to a <see cref="Enums.UnityFunction"/>s
	/// </summary>
	public abstract class AbstractFunctionHandler : BetterMonoBehaviour
	{
		[SerializeField]
		private UnityFunction reactTo;

		protected virtual void OnEnable()
		{
			if ((reactTo & UnityFunction.OnEnable) == 0)
			{
				return;
			}

			ReactToEvent(UnityFunction.OnEnable);
		}

		protected virtual void Start()
		{
			if ((reactTo & UnityFunction.Start) == 0)
			{
				return;
			}

			ReactToEvent(UnityFunction.Start);
		}

		protected virtual void OnDisable()
		{
			if ((reactTo & UnityFunction.OnDisable) == 0)
			{
				return;
			}

			ReactToEvent(UnityFunction.OnDisable);
		}

		protected virtual void OnDestroy()
		{
			if ((reactTo & UnityFunction.OnDestroy) == 0)
			{
				return;
			}

			ReactToEvent(UnityFunction.OnDestroy);
		}

		/// <summary>
		/// Will be called by the <see cref="AbstractFunctionHandler"/> as a reaction to a <see cref="Enums.UnityFunction"/>
		/// </summary>
		/// <param name="unityFunction">The function that caused this invoke to happen</param>
		protected abstract void ReactToEvent(UnityFunction unityFunction);
	}
}