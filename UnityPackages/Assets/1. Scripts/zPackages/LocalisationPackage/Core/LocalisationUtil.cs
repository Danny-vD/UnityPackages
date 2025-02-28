using LocalisationPackage.Core.IO.Parsers;
using VDFramework.Utility.DataTypes;

namespace LocalisationPackage.Core
{
	/// <summary>
	/// Provides helper functions to localise (parts of) a text using predefined EntryIDs
	/// </summary>
	public static class LocalisationUtil
	{
		public const char ENTRY_OPENING_CHAR = '[';
		public const char ENTRY_CLOSING_CHAR = ']';

		public const string ENTRY_OPENING_STRING = "[";
		public const string ENTRY_CLOSING_STRING = "]";

		/// <summary>
		/// Returns the string assigned to the given entryID for the current language
		/// </summary>
		/// <param name="entryID">The ID of the localisation as given in the Resources/Localisation folder</param>
		/// <returns>The string that is assigned to the given EntryID for the current language</returns>
		public static string GetLocalisedString(string entryID)
		{
			return JsonLocalisationParser.GetVariable(entryID, LanguageSettings.Language.ToString());
		}

		/// <summary>
		/// Returns the formatted string assigned to the given entryID for the current language
		/// </summary>
		/// <param name="entryID">The ID of the localisation as given in the Resources/Localisation folder</param>
		/// <param name="args">The arguments that should be placed in the string</param>
		/// <returns>The string that is assigned to the given EntryID for the current language, formatted to include the given arguments</returns>
		public static string GetLocalisedStringFormat(string entryID, params object[] args)
		{
			return string.Format(GetLocalisedString(entryID), args);
		}

		/// <summary>
		/// Returns the string for the given EntryID for the current language where the localised string can also contain entryIDs as determined by a pair of opening and closing strings
		/// </summary>
		/// <param name="entryID">The ID of the localisation as given in the Resources/Localisation folder</param>
		/// <param name="entryOpening">The string that represents the opening of a nested EntryID</param>
		/// <param name="entryClosing">The string that represents the closing of a nested EntryID</param>
		/// <returns>The string that is assigned to the given EntryID for the current language, with any text between a pair of strings also localised</returns>
		public static string GetLocalisedStringNested(string entryID, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING)
		{
			return LocaliseWithinString(GetLocalisedString(entryID), entryOpening, entryClosing);
		}
		
		/// <summary>
		/// Returns the formatted string for the given EntryID for the current language where the localised string is also formatted and can also contain entryIDs as determined by a pair of opening and closing strings
		/// </summary>
		/// <param name="entryID">The ID of the localisation as given in the Resources/Localisation folder</param>
		/// <param name="entryOpening">The string that represents the opening of a nested EntryID</param>
		/// <param name="entryClosing">The string that represents the closing of a nested EntryID</param>
		/// <param name="args">The arguments that should be placed in the string</param>
		/// <returns>The string that is assigned to the given EntryID for the current language, with any text between a pair of strings also localised, formatted at each step to include the given arguments</returns>
		public static string GetLocalisedStringNestedFormat(string entryID, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING, params object[] args)
		{
			return LocaliseWithinStringFormat(GetLocalisedStringFormat(entryID, args), entryOpening, entryClosing, args);
		}

		/// <summary>
		/// Returns the string for the given EntryID for the current language where the localised string can also contain entryIDs as determined by a pair of opening and closing characters
		/// </summary>
		/// <param name="entryID">The ID of the localisation as given in the Resources/Localisation folder</param>
		/// <param name="entryOpening">The character that represents the opening of a nested EntryID</param>
		/// <param name="entryClosing">The character that represents the closing of a nested EntryID</param>
		/// <param name="ignoreEscaped">Whether a found closing or opening match should be ignored if it is preceded by the escape character: '\'</param>
		/// <returns>The string that is assigned to the given EntryID for the current language, with any text between a pair of characters also localised</returns>
		public static string GetLocalisedStringNested(string entryID, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false)
		{
			return LocaliseWithinString(GetLocalisedString(entryID), entryOpening, entryClosing, ignoreEscaped);
		}

		/// <summary>
		/// Returns the formatted string for the given EntryID for the current language where the localised string is also formatted and can also contain entryIDs as determined by a pair of opening and closing characters
		/// </summary>
		/// <param name="entryID">The ID of the localisation as given in the Resources/Localisation folder</param>
		/// <param name="entryOpening">The character that represents the opening of a nested EntryID</param>
		/// <param name="entryClosing">The character that represents the closing of a nested EntryID</param>
		/// <param name="ignoreEscaped">Whether a found closing or opening match should be ignored if it is preceded by the escape character: '\'</param>
		/// <param name="args">The arguments that should be placed in the string</param>
		/// <returns>The string that is assigned to the given EntryID for the current language, with any text between a pair of characters also localised, formatted at each step to include the given arguments</returns>
		public static string GetLocalisedStringNestedFormat(string entryID, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false, params object[] args)
		{
			return LocaliseWithinStringFormat(GetLocalisedStringFormat(entryID, args), entryOpening, entryClosing, ignoreEscaped, args);
		}

