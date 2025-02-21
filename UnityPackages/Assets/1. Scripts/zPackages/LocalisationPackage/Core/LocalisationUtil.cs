using System;
using System.Text;
using LocalisationPackage.Core.IO.Parsers;
using VDFramework.Utility.DataTypes;

namespace LocalisationPackage.Core
{
	public static class LocalisationUtil
	{
		public const char ENTRY_OPENING_CHAR = '[';
		public const char ENTRY_CLOSING_CHAR = ']';
		
		public const string ENTRY_OPENING_STRING = "[";
		public const string ENTRY_CLOSING_STRING = "]";
		
		/// <summary>
		/// Returns the string for a specific key for the current language
		/// </summary>
		public static string GetLocalisedString(string entryID)
		{
			return JsonLocalisationParser.GetVariable(entryID, LanguageSettings.Language.ToString());
		}

		/// <summary>
		/// Returns the formatted string for a specific key for the current language  
		/// </summary>
		public static string GetLocalisedString(string entryID, params object[] args)
		{
			return string.Format(GetLocalisedString(entryID), args);
		}
		
		/// <summary>
		/// Returns the string for a specific key for the current language where the localised string can also contain a entryID as determined by a opening and closing character
		/// </summary>
		public static string GetNestedLocalisedString(string entryID, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false)
		{
			string localisedString = JsonLocalisationParser.GetVariable(entryID, LanguageSettings.Language.ToString());
			
			while (localisedString.IndexOf(entryOpening) != -1 && localisedString.IndexOf(entryOpening) < localisedString.IndexOf(entryClosing))
			{
				localisedString = LocaliseWithinString(localisedString, entryOpening, entryClosing, ignoreEscaped);
			}
			
			return localisedString;
		}

		/// <summary>
		/// Returns the formatted string for a specific key for the current language where the localised string can also contain a entryID as determined by a opening and closing character
		/// </summary>
		public static string GetNestedLocalisedString(string entryID, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false, params object[] args)
		{
			return string.Format(GetNestedLocalisedString(entryID, entryOpening, entryClosing, ignoreEscaped), args);
		}
		
		/// <summary>
		/// Returns the string for a specific key for the current language where the localised string can also contain a entryID as determined by a opening and closing string
		/// </summary>
		public static string GetNestedLocalisedString(string entryID, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING)
		{
			string localisedString = JsonLocalisationParser.GetVariable(entryID, LanguageSettings.Language.ToString());
			
			while (localisedString.IndexOf(entryOpening, StringComparison.InvariantCulture) != -1 && localisedString.IndexOf(entryOpening, StringComparison.InvariantCulture) < localisedString.IndexOf(entryClosing, StringComparison.InvariantCulture))
			{
				localisedString = LocaliseWithinString(localisedString, entryOpening, entryClosing);
			}
			
			return localisedString;
		}

		/// <summary>
		/// Returns the formatted string for a specific key for the current language where the localised string can also contain a entryID as determined by a opening and closing string
		/// </summary>
		public static string GetNestedLocalisedString(string entryID, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING, params object[] args)
		{
			return string.Format(GetNestedLocalisedString(entryID, entryOpening, entryClosing), args);
		}

		/// <summary>
		/// Returns given the string but with specific parts localised as determined by a opening and closing character for the localised parts
		/// </summary>
		public static string LocaliseWithinString(string text, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, bool ignoreEscaped = false)
		{
			string newText = text;
			
			foreach (string entryID in StringUtil.GetStringsBetweenAandB(text, entryOpening, entryClosing, false, ignoreEscaped))
			{
				if (entryID.Equals(string.Empty))
				{
					continue;
				}
				
				StringBuilder builder = new StringBuilder();
				builder.Append(entryOpening).Append(entryID).Append(entryClosing);
				string substring = builder.ToString();

				string localisedString = GetNestedLocalisedString(entryID, entryOpening, entryClosing);
				
				newText = newText.Replace(substring, localisedString);
			}

			return newText;
		}

		/// <summary>
		/// Returns the given string formatted but with specific parts localised as determined by a opening and closing character for the localised parts
		/// </summary>
		public static string LocaliseWithinString(string text, char entryOpening = ENTRY_OPENING_CHAR, char entryClosing = ENTRY_CLOSING_CHAR, params object[] args)
		{
			return string.Format(LocaliseWithinString(text, entryOpening, entryClosing), args);
		}
		
		/// <summary>
		/// Returns the given string but with specific parts localised as determined by a opening and closing string for the localised parts
		/// </summary>
		public static string LocaliseWithinString(string text, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING)
		{
			string newText = text;
			
			foreach (string entryID in StringUtil.GetStringsBetweenAandB(text, entryOpening, entryClosing))
			{
				if (entryID.Equals(string.Empty))
				{
					continue;
				}
				
				StringBuilder builder = new StringBuilder();
				builder.Append(entryOpening).Append(entryID).Append(entryClosing);
				string substring = builder.ToString();

				string localisedString = GetNestedLocalisedString(entryID, entryOpening, entryClosing);
				
				newText = newText.Replace(substring, localisedString);
			}

			return newText;
		}

		/// <summary>
		/// Returns the given string formatted but with specific parts localised as determined by a opening and closing string for the localised parts
		/// </summary>
		public static string LocaliseWithinString(string text, string entryOpening = ENTRY_OPENING_STRING, string entryClosing = ENTRY_CLOSING_STRING, params object[] args)
		{
			return string.Format(LocaliseWithinString(text, entryOpening, entryClosing), args);
		}
	}
}