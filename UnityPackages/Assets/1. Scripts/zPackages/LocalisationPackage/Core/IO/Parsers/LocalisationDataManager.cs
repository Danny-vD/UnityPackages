using System.Collections.Generic;
using LocalisationPackage.Core.Enums;
using LocalisationPackage.Core.IO.Parsers.Interfaces;
using VDFramework.Logger;

namespace LocalisationPackage.Core.IO.Parsers
{
	/// <summary>
	/// Provides a wrapper from getting data from a <see cref="ILocalisationParser"/> and caching that data if possible
	/// </summary>
	public static class LocalisationDataManager
	{
		public const string NO_LOCALISATION_STRING = "NO-LOCALISATION";

		public static ILocalisationParser ParserImplementation = new JsonLocalisationParser();

		private static Dictionary<string, Dictionary<Language, string>> localisedEntries;

		private static bool IsInitialized => ParserImplementation.CanPreReadAllEntries && localisedEntries != null;

		public static void Initialize()
		{
			if (ParserImplementation.CanPreReadAllEntries)
			{
				localisedEntries = ParserImplementation.GetAllEntries();
			}
		}

		/// <summary>
		/// Returns the localised string for the given entry in the current <see cref="LanguageSettings.Language"/> or <see cref="LanguageSettings.DEFAULT_LANGUAGE"/> as a fallback option
		/// </summary>
		/// <param name="entryID">The ID of the entry</param>
		/// <returns>The localised <see langword="string"/> for the given entry</returns>
		/// <remarks>
		/// If an entry can not be localised for the current <see cref="LanguageSettings.Language"/> then the <see cref="LanguageSettings.DEFAULT_LANGUAGE"/> will be used instead<br/>
		/// If this also fails then the <see cref="NO_LOCALISATION_STRING"/> will be returned<br/>
		/// If the entry cannot be found, then the <paramref name="entryID"/> will be returned in uppercase
		/// </remarks>
		public static string GetLocalisedEntry(string entryID)
		{
			if (!IsInitialized)
			{
				Initialize();
			}
			
			if (ParserImplementation.CanPreReadAllEntries) // Because the parser can pre-read the entries, we can use the cached values
			{
				if (localisedEntries.TryGetValue(entryID, out Dictionary<Language, string> localisedStringPerLanguage))
				{
					if (localisedStringPerLanguage.TryGetValue(LanguageSettings.Language, out string localisedString))
					{
						return localisedString;
					}
					
					if (localisedStringPerLanguage.TryGetValue(LanguageSettings.DEFAULT_LANGUAGE, out localisedString))
					{
						return localisedString;
					}
					
					LogManager.LogError($"Entry '{entryID}' has no localisation for language {LanguageSettings.Language} or {LanguageSettings.DEFAULT_LANGUAGE}!");
					return NO_LOCALISATION_STRING;
				}
				
				LogManager.LogError($"Entry '{entryID}' was not found!");
				return entryID.ToUpper();
			}
			
			return ParserImplementation.GetLocalisedEntry(entryID, LanguageSettings.Language);
		}
	}
}