using System;
using System.Collections.Generic;
using LocalisationPackage.Core.Enums;
using LocalisationPackage.Core.IO.Parsers.Interfaces;
using UnityEngine;
using VDFramework.Logger;

namespace LocalisationPackage.Core.IO.Parsers
{
	public class JsonLocalisationParser : ILocalisationParser
	{
		private static readonly Dictionary<string, Dictionary<Language, string>> localisationEntries = new Dictionary<string, Dictionary<Language, string>>();
		
		public bool CanPreReadAllEntries => true;

		static JsonLocalisationParser()
		{
			ReadData();
		}

		private static void ReadData()
		{
			foreach (TextAsset file in Resources.LoadAll<TextAsset>("Localisation"))
			{
				JsonLocalisationEntries jsonLocalisationEntries = JsonUtility.FromJson<JsonLocalisationEntries>(file.ToString());

				foreach (LocalisationEntry entry in jsonLocalisationEntries.Entries)
				{
					// Check if this entry ID was already defined elsewhere (if it is, we can simply add any new languages to it)
					if (!localisationEntries.TryGetValue(entry.EntryID, out Dictionary<Language, string> stringsPerLanguageDictionary))
					{
						stringsPerLanguageDictionary = new Dictionary<Language, string>();
						localisationEntries.Add(entry.EntryID, stringsPerLanguageDictionary);
					}

					foreach (LanguageKeyValuePair languageKeyValuePair in entry.LanguagePairs)
					{
						Language language = Enum.Parse<Language>(languageKeyValuePair.LanguageID);
						
						// Only add the new Language-Value pair if that language was not already defined for this EntryID
						if (!stringsPerLanguageDictionary.TryAdd(language, languageKeyValuePair.Value))
						{
							LogManager.LogWarning($"Language {language} already defined for {entry.EntryID}!\n{file.name}\nIgnoring value \"{languageKeyValuePair.Value}\"");
						}
					}
				}
			}
		}

		public string GetLocalisedEntry(string entryID, Language languageID)
		{
			if (localisationEntries.TryGetValue(entryID, out Dictionary<Language, string> stringsPerLanguageDictionary))
			{
				if (stringsPerLanguageDictionary.TryGetValue(languageID, out string localisedString))
				{
					return localisedString;
				}
				
				if (stringsPerLanguageDictionary.TryGetValue(LanguageSettings.DEFAULT_LANGUAGE, out localisedString))
				{
					return localisedString;
				}

				LogManager.LogError($"Entry '{entryID}' has no localisation for language {LanguageSettings.Language} or {LanguageSettings.DEFAULT_LANGUAGE}!");
				return LocalisationDataManager.NO_LOCALISATION_STRING;
			}
			
			LogManager.LogError($"Entry '{entryID}' was not found!");
			return entryID.ToUpper();
		}

		public Dictionary<string, Dictionary<Language, string>> GetAllEntries()
		{
			return localisationEntries;
		}
	}

	[Serializable]
	public class JsonLocalisationEntries
	{
		public List<LocalisationEntry> Entries = new List<LocalisationEntry>();
	}

	[Serializable]
	public class LocalisationEntry
	{
		public string EntryID;
		public LanguageKeyValuePair[] LanguagePairs;
	}
	
	[Serializable]
	public class LanguageKeyValuePair
	{
		public string LanguageID;
		public string Value;
	}
}