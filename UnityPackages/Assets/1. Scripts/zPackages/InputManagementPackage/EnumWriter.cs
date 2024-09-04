using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.Compilation;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace InputManagementPackage
{
	public static class EnumWriter
	{
#if UNITY_EDITOR
		private const string scriptsFolder = "1. Scripts";

		private static readonly string typePath = @$"{Application.dataPath}\{scriptsFolder}\";

		public static void WriteToEnum<TEnum>(string subPath, IEnumerable<string> values, IEnumerable<string> documentation) where TEnum : System.Enum
		{
			if (subPath != null)
			{
				if (subPath != string.Empty && !subPath.EndsWith('\\') && !subPath.EndsWith('/'))
				{
					subPath += '\\';
				}
			}
			else
			{
				subPath = string.Empty;
			}

			WriteToFile(typePath + subPath, typeof(TEnum).Name, values, documentation);
		}

		public static void WriteToEnum<TEnum>(IEnumerable<string> values, IEnumerable<string> documentation) where TEnum : System.Enum
		{
			Type type = typeof(TEnum);

			string subPath = string.IsNullOrEmpty(type.Namespace) ? string.Empty : type.Namespace.Replace('.', '\\') + "\\";

			WriteToFile(typePath + subPath, type.Name, values, documentation);
		}

		public static void WriteToFile(string path, string typeName, IEnumerable<string> values, IEnumerable<string> documentation)
		{
			WriteEnumValues(path, typeName, values, documentation);

			CompilationPipeline.RequestScriptCompilation();
		}

		private static void WriteEnumValues(string path, string typeName, IEnumerable<string> values, IEnumerable<string> documentation)
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
				afterEnum = content[closeIndex..];
			}

			string beforeEnum = content[..startIndex];
			StringBuilder builder = new StringBuilder(beforeEnum);
			builder.AppendLine("{");

			string[] enumValues = values as string[] ?? values.ToArray();

			string[] enumDocumentation = null;

			if (documentation != null)
			{
				enumDocumentation = documentation as string[] ?? documentation.ToArray();
			}

			bool hasDocumentation = enumDocumentation != null;

			for (int i = 0; i < enumValues.Length; i++)
			{
				if (hasDocumentation)
				{
					builder.AppendLine("\t\t/// <summary>");
					builder.AppendLine($"\t\t/// {enumDocumentation[i].Replace("&", "&amp;").Replace("'", "&apos;")}"); // XML does not allow special characters directly
					builder.AppendLine("\t\t/// </summary>");
				}

				builder.AppendLine($"\t\t{enumValues[i]},");

				if (hasDocumentation && i != enumValues.Length - 1)
				{
					hasDocumentation = i < enumDocumentation.Length;

					if (hasDocumentation)
					{
						builder.AppendLine();
					}
				}
			}

			builder.Append("\t");

			builder.Append(afterEnum);

			File.WriteAllText(fullPath, builder.ToString());
		}
#endif
	}
}