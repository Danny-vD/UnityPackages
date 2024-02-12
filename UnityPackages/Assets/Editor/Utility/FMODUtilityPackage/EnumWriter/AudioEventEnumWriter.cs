using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FMODUnity;
using FMODUtilityPackage.Enums;
using FMODUtilityPackage.Utility;
using UnityEditor.Compilation;
using UnityEngine;
using Utility.UtilityPackage;

namespace Utility.FMODUtilityPackage.EnumWriter
{
	/// <summary>
	/// Used to write all the event paths to a file in the resources folder to be able to retrieve the eventpaths without having to put an instantiated AudioManager in the scene
	/// </summary>
	public static class AudioEventEnumWriter
	{
		private const string scriptsFolder = "1. Scripts";
		private const string subFolder = "";

		private static readonly string typePath = @$"{Application.dataPath}/{scriptsFolder}{subFolder}/FMODUtilityPackage/Enums/";

		public static void WriteFmodEventsToEnum()
		{
			List<EditorEventRef> editorEventRefs = EventManager.Events;
			string[] eventNames = editorEventRefs.Select(eventref => eventref.Path).ToArray();

			string[] pathNames = new string[eventNames.Length];
			Array.Copy(eventNames, pathNames, eventNames.Length);

			WriteToResourcesUtil.WriteToResources(pathNames, "EventPaths.txt", "FMODUtilityPackage/");

			eventNames = EventPathToEnumValueUtil.ConvertEventPathToEnumValuesString(eventNames);
			
			WriteToFile(typePath, nameof(AudioEventType), eventNames, pathNames);
		}

		public static void WriteToFile(string path, string typeName, string[] values, string[] documentation)
		{
			WriteEnumValues(values, documentation, path, typeName);

			CompilationPipeline.RequestScriptCompilation();
		}

		private static void WriteEnumValues(string[] values, string[] documentation, string path, string typeName)
		{
			string fullPath = $"{path}{typeName}.cs";

			string content = File.ReadAllText(fullPath);

			string enumDeclaration = $"enum {typeName}";

			int startIndex = content.IndexOf(enumDeclaration, StringComparison.Ordinal); // Find the enum declaration

			if (startIndex == -1)
			{
				Debug.LogError("No valid enum declaration found in file");
				return;
			}

			startIndex = content.IndexOf('{', startIndex);     // Find the opening brace of the enum
			int closeIndex = content.IndexOf('}', startIndex); // Find the closing brace of the enum

			string afterEnum = string.Empty;

			if (closeIndex != -1)
			{
				afterEnum = content.Substring(closeIndex);
			}

			string beforeEnum = content.Substring(0, startIndex);
			StringBuilder builder = new StringBuilder(beforeEnum);
			builder.AppendLine("{");

			for (int i = 0; i < values.Length; i++)
			{
				builder.AppendLine("\t\t/// <FMODEventPath>");
				builder.AppendLine($"\t\t/// {documentation[i].Replace("&", "&amp;").Replace("'", "&apos;")}"); // XML does not allow special characters directly
				builder.AppendLine("\t\t/// </FMODEventPath>");
				builder.AppendLine($"\t\t{values[i]},");

				if (i != values.Length - 1)
				{
					builder.AppendLine();
				}
			}

			builder.Append("\t");

			builder.Append(afterEnum);

			File.WriteAllText(fullPath, builder.ToString());
		}
	}
}