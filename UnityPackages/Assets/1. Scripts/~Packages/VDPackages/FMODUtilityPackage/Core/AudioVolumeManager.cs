using FMOD.Studio;
using VDPackages.FMODUtilityPackage.Enums;

namespace VDPackages.FMODUtilityPackage.Core
{
	public static class AudioVolumeManager
	{
		private static float masterVolume;
		private static bool masterMute;
		
		public static float GetBusVolume(AudioBus audioBus)
		{
			if (IsMasterBus(audioBus))
			{
				return masterVolume;
			}

			Bus bus = GetBus(audioBus);
			bus.getVolume(out float volume);

			return volume;
		}
		
		public static void SetBusVolume(AudioBus audioBus, float volume)
		{
			if (IsMasterBus(audioBus))
			{
				SetMasterVolume(volume);
				return;
			}

			Bus bus = GetBus(audioBus);
			bus.setVolume(volume);
		}

		public static bool GetBusMute(AudioBus audioBus)
		{
			if (IsMasterBus(audioBus))
			{
				return masterMute;
			}

			Bus bus = GetBus(audioBus);
			bus.getMute(out bool isMuted);

			return isMuted;
		}
		
		public static void SetBusMute(AudioBus audioBus, bool isMuted)
		{
			if (IsMasterBus(audioBus))
			{
				SetMasterMute(isMuted);
				return;
			}
			
			Bus bus = GetBus(audioBus);
			bus.setMute(isMuted);
		}
		
		public static float GetMasterVolume()
		{
			return masterVolume;
		}
		
		/// <summary>
		/// Sets the volume of the master bus
		/// </summary>
		/// <param name="volume"></param>
		/// <param name="updateCached">Update the cached value as well (the cached value is used to get the old volume when you unmute)</param>
		/// <param name="ignoreMute">Should ignore the current mute state and set the volume anyway (can cancel mute)</param>
		public static void SetMasterVolume(float volume, bool updateCached = true, bool ignoreMute = false)
		{
			if (updateCached)
			{
				masterVolume = volume;
			}

			if (!ignoreMute && masterMute)
			{
				return;
			}

			Bus bus = GetBus(0);
			bus.setVolume(volume);
		}

		public static bool GetMasterMute()
		{
			return masterMute;
		}

		/// <summary>
		/// Mute the master bus (sets volume to 0)
		/// </summary>
		/// <explanation>
		/// Because muting the master bus does not work this function sets the volume to 0 instead <br/>
		/// The volume is internally cached so (un)muting behaves just like any other bus
		/// </explanation>
		public static void SetMasterMute(bool isMuted)
		{
			masterMute = isMuted;

			SetMasterVolume(isMuted ? 0 : masterVolume, false, true);
		}

		private static Bus GetBus(AudioBus audioBus)
		{ 
			return AudioManager.Instance.FMODPathResolver.GetAudioBus(audioBus);
		}

		private static bool IsMasterBus(AudioBus audioBus)
		{
			return audioBus == 0;
		}
	}
}