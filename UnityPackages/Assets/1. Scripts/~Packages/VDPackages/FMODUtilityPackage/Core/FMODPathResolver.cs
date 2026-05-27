using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using VDFramework.Extensions;
using VDFramework.Logger;
using VDFramework.Utility;
using VDPackages.FMODUtilityPackage.Enums;
using VDPackages.FMODUtilityPackage.Structs;
using VDPackages.FMODUtilityPackage.Utility;
#if !UNITY_EDITOR
using FMODUtilityPackage.Constants;
#endif

namespace VDPackages.FMODUtilityPackage.Core
{
	/// <summary>
	/// Utility class that resolves <see cref="AudioEvent"/>s, <see cref="AudioBus"/>ses and <see cref="GlobalEmitter"/>s to FMOD events, busses and emitters 
	/// </summary>
	[Serializable]
	public class FMODPathResolver : ISerializationCallbackReceiver
	{
		public const string MASTER_BUS_PATH = "bus:/";

		private static Dictionary<AudioBus, Bus> busPerAudioBusEnum = new Dictionary<AudioBus, Bus>();
		
		[SerializeField]
		private List<EventReferencePerEvent> events = new List<EventReferencePerEvent>();

		[SerializeField]
		private List<BusPathPerBus> buses = new List<BusPathPerBus>();

		[SerializeField]
		private List<EventsPerEmitter> emitterEvents = new List<EventsPerEmitter>();

		private readonly Dictionary<GlobalEmitter, StudioEventEmitter> emitters = new Dictionary<GlobalEmitter, StudioEventEmitter>();

		public FMODPathResolver()
		{
			buses.Add(new BusPathPerBus { Key = default, Value = MASTER_BUS_PATH });
		}

		/// <summary>
		/// Will also initialize all the Dictionaries and set the Event and Bus paths
		/// </summary>
		public FMODPathResolver(bool setAllEventPaths) : this()
		{
			if (setAllEventPaths)
			{
				UpdateDictionaries();

				SetEventPaths();
				SetBusPaths();
			}
		}

		public void AddEmitters(GameObject gameObject)
		{
			foreach (GlobalEmitter emitterType in default(GlobalEmitter).GetValues())
			{
				StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
				emitter.EventReference = GetEventReferenceForEmitter(emitterType);

				emitters.Add(emitterType, emitter);
			}
		}

		public EventReference GetEventReference(AudioEvent audioEvent)
		{
			return events.First(item => item.Key.Equals(audioEvent)).Value;
		}

		public Bus GetAudioBus(AudioBus audioBus)
		{
			if (!busPerAudioBusEnum.TryGetValue(audioBus, out Bus bus))
			{
				bus = GetBusFromPath(GetAudioBusPath(audioBus));
				busPerAudioBusEnum.Add(audioBus, bus);
			}

			return bus;
		}
		
		public string GetAudioBusPath(AudioBus audioBus)
		{
			return buses.First(item => item.Key.Equals(audioBus)).Value;
		}
		
		public StudioEventEmitter GetEmitter(GlobalEmitter globalEmitter)
		{
			return emitters[globalEmitter];
		}

		public static Bus GetBusFromPath(string busPath)
		{
			return RuntimeManager.GetBus(busPath);
		}
		
		private EventReference GetEventReferenceForEmitter(GlobalEmitter globalEmitter)
		{
			AudioEvent audioEvent = emitterEvents.First(item => item.Key == globalEmitter).Value;
			return GetEventReference(audioEvent);
		}

#if UNITY_EDITOR
		private void SetEventPaths()
		{
			try
			{
				List<EditorEventRef> eventRefs = EventManager.Events;
				string[] eventPaths = eventRefs.Select(eventref => eventref.Path).ToArray();

				AudioEvent[] enumValues = EventPathToEnumValueUtil.ConvertEventPathToEnumValues(eventPaths);

				for (int i = 0; i < enumValues.Length; i++)
				{
					EventReferencePerEvent pair = default;
					pair.Key = enumValues[i];

					// Use the enum representation of the eventPath because EventManager.Events returns the events in a different order on different systems which would cause a mismapping of the EventReferences
					int index = events.FindIndex(referencePerEvent => referencePerEvent.Key.Equals(enumValues[i]));

					if (index == -1)
					{
						// Technically should never happen since UpdateDictionaries was called before this | If it does, there is a problem in FindIndex above
						LogManager.LogError($"Event paths do not contain a pair for {enumValues[i].ToString()}");
						continue;
					}

					pair.Value    = EventReference.Find(eventRefs[i].Path);
					events[index] = pair;
				}
			}
			catch (Exception e)
			{
				if (UnityEditor.EditorApplication.isPlaying)
				{
					LogManager.LogException(e);
				}

				// ignore all exceptions outside of playmode
			}
		}
#else
		private void SetEventPaths()
		{
			try
			{
				TextAsset file = Resources.Load<TextAsset>(ResourcesPathConstants.PATH);

				string[] lines = file.ToString().Split(Environment.NewLine);
				AudioEvent[] eventTypes = default(AudioEvent).GetValues().ToArray();

				for (int i = 0; i < events.Count; i++)
				{
					EventReferencePerEvent pair = new EventReferencePerEvent
					{
						Key = eventTypes[i],
						Value = RuntimeManager.PathToEventReference(lines[i]),
					};

					events[i] = pair;
				}
			}
			catch
			{
				// ignore all outside of editor
			}
		}
#endif

		private void SetBusPaths()
		{
			int busCount = buses.Count;

			if (busCount <= 1) // The master bus is already taken care of in the constructor
			{
				return;
			}

			string[] busNames = default(AudioBus).GetNames().ToArray();

			// Start at 1 because 0 is always the master bus
			for (int i = 1; i < busCount; i++)
			{
				BusPathPerBus pathPerBus = buses[i];

				// Bus paths always start with bus:/ which is the Master Bus Path 
				pathPerBus.Value = MASTER_BUS_PATH + busNames[i];

				buses[i] = pathPerBus;
			}
		}

		private void UpdateDictionaries()
		{
			//TODO replace the lists with SerializableDictionaries (check the serializableDictionary drawer how to properly display it in the inspector)
			EnumDictionaryUtil.PopulateEnumDictionary<EventReferencePerEvent, AudioEvent, EventReference>(events);

			EnumDictionaryUtil.PopulateEnumDictionary<BusPathPerBus, AudioBus, string>(buses);

			EnumDictionaryUtil.PopulateEnumDictionary<EventsPerEmitter, GlobalEmitter, AudioEvent>(emitterEvents);
		}

		public void OnBeforeSerialize()
		{
			UpdateDictionaries();

#if UNITY_EDITOR
			if (FMODUnity.EventManager.IsInitialized && !UnityEditor.EditorApplication.isPlaying) //EventManager is an editor script
#endif
				SetEventPaths();
		}

		public void OnAfterDeserialize()
		{
		}
	}
}