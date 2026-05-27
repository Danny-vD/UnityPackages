using UnityEngine;
using VDFramework;
using VDPackages.FMODUtilityPackage.Core;
using VDPackages.FMODUtilityPackage.Enums;

namespace VDPackages.FMODUtilityPackage.Components
{
	/// <summary>
	/// A component that provides several functions to manage setting volume of an audio bus
	/// </summary>
	public class AudioVolumeController : BetterMonoBehaviour
	{
		[SerializeField]
		private AudioBus busToControl;

		/// <summary>
		/// Changes the bus that this component controls
		/// </summary>
		public void ChangeAudioBus(AudioBus audioBus)
		{
			busToControl = audioBus;
		}

		public float GetVolume()
		{
			return AudioVolumeManager.GetBusVolume(busToControl);
		}
		
		public void SetVolume(float volume)
		{
			AudioVolumeManager.SetBusVolume(busToControl, volume);
		}

		public bool GetMute()
		{
			return AudioVolumeManager.GetBusMute(busToControl);
		}
		
		public void SetMute(bool isMuted)
		{
			AudioVolumeManager.SetBusMute(busToControl, isMuted);
		}

		public void ToggleMute()
		{
			SetMute(!GetMute());
		}
	}
}