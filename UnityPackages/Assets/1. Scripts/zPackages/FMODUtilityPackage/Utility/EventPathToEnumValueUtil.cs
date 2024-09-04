using System;
using System.Collections.Generic;
using System.Linq;
using FMODUtilityPackage.Enums;

namespace FMODUtilityPackage.Utility
{
	public static class EventPathToEnumValueUtil
	{
		public static AudioEventType[] ConvertEventPathToEnumValues(string[] eventPaths)
		{
			return ConvertEventPathToEnumValuesString(eventPaths).Select(Enum.Parse<AudioEventType>).ToArray();
		}
		
		public static string[] ConvertEventPathToEnumValuesString(string[] eventPaths)
		{
			return ValidateArray(eventPaths, '/'); // everything starts with 'event:/'
		}

		public static string GetValidEnumName(string stringValue)
		{
			string enumValue = stringValue;

			for (int j = stringValue.Length - 1; j >= 0; j--)
			{
				char character = stringValue[j];

				// Replace any special characters with an underscore (specials characters are not allowed in an enum name, but an underscore is)
				if (!char.IsLetterOrDigit(character) && !character.Equals('_'))
				{
					enumValue = enumValue.Replace(character, '_');
				}
			}

			// Starting an enum value with a digit (or underscore) is not allowed, prefix it with 'E' if that is the case
			if (stringValue[0].Equals('_'))
			{
				enumValue = "E" + stringValue;
			}

			return enumValue;
		}
		
		private static string[] ValidateArray(string[] array, char startCharacter)
		{
			List<string> list = array.Distinct().ToList(); // Prevent duplicates

			for (int i = list.Count - 1; i >= 0; i--)
			{
				string value = list[i];

				// Trim everything after our starting character
				value = value.Substring(value.IndexOf(startCharacter) + 1);

				if (value == string.Empty) // If that removed the entire string, skip it
				{
					list.RemoveAt(i);
					continue;
				}

				// Convert the string into a valid enum name and store it in the list
				list[i] = GetValidEnumName(value);
			}

			return list.ToArray();
		}
	}
}