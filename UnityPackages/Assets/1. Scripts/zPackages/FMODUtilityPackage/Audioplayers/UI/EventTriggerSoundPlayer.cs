using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Structs;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using VDFramework;
using VDFramework.UnityExtensions;

namespace FMODUtilityPackage.Audioplayers.UI
{
	/// <summary>
	/// Play an <see cref="EventTrigger"/> as a reaction to an <see cref="UnityEngine.EventSystems"/> event 
	/// </summary>
	public class EventTriggerAudioPlayer : BetterMonoBehaviour
	{
		[Serializable]
		public struct AudioEventData
		{
			[Tooltip("Share this event instance between all EventTriggerSoundPlayers")]
			public bool IsGlobalInstance;

			public AudioEventType AudioEvent;
			public PlayState PlayState;
			public EventParameters Parameters;
		}

		[Header("On EventTrigger"), SerializeField, Tooltip("Allow any event to fade out when it is stopped")]
		private bool allowFadeoutOnStop = true;

		[Header("On Disable"), SerializeField, Tooltip("Stop all instances when this object is disabled")]
		private bool stopInstancesOnDisable;

		[SerializeField, Tooltip("Stop all global instances when this object is disabled")]
		private bool stopGlobalInstancesOnDisable;

		[SerializeField, Tooltip("Allow the playing events to fade out when this object is disabled (also applies to global events if they are stopped)")]
		private bool allowFadeoutOnDisable = false;

		[Header("On Destroy"), SerializeField, Tooltip("Allow the playing events to fade out when this object is destroyed (also applies to global events if they are stopped)")]
		private bool allowFadeoutOnDestroy = false;

		[SerializeField, Tooltip("Stop (and release memory of) all global instances when this object is destroyed")]
		private bool stopGlobalInstancesOnDestroy = true;

		[Space, SerializeField]
		private SerializableDictionary<EventTriggerType, AudioEventData[]> audioDataPerTriggerType;

		// Static to allow sharing between different classes
		private static readonly Dictionary<AudioEventType, EventInstance> staticInstancePerEventType = new Dictionary<AudioEventType, EventInstance>();

		private readonly Dictionary<AudioEventType, EventInstance> instancePerEventType = new Dictionary<AudioEventType, EventInstance>();

		private EventTrigger eventTrigger;

		private void Awake()
		{
			eventTrigger = this.EnsureComponent<EventTrigger>();

			foreach (KeyValuePair<EventTriggerType, AudioEventData[]> eventDataPerTrigger in audioDataPerTriggerType)
			{
				EventTrigger.Entry entry = new EventTrigger.Entry
				{
					eventID = eventDataPerTrigger.Key,
				};

				foreach (AudioEventData audioEventData in eventDataPerTrigger.Value)
				{
					CacheInstanceIfNeeded(audioEventData);

					entry.callback.AddListener(GetCallback(audioEventData));
				}

				eventTrigger.triggers.Add(entry);
			}
		}

		private void OnDisable()
		{
			STOP_MODE stopMode = allowFadeoutOnDisable ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;

			if (stopInstancesOnDisable)
			{
				foreach (KeyValuePair<AudioEventType, EventInstance> keyValuePair in instancePerEventType)
				{
					keyValuePair.Value.stop(stopMode);
				}
			}

			if (stopGlobalInstancesOnDisable)
			{
				foreach (KeyValuePair<AudioEventType, EventInstance> keyValuePair in staticInstancePerEventType)
				{
					keyValuePair.Value.stop(stopMode);
				}
			}
		}

		public static void StopStaticInstance(AudioEventType audioEventType, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT, bool releaseMemory = true)
		{
			if (staticInstancePerEventType.ContainsKey(audioEventType))
			{
				EventInstance instance = staticInstancePerEventType[audioEventType];
				instance.stop(stopMode);

				if (releaseMemory)
				{
					instance.release();
					staticInstancePerEventType.Remove(audioEventType);
				}
			}
		}