		/// <summary>
		/// Returns the given string but with specific parts localised as determined by an opening and closing pair of strings
		/// </summary>
		/// <param name="input">The string to partially localise</param>
		/// <param name="entryOpening">The string that represents the opening of a EntryID</param>
		/// <param name="entryClosing">The string that represents the closing of a EntryID</param>
		/// <returns>The <paramref name="input"/> string with specific parts localised</returns>
		public static string LocaliseWithinString(string input, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING)
		{
			string newText = input;

			while (StringUtil.GetFirstMatchingPair(newText, entryOpening, entryClosing) is { } pairIndices)
			{
				int indexOfEntryID = pairIndices.Item1 + entryOpening.Length;             // The first character after the entryOpening
				int length = pairIndices.Item2 - pairIndices.Item1 + entryClosing.Length; //Make sure the length includes the entire entryClosing
				int entryIDLength = pairIndices.Item2 - indexOfEntryID;
				
				string toReplace = newText.Substring(pairIndices.Item1, length);
				string entryID = newText.Substring(indexOfEntryID, entryIDLength);
				
				string localisedString = GetLocalisedString(entryID);

				newText = newText.Replace(toReplace, localisedString);
			}

			return newText;
		}

		/// <summary>
		/// Returns the given string but with specific parts localised as determined by an opening and closing pair of strings
		/// </summary>
		/// <param name="input">The string to partially localise</param>
		/// <param name="entryOpening">The string that represents the opening of a EntryID</param>
		/// <param name="entryClosing">The string that represents the closing of a EntryID</param>
		/// <param name="args">The arguments that should be placed in the string</param>
		/// <returns>The <paramref name="input"/> string with specific parts localised, formatted to include the given arguments</returns>
		public static string LocaliseWithinStringFormat(string input, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING, params object[] args)
		{
			string newText = input;

			while (StringUtil.GetFirstMatchingPair(newText, entryOpening, entryClosing) is { } pairIndices)
			{
				int indexOfEntryID = pairIndices.Item1 + entryOpening.Length;             // The first character after the entryOpening
				int length = pairIndices.Item2 - pairIndices.Item1 + entryClosing.Length; //Make sure the length includes the entire entryClosing
				int entryIDLength = pairIndices.Item2 - indexOfEntryID;
				
				string toReplace = newText.Substring(pairIndices.Item1, length);
				string entryID = newText.Substring(indexOfEntryID, entryIDLength);
				
				string localisedString = GetLocalisedStringFormat(entryID, args);

				newText = newText.Replace(toReplace, localisedString);
			}

			return newText;
		}
		
		/// <summary>
		/// Returns the given string but with specific parts localised as determined by an opening and closing pair of characters
		/// </summary>
		/// <param name="input">The string to partially localise</param>
		/// <param name="entryOpening">The character that represents the opening of a EntryID</param>
		/// <param name="entryClosing">The character that represents the closing of a EntryID</param>
		/// <param name="ignoreEscaped">Whether a found closing or opening match should be ignored if it is preceded by the escape character: '\'</param>
		/// <returns>The <paramref name="input"/> string with specific parts localised</returns>
		public static string LocaliseWithinString(string input, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false)
		{
			string newText = input;

			while (StringUtil.GetFirstMatchingPair(newText, entryOpening, entryClosing, 0, ignoreEscaped) is { } pairIndices)
			{
				int indexOfEntryID = pairIndices.Item1 + 1;             // The first character after the entryOpening
				int length = pairIndices.Item2 - pairIndices.Item1 + 1; //Make sure the length includes the entire entryClosing
				int entryIDLength = pairIndices.Item2 - indexOfEntryID;
				
				string toReplace = newText.Substring(pairIndices.Item1, length);
				string entryID = newText.Substring(indexOfEntryID, entryIDLength);
				
				string localisedString = GetLocalisedString(entryID);

				newText = newText.Replace(toReplace, localisedString);
			}

			return newText;
		}

		/// <summary>
		/// Returns the given string but with specific parts localised as determined by an opening and closing pair of characters
		/// </summary>
		/// <param name="input">The string to partially localise</param>
		/// <param name="entryOpening">The character that represents the opening of a EntryID</param>
		/// <param name="entryClosing">The character that represents the closing of a EntryID</param>
		/// <param name="ignoreEscaped">Whether a found closing or opening match should be ignored if it is preceded by the escape character: '\'</param>
		/// <param name="args">The arguments that should be placed in the string</param>
		/// <returns>The <paramref name="input"/> string with specific parts localised, formatted to include the given arguments</returns>
		public static string LocaliseWithinStringFormat(string input, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false, params object[] args)
		{
			string newText = input;

			while (StringUtil.GetFirstMatchingPair(newText, entryOpening, entryClosing, 0, ignoreEscaped) is { } pairIndices)
			{
				int indexOfEntryID = pairIndices.Item1 + 1;             // The first character after the entryOpening
				int length = pairIndices.Item2 - pairIndices.Item1 + 1; //Make sure the length includes the entire entryClosing
				int entryIDLength = pairIndices.Item2 - indexOfEntryID;
				
				string toReplace = newText.Substring(pairIndices.Item1, length);
				string entryID = newText.Substring(indexOfEntryID, entryIDLength);
				
				string localisedString = GetLocalisedStringFormat(entryID, args);

				newText = newText.Replace(toReplace, localisedString);
			}

			return newText;
		}
	}
}