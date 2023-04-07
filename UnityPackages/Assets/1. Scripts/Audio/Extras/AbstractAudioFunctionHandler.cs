using Audio.Core;
using Utility;

namespace Audio.Extras
{
	/// <summary>
	/// React to specific unity events with respect to the current initialisation state of the <see cref="AudioManager"/>
	/// </summary>
	public abstract class AbstractAudioFunctionHandler : AbstractFunctionHandler
	{
		protected override void OnEnable()
		{
			if (AudioManager.IsInitialized)
			{
				base.OnEnable();
			}
			else
			{
				Invoke(nameof(OnEnable), 0.1f);
			}
		}

		protected override void OnDisable()
		{
			if (AudioManager.IsInitialized)
			{
				base.OnDisable();
			}
		}

		protected override void OnDestroy()
		{
			if (AudioManager.IsInitialized)
			{
				base.OnDestroy();
			}
		}
	}
}