		public static void StopAllStaticInstances(STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT, bool releaseMemory = true)
		{
			foreach (KeyValuePair<AudioEventType, EventInstance> keyValuePair in staticInstancePerEventType)
			{
				EventInstance instance = keyValuePair.Value;

				instance.stop(stopMode);

				if (releaseMemory)
				{
					instance.release();
				}
			}

			if (releaseMemory)
			{
				staticInstancePerEventType.Clear();
			}
		}

		public void AddEventTriggerHandler(EventTriggerType triggerType, AudioEventData eventData)
		{
			EventTrigger.Entry entry = eventTrigger.triggers.FirstOrDefault(trigger => trigger.eventID == triggerType);

			bool entryExisted = entry != null;

			if (!entryExisted)
			{
				entry = new EventTrigger.Entry
				{
					eventID = triggerType,
				};
			}

			CacheInstanceIfNeeded(eventData);

			entry.callback.AddListener(GetCallback(eventData));

			if (!entryExisted)
			{
				eventTrigger.triggers.Add(entry);
			}
		}

		private void CacheInstanceIfNeeded(AudioEventData audioEventData)
		{
			if (audioEventData.IsGlobalInstance)
			{
				if (!staticInstancePerEventType.ContainsKey(audioEventData.AudioEvent))
				{
					staticInstancePerEventType.Add(audioEventData.AudioEvent, AudioPlayer.GetEventInstance(audioEventData.AudioEvent));
				}
			}
			else
			{
				if (!instancePerEventType.ContainsKey(audioEventData.AudioEvent))
				{
					instancePerEventType.Add(audioEventData.AudioEvent, AudioPlayer.GetEventInstance(audioEventData.AudioEvent));
				}
			}
		}

		private UnityAction<BaseEventData> GetCallback(AudioEventData audioEventData)
		{
			EventInstance instance = audioEventData.IsGlobalInstance ? staticInstancePerEventType[audioEventData.AudioEvent] : instancePerEventType[audioEventData.AudioEvent];

			return audioEventData.PlayState switch
			{
				PlayState.Play => delegate
				{
					instance.start();

					instance.SetParameters(audioEventData.Parameters);
				},
				PlayState.PlayIfNotPlaying => delegate
				{
					instance.getPlaybackState(out PLAYBACK_STATE state);

					if (state is PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)
					{
						instance.start();
					}

					instance.SetParameters(audioEventData.Parameters);
				},
				PlayState.Resume => delegate
				{
					instance.setPaused(false);

					instance.SetParameters(audioEventData.Parameters);
				},
				PlayState.Pause => delegate
				{
					instance.setPaused(true);

					instance.SetParameters(audioEventData.Parameters);
				},
				PlayState.TogglePause => delegate
				{
					instance.getPaused(out bool paused);
					instance.setPaused(!paused);

					instance.SetParameters(audioEventData.Parameters);
				},
				PlayState.Stop => delegate
				{
					instance.stop(allowFadeoutOnStop ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);

					instance.SetParameters(audioEventData.Parameters);
				},
				PlayState.ParametersOnly => delegate { instance.SetParameters(audioEventData.Parameters); },
				_ => throw new ArgumentOutOfRangeException(nameof(audioEventData.PlayState), audioEventData.PlayState, null),
			};
		}

		private void OnDestroy()
		{
			STOP_MODE stopMode = allowFadeoutOnDestroy ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE;

			// Always stop the local instances on Destroy, because there is no other way to stop them afterwards
			foreach (KeyValuePair<AudioEventType, EventInstance> pair in instancePerEventType)
			{
				EventInstance instance = pair.Value;
				instance.stop(stopMode);
				instance.release();
			}

			if (stopGlobalInstancesOnDestroy)
			{
				StopAllStaticInstances(stopMode);
			}
		}
	}
}