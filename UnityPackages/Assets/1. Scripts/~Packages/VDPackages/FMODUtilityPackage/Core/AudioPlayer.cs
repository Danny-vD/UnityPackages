using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using VDPackages.FMODUtilityPackage.Enums;
using VDPackages.FMODUtilityPackage.ExtentionMethods;
using VDPackages.FMODUtilityPackage.Structs;

namespace VDPackages.FMODUtilityPackage.Core
{
	/// <summary>
	/// A static class that serves as an API for playing audio and managing the global event emitters, it also provides EventReferences and EventInstances for manual control
	/// </summary>
	public static class AudioPlayer
	{
		// Reflection because the StudioEventEmitter internally caches the event after setting (and playing) it once, so you cannot normally set it again

		/// <summary>
		/// <see cref="StudioEventEmitter.Lookup"/>
		/// </summary>
		private static readonly MethodInfo lookup = typeof(StudioEventEmitter).GetMethod("Lookup", BindingFlags.Instance | BindingFlags.NonPublic);

		private static readonly FieldInfo emitterEventInstance = typeof(StudioEventEmitter).GetField("instance", BindingFlags.Instance | BindingFlags.NonPublic);

		public static void PlayEmitter(GlobalEmitter globalEmitter)
		{
			GetEmitter(globalEmitter).Play();
		}

		public static void ToggleEmitter(GlobalEmitter globalEmitter)
		{
			StudioEventEmitter emitter = GetEmitter(globalEmitter);

			if (emitter.IsPlaying())
			{
				emitter.Stop();
				return;
			}

			emitter.Play();
		}

		public static void StopEmitter(GlobalEmitter globalEmitter)
		{
			GetEmitter(globalEmitter).Stop();
		}

		public static void SetEmitterEvent(GlobalEmitter globalEmitter, AudioEvent audioEvent)
		{
			StudioEventEmitter studioEventEmitter = GetEmitter(globalEmitter);
			bool wasPlaying = studioEventEmitter.IsPlaying();

			studioEventEmitter.Stop(); // By telling the emitter to stop we also tell it to release the current instance

			EventReference eventReference = GetEventReference(audioEvent);
			studioEventEmitter.EventReference = eventReference;

			eventReference.GetEventDescription().createInstance(out EventInstance instance);

			emitterEventInstance.SetValue(studioEventEmitter, instance);

			// Lookup updates the emitters internal eventDescription
			lookup.Invoke(studioEventEmitter, null);

			if (wasPlaying)
			{
				studioEventEmitter.Play();
			}
		}

		/// <summary>
		/// Play a 3D event which is attached to the given GameObject
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		/// <param name="location">The object to attach this event to</param>
		public static void PlayOneShot3D(AudioEvent audioEvent, GameObject location)
		{
			RuntimeManager.PlayOneShotAttached(GetEventReference(audioEvent), location);
		}

		/// <summary>
		/// Play a 3D event at the position of the AudioManager
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		public static void PlayOneShot3D(AudioEvent audioEvent)
		{
			if (!AudioManager.IsInitialized)
			{
				// Does not break the code. But since we play at the position of the AudioManager, it is unlikely that it was intended that it was not present already
				throw new Exception("Audiomanager is not initialised yet, check if it is present in the scene");
			}

			RuntimeManager.PlayOneShotAttached(GetEventReference(audioEvent), AudioManager.Instance.gameObject);
		}

		/// <summary>
		/// Play a 3D event with the given parameters which is attached to the given GameObject
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		/// <param name="parameters">The parameters to use</param>
		/// <param name="gameObject">The object to attach this event to</param>
		public static void PlayOneShot3D(AudioEvent audioEvent, EventParameters parameters, GameObject gameObject)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(GetEventReference(audioEvent));
			eventInstance.set3DAttributes(gameObject.transform.To3DAttributes());

			eventInstance.SetParameters(parameters);

			eventInstance.start();
			eventInstance.release(); // Release each event instance immediately, they are fire and forget, one-shot instances. 
		}

		/// <summary>
		/// Play a 2D event
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		public static void PlayOneShot2D(AudioEvent audioEvent)
		{
			RuntimeManager.PlayOneShot(GetEventReference(audioEvent));
		}

		/// <summary>
		/// Play a 2D event with parameters
		/// </summary>
		/// <param name="audioEvent">The event to play</param>
		/// <param name="parameters">The parameters to use</param>
		public static void PlayOneShot2D(AudioEvent audioEvent, EventParameters parameters)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(GetEventReference(audioEvent));

			eventInstance.SetParameters(parameters);

			eventInstance.start();
			eventInstance.release(); //Release each event instance immediately, they are fire and forget, one-shot instances. 
		}

		/// <summary>
		/// Returns an instance of the specified event with given parameters, you will need to start, stop and release it manually
		/// </summary>
		public static EventInstance GetEventInstance(AudioEvent audioEvent, EventParameters parameters)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(GetEventReference(audioEvent));

			eventInstance.SetParameters(parameters);

			return eventInstance;
		}

		/// <summary>
		/// Returns an instance of the specified event, you will need to start, stop and release it manually
		/// </summary>
		public static EventInstance GetEventInstance(AudioEvent audioEvent)
		{
			EventReference eventReference = GetEventReference(audioEvent);
			eventReference.GetEventDescription().createInstance(out EventInstance instance);

			return instance;
		}

		/// <summary>
		/// Returns a reference of the specified event, from there you can request specific details about the event
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EventReference GetEventReference(AudioEvent audioEvent)
		{
			return AudioManager.Instance.FMODPathResolver.GetEventReference(audioEvent);
		}

		/// <summary>
		/// Returns a reference to the specified emitter
		/// </summary>
		public static StudioEventEmitter GetEmitter(GlobalEmitter audioGlobalEmitter)
		{
			return AudioManager.Instance.FMODPathResolver.GetEmitter(audioGlobalEmitter);
		}
	}
}