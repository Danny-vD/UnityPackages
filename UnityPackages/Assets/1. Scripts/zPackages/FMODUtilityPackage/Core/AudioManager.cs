using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.Structs;
using UnityEngine;
using UnityEngine.Serialization;
using VDFramework.Logger;
using VDFramework.Singleton;
using VDFramework.Utility;

namespace FMODUtilityPackage.Core
{
	/// <summary>
	/// A Singleton that allows setting the EventReferences, BusPaths and Bus volumes in the inspector <br/>
	/// Holds the <see cref="FMODPathResolver"/> which resolves the Enums to actual FMOD events, busses and emitters
	/// </summary>
	[DefaultExecutionOrder(-1)] // Ensure the AudioManager is initialized before others
	public class AudioManager : Singleton<AudioManager>, ISerializationCallbackReceiver
	{
		public FMODPathResolver FMODPathResolver;

		[SerializeField]
		private List<InitialVolumePerBus> initialVolumes = new List<InitialVolumePerBus>();
		
		protected override void Awake()
		{
			/*
			 * DontDestroyOnLoad Singletons usually don't play nice when placed in a scene manually
			 * This is because if the same scene would be reloaded the singleton would exist twice
			 * We catch such a case before the Baseclass notices and destroy ourselves if we are the duplicate
			 * (This prevents the SingletonViolationException from being thrown)
			 */
			if (IsInitialized)
			{
				Destroy(this);
				return;
			}

			if (FMODPathResolver == null) // If EventPaths is null, the AudioManager was not present in the scene already (because otherwise the field would be deserialised)
			{
#if UNITY_EDITOR
				LogManager.LogInfo($"Add an {nameof(AudioManager)} to the scene manually for more control over the bus volumes at the start.");

				if (!FMODUnity.EventManager.IsInitialized) //EventManager is an editor script
				{
					FMODUnity.EventManager.Startup();
				}
#endif
				FMODPathResolver = new FMODPathResolver(true);
				OnBeforeSerialize();
			}

			base.Awake();
			FMODPathResolver.AddEmitters(gameObject);

			if (!transform.parent)
			{
				DontDestroyOnLoad(gameObject);
			}

			SetInitialVolumes();
		}

		private void SetInitialVolumes()
		{
			foreach (InitialVolumePerBus pair in initialVolumes)
			{
				if (pair.Key == 0) // Use 0 so people can freely rename the enum value | Technically this check is not necessary as SetBusVolume/Mute already performs this check
				{
					AudioVolumeManager.SetMasterVolume(pair.Value);
					AudioVolumeManager.SetMasterMute(pair.isMuted);
					continue;
				}

				AudioVolumeManager.SetBusVolume(pair.Key, pair.Value);
				AudioVolumeManager.SetBusMute(pair.Key, pair.isMuted);
			}
		}

		private void Reset()
		{
			for (int i = 0; i < initialVolumes.Count; i++)
			{
				initialVolumes[i] = InitialVolumePerBus.DefaultValue;
			}
		}

		public void OnBeforeSerialize()
		{
			int countBeforeResize = initialVolumes.Count;
			EnumDictionaryUtil.PopulateEnumDictionary<InitialVolumePerBus, AudioBus, float>(initialVolumes);
			int countAfterResize = initialVolumes.Count;

			if (countAfterResize > countBeforeResize) // If we have more values now then we had before, initialize the new ones with default values
			{
				for (int i = countBeforeResize; i < countAfterResize; i++)
				{
					InitialVolumePerBus initialVolumePerBus = initialVolumes[i];

					initialVolumes[i] = new InitialVolumePerBus(initialVolumePerBus.Key, 1, initialVolumePerBus.isMuted);
				}
			}
		}

		public void OnAfterDeserialize()
		{
		}

		[ContextMenu("Update Data")]
		private void UpdateDictionaries()
		{
			FMODPathResolver.OnBeforeSerialize();
		}
	}
}