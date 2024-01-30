using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.Structs;
using UnityEditor;
using UnityEngine;
using VDFramework.Extensions;
using VDFramework.Utility;

namespace FMODUtilityPackage.Core
{
	[Serializable]
	public class EventPaths : ISerializationCallbackReceiver
	{
		public const string MASTER_BUS_PATH = @"bus:/";

		[SerializeField]
		private List<EventReferencePerEvent> events = new List<EventReferencePerEvent>();

		[SerializeField]
		private List<BusPathPerBus> buses = new List<BusPathPerBus>();

		[SerializeField]
		private List<EventsPerEmitter> emitterEvents = new List<EventsPerEmitter>();

		private readonly Dictionary<EmitterType, StudioEventEmitter> emitters = new Dictionary<EmitterType, StudioEventEmitter>();

		public EventPaths()
		{
			buses.Add(new BusPathPerBus { Key = default, Value = MASTER_BUS_PATH });
		}

		/// <summary>
		/// Will also initialize all the Dictionaries and set the Event and Bus paths
		/// </summary>
		public EventPaths(bool setAllEventPaths) : this()
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
			foreach (EmitterType emitterType in default(EmitterType).GetValues())
			{
				StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
				emitter.EventReference = GetEventReferenceForEmitter(emitterType);

				emitters.Add(emitterType, emitter);
			}
		}

		public EventReference GetEventReference(AudioEventType audioEventType)
		{
			return events.First(item => item.Key.Equals(audioEventType)).Value;
		}

		public string GetPath(BusType busType)
		{
			return buses.First(item => item.Key.Equals(busType)).Value;
		}

		public StudioEventEmitter GetEmitter(EmitterType emitterType)
		{
			return emitters[emitterType];
		}

		private EventReference GetEventReferenceForEmitter(EmitterType emitterType)
		{
			AudioEventType audioEventType = emitterEvents.First(item => item.Key == emitterType).Value;
			return GetEventReference(audioEventType);
		}

#if UNITY_EDITOR
		private void SetEventPaths()
		{
			try
			{
				List<EditorEventRef> eventRefs = EventManager.Events;

				for (int i = 0; i < events.Count; i++)
				{
					EventReferencePerEvent pair = events[i];

					pair.Value = EventReference.Find(eventRefs[i].Path);
					events[i]  = pair;
				}
			}
			catch (Exception e)
			{
				if (EditorApplication.isPlaying)
				{
					Debug.LogException(e);
				}

				// ignore all outside of playmode
			}
		}
#else
		private void SetEventPaths()
		{
			try
			{
				TextAsset file = Resources.Load<TextAsset>("FmodUtils/EventPaths");

				string[] lines = file.ToString().Split(Environment.NewLine);
				EventType[] eventTypes = default(EventType).GetValues().ToArray();

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

			string[] busNames = default(BusType).GetNames().ToArray();

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
			EnumDictionaryUtil.PopulateEnumDictionary<EventReferencePerEvent, AudioEventType, EventReference>(events);

			EnumDictionaryUtil.PopulateEnumDictionary<BusPathPerBus, BusType, string>(buses);

			EnumDictionaryUtil.PopulateEnumDictionary<EventsPerEmitter, EmitterType, AudioEventType>(emitterEvents);
		}

		public void OnBeforeSerialize()
		{
			UpdateDictionaries();

			SetEventPaths();
		}

		public void OnAfterDeserialize()
		{
		}
	}
}