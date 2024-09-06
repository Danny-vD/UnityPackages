using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.Utility;
using Utility.UtilityPackage;

namespace Utility.FMODUtilityPackage
{
	/// <summary>
	/// Used to write all the event paths to a file in the resources folder to be able to retrieve the eventpaths without having to put an instantiated AudioManager in the scene
	/// </summary>
	public static class AudioEventEnumWriter
	{
		private const string subFolder = "/zPackages"; // First slash is required so that it is possible to leave this empty

		private static readonly string typePath = @$"{subFolder}/FMODUtilityPackage/Enums/";

		public static void WriteFmodEventsToEnum()
		{
			List<EditorEventRef> editorEventRefs = EventManager.Events;
			string[] eventNames = editorEventRefs.Select(eventref => eventref.Path).ToArray();

			string[] pathNames = new string[eventNames.Length];
			Array.Copy(eventNames, pathNames, eventNames.Length);

			WriteToResourcesUtil.WriteToResources(pathNames, "EventPaths.txt", "FMODUtilityPackage/");

			eventNames = EventPathToEnumValueUtil.ConvertEventPathToEnumValuesString(eventNames);
			
			EnumWriter.WriteEnumValuesAutomaticPath<AudioEventType>("zPackages/", eventNames, pathNames, "FMODEventPath");
		}
	}
}