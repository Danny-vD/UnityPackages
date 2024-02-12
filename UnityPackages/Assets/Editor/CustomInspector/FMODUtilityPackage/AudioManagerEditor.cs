using System;
using System.Linq;
using FMODUnity;
using FMODUtilityPackage.Core;
using FMODUtilityPackage.Enums;
using UnityEditor;
using UnityEngine;
using Utility.FMODUtilityPackage.EnumWriter;
using VDFramework.Extensions;
using static Utility.UtilityPackage.EditorUtils;

namespace CustomInspector.FMODUtilityPackage
{
	[CustomEditor(typeof(AudioManager))]
	public class AudioManagerEditor : Editor
	{
		// AudioManager
		private AudioManager audioManager;
		private bool showBusVolume;
		private bool[] busVolumeFoldout;

		// EventPaths
		private bool showEventPaths;
		private bool[] eventPathsFoldout;

		private bool showBuses;
		private bool[] busesFoldout;

		private bool showEmitterEvents;
		private bool[] emitterEventsFoldout;

		// Fmod
		private static Type eventBrowser;

		// Icons
		private static Texture fmodIcon;

		private static Texture folderIconClosed;
		private static Texture folderIconOpen;

		private static Texture[] eventIcon;
		private static Texture[] busIcon;

		//////////////////////////////////////////////////

		// AudioManager
		private SerializedProperty initialVolumes;

		// EventPaths
		private SerializedProperty events;
		private SerializedProperty buses;
		private SerializedProperty emitterEvents;

		private void OnEnable()
		{
			if (ValidateEventTypeEnum())
			{
				return;
			}

			// AudioManager
			audioManager = (AudioManager)target;
			
			initialVolumes   = serializedObject.FindProperty("initialVolumes");
			busVolumeFoldout = new bool[initialVolumes.arraySize];

			// EventPaths
			events        = serializedObject.FindProperty("EventPaths.events");
			buses         = serializedObject.FindProperty("EventPaths.buses");
			emitterEvents = serializedObject.FindProperty("EventPaths.emitterEvents");

			eventPathsFoldout    = new bool[events.arraySize];
			busesFoldout         = new bool[buses.arraySize];
			emitterEventsFoldout = new bool[emitterEvents.arraySize];

			// Icons
			fmodIcon = EditorUtils.LoadImage("StudioIcon.png");

			folderIconClosed = EditorUtils.LoadImage("FolderIconClosed.png");
			folderIconOpen   = EditorUtils.LoadImage("FolderIconOpen.png");

			eventIcon = new[]
			{
				EditorUtils.LoadImage("EventIcon.png"),
			};

			busIcon = new[]
			{
				GetTexture("AudioManager/BusIcon.png"),
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawEventPaths();
			DrawBusPaths();

			DrawSeperatorLine(); //-------------------

			DrawEmitterEvents();

			DrawSeperatorLine(); //-------------------

			DrawInitialVolumes();

			DrawSeperatorLine(); //-------------------

			DrawEventBrowserButton();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawEventPaths()
		{
			if (IsFoldOut(ref showEventPaths, showEventPaths ? folderIconOpen : folderIconClosed, "Event Paths"))
			{
				DrawFoldoutKeyValueArray<AudioEventType>(events, "key", "value", eventPathsFoldout, eventIcon, new GUIContent("Path"));
			}
		}

		private void DrawBusPaths()
		{
			if (IsFoldOut(ref showBuses, showBuses ? folderIconOpen : folderIconClosed, "Bus Paths"))
			{
				DrawFoldoutKeyValueArray<BusType>(buses, "key", "value", busesFoldout, busIcon, DrawElement);
			}

			void DrawElement(int index, SerializedProperty key, SerializedProperty value)
			{
				string path = value.stringValue;

				if (index == 0)
				{
					EditorGUILayout.LabelField("Path", path);
					return;
				}

				if (!path.StartsWith("Bus:/") && !path.StartsWith("bus:/"))
				{
					if (value.stringValue == string.Empty)
					{
						value.stringValue = $"bus:/{key.enumNames[key.enumValueIndex]}";
					}
					else
					{
						value.stringValue = path.Insert(0, "bus:/");
					}
				}

				EditorGUILayout.PropertyField(value, new GUIContent("Path"));
			}
		}

		private void DrawEmitterEvents()
		{
			if (IsFoldOut(ref showEmitterEvents, showEmitterEvents ? folderIconOpen : folderIconClosed, "Emitters"))
			{
				DrawFoldoutKeyValueArray<EmitterType>(emitterEvents, "key", "value", emitterEventsFoldout,
					new GUIContent("Event to play", eventIcon[0]));
			}
		}

		private void DrawInitialVolumes()
		{
			if (IsFoldOut(ref showBusVolume, showBusVolume ? folderIconOpen : folderIconClosed, "Volume"))
			{
				DrawFoldoutKeyValueArray<BusType>(initialVolumes, "key", busVolumeFoldout, busIcon, DrawStruct);
			}

			void DrawStruct(int i, SerializedProperty @struct)
			{
				SerializedProperty value = @struct.FindPropertyRelative("value");
				SerializedProperty isMuted = @struct.FindPropertyRelative("isMuted");

				float volume = value.floatValue;

				EditorGUILayout.PropertyField(isMuted, new GUIContent("Mute"));
				value.floatValue = EditorGUILayout.Slider("Volume", volume, 0.0f, 1.0f);
			}
		}

		private static void DrawEventBrowserButton()
		{
			if (GUILayout.Button(new GUIContent("Event Browser", fmodIcon)))
			{
				EventBrowser.ShowWindow();
			}
		}

		/// <summary>
		/// Will update the EventType enum to match the amount of events if necessary
		/// </summary>
		/// <returns>Whether or not a recompile is necessary</returns>
		private static bool ValidateEventTypeEnum()
		{
			bool needRecompile = default(AudioEventType).GetValues().Count() != EventManager.Events.Count;

			if (needRecompile)
			{
				AudioEventEnumWriter.WriteFmodEventsToEnum();
			}

			return needRecompile;
		}

		[MenuItem("FMOD/Import Events")] // Make the validation a menu command that can be triggered from anywhere
		private static void ValidateEventTypeEnumMenuItem(MenuCommand command)
		{
			_ = ValidateEventTypeEnum();
		}
	}
}