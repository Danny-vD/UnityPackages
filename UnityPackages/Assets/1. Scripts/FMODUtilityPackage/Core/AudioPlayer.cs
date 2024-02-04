﻿using System;
using System.Reflection;
using FMOD.Studio;
using FMODUnity;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Structs;
using UnityEngine;

namespace FMODUtilityPackage.Core
{
	/// <summary>
	/// A static class that serves as an API for playing audio and managing the global event emitters, it also provides EventReferences and EventInstances for manual control
	/// </summary>
	public static class AudioPlayer
	{
		// Reflection because the StudioEventEmitter internally caches the event after setting (and playing) it once, so you cannot normally set it again
		private static readonly MethodInfo lookup = typeof(StudioEventEmitter).GetMethod("Lookup", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly FieldInfo emitterEventInstance = typeof(StudioEventEmitter).GetField("instance", BindingFlags.Instance | BindingFlags.NonPublic);
		
		public static void PlayEmitter(EmitterType emitter)
		{
			AudioManager.Instance.EventPaths.GetEmitter(emitter).Play();
		}

		public static void ToggleEmitter(EmitterType emitterType)
		{
			StudioEventEmitter emitter = AudioManager.Instance.EventPaths.GetEmitter(emitterType);

			if (emitter.IsPlaying())
			{
				emitter.Stop();
				return;
			}

			emitter.Play();
		}

		public static void StopEmitter(EmitterType emitter)
		{
			AudioManager.Instance.EventPaths.GetEmitter(emitter).Stop();
		}
		
		public static void SetEmitterEvent(EmitterType emitter, AudioEventType audioEvent)
		{
			StudioEventEmitter studioEventEmitter = AudioManager.Instance.EventPaths.GetEmitter(emitter);
			bool isPlaying = studioEventEmitter.IsPlaying();

			studioEventEmitter.Stop(); // By telling the emitter to stop we also tell it to release the current instance
			
			EventReference eventReference = AudioManager.Instance.EventPaths.GetEventReference(audioEvent);
			studioEventEmitter.EventReference = eventReference;

			eventReference.GetEventDescription().createInstance(out EventInstance instance);
			
			emitterEventInstance.SetValue(studioEventEmitter, instance);
			
			// Lookup updates the emitters internal eventDescription
			lookup.Invoke(studioEventEmitter, null);
			
			if (isPlaying)
			{
				studioEventEmitter.Play();
			}
		}

		/// <summary>
		/// Play a 3D event which is attached to the given GameObject
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		/// <param name="location">The object to attach this event to</param>
		public static void PlayOneShot3D(AudioEventType audioEvent, GameObject location)
		{
			RuntimeManager.PlayOneShotAttached(AudioManager.Instance.EventPaths.GetEventReference(audioEvent), location);
		}

		/// <summary>
		/// Play a 3D event at the position of the AudioManager
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		public static void PlayOneShot3D(AudioEventType audioEvent)
		{
			if (!AudioManager.IsInitialized)
			{
				throw new Exception("Audiomanager is not initialised yet, check if it is present in the scene or whether you called from Awake");
			}

			RuntimeManager.PlayOneShotAttached(AudioManager.Instance.EventPaths.GetEventReference(audioEvent), AudioManager.Instance.gameObject);
		}

		/// <summary>
		/// Play a 3D event with the given parameters which is attached to the given GameObject
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		/// <param name="parameters">The parameters to use</param>
		/// <param name="gameObject">The object to attach this even to</param>
		public static void PlayOneShot3D(AudioEventType audioEvent, EventParameters parameters, GameObject gameObject)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(AudioManager.Instance.EventPaths.GetEventReference(audioEvent));
			eventInstance.set3DAttributes(gameObject.transform.To3DAttributes());

			eventInstance.SetParameters(parameters);

			eventInstance.start();
			eventInstance.release(); //Release each event instance immediately, they are fire and forget, one-shot instances. 
		}

		/// <summary>
		/// Play a 2D event
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		public static void PlayOneShot2D(AudioEventType audioEvent)
		{
			RuntimeManager.PlayOneShot(AudioManager.Instance.EventPaths.GetEventReference(audioEvent));
		}

		/// <summary>
		/// Play a 2D event with parameters
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		/// <param name="parameters">The parameters to use</param>
		public static void PlayOneShot2D(AudioEventType audioEvent, EventParameters parameters)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(AudioManager.Instance.EventPaths.GetEventReference(audioEvent));

			eventInstance.SetParameters(parameters);

			eventInstance.start();
			eventInstance.release(); //Release each event instance immediately, they are fire and forget, one-shot instances. 
		}

		/// <summary>
		/// Returns an instance of the specified event with given parameters, you will need to start, stop and release it manually
		/// </summary>
		public static EventInstance GetEventInstance(AudioEventType audioEvent, EventParameters parameters)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(AudioManager.Instance.EventPaths.GetEventReference(audioEvent));

			eventInstance.SetParameters(parameters);

			return eventInstance;
		}

		/// <summary>
		/// Returns an instance of the specified event, you will need to start, stop and release it manually
		/// </summary>
		public static EventInstance GetEventInstance(AudioEventType audioEvent)
		{
			EventReference eventReference = GetEventReference(audioEvent);
			eventReference.GetEventDescription().createInstance(out EventInstance instance);

			return instance;
		}

		/// <summary>
		/// Returns a reference of the specified event, from there you can request specific details about the event
		/// </summary>
		public static EventReference GetEventReference(AudioEventType audioEvent)
		{
			return AudioManager.Instance.EventPaths.GetEventReference(audioEvent);
		}
	}
}