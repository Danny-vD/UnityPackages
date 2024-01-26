using System;
using FMOD.Studio;
using FMODUnity;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.ExtentionMethods;
using FMODUtilityPackage.Structs;
using UnityEngine;
using EventType = FMODUtilityPackage.Enums.EventType;

namespace FMODUtilityPackage.Core
{
	public static class AudioPlayer
	{
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
		
		public static void SetEmitterEvent(EmitterType emitter, EventType @event)
		{
			StudioEventEmitter studioEventEmitter = AudioManager.Instance.EventPaths.GetEmitter(emitter);
			bool isPlaying = studioEventEmitter.IsPlaying();

			studioEventEmitter.Stop(); // By telling the emitter to stop we also tell it to release the current instance
			studioEventEmitter.EventReference = AudioManager.Instance.EventPaths.GetEventReference(@event);
			
			if (isPlaying)
			{
				studioEventEmitter.Play();
			}
		}

		/// <summary>
		/// Play a 3D event which is attached to the given GameObject
		/// </summary>
		/// <param name="event">The event to play</param>
		/// <param name="location">The object to attach this event to</param>
		public static void PlayOneShot3D(EventType @event, GameObject location)
		{
			RuntimeManager.PlayOneShotAttached(AudioManager.Instance.EventPaths.GetEventReference(@event), location);
		}

		/// <summary>
		/// Play a 3D event at the position of the AudioManager
		/// </summary>
		/// <param name="event">The event to play</param>
		public static void PlayOneShot3D(EventType @event)
		{
			if (!AudioManager.IsInitialized)
			{
				throw new Exception("Audiomanager is not initialised yet, check if it is present in the scene or whether you called from Awake");
			}

			RuntimeManager.PlayOneShotAttached(AudioManager.Instance.EventPaths.GetEventReference(@event), AudioManager.Instance.gameObject);
		}

		/// <summary>
		/// Play a 3D event with the given parameters which is attached to the given GameObject
		/// </summary>
		/// <param name="event">The event to play</param>
		/// <param name="parameters">The parameters to use</param>
		/// <param name="gameObject">The object to attach this even to</param>
		public static void PlayOneShot3D(EventType @event, EventParameters parameters, GameObject gameObject)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(AudioManager.Instance.EventPaths.GetEventReference(@event));
			eventInstance.set3DAttributes(gameObject.transform.To3DAttributes());

			eventInstance.SetParameters(parameters);

			eventInstance.start();
			eventInstance.release(); //Release each event instance immediately, they are fire and forget, one-shot instances. 
		}

		/// <summary>
		/// Play a 2D event
		/// </summary>
		/// <param name="event">The event to play</param>
		public static void PlayOneShot2D(EventType @event)
		{
			RuntimeManager.PlayOneShot(AudioManager.Instance.EventPaths.GetEventReference(@event));
		}

		/// <summary>
		/// Play a 2D event with parameters
		/// </summary>
		/// <param name="event">The event to play</param>
		/// <param name="parameters">The parameters to use</param>
		public static void PlayOneShot2D(EventType @event, EventParameters parameters)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(AudioManager.Instance.EventPaths.GetEventReference(@event));

			eventInstance.SetParameters(parameters);

			eventInstance.start();
			eventInstance.release(); //Release each event instance immediately, they are fire and forget, one-shot instances. 
		}

		/// <summary>
		/// Returns an instance of the specified event with given parameters, you will need to start, stop and release it manually
		/// </summary>
		public static EventInstance GetEventInstance(EventType @event, EventParameters parameters)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(AudioManager.Instance.EventPaths.GetEventReference(@event));

			eventInstance.SetParameters(parameters);

			return eventInstance;
		}

		/// <summary>
		/// Returns an instance of the specified event, you will need to start, stop and release it manually
		/// </summary>
		public static EventInstance GetEventInstance(EventType @event)
		{
			return RuntimeManager.CreateInstance(GetEventReference(@event));
		}

		/// <summary>
		/// Returns a reference of the specified event, from there you can request specific details about the event
		/// </summary>
		public static EventReference GetEventReference(EventType @event)
		{
			return AudioManager.Instance.EventPaths.GetEventReference(@event);
		}
	}
}