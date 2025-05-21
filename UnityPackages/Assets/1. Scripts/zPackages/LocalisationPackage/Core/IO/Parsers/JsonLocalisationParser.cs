using System;
using System.Collections.Generic;
using System.Linq;
using LocalisationPackage.Core.Enums;
using LocalisationPackage.Core.IO.Parsers.Interfaces;
using UnityEngine;

namespace LocalisationPackage.Core.IO.Parsers
{
	public class JsonLocalisationParser : ILocalisationParser
	{
		private static readonly JsonLanguageVariables variables;
		
		public bool CanPreReadAllEntries => true;

		static JsonLocalisationParser()
		{
			variables = new JsonLanguageVariables();

			foreach (TextAsset file in Resources.LoadAll<TextAsset>("Localisation"))
			{
				variables.AddVariables(JsonUtility.FromJson<JsonLanguageVariables>(file.ToString()));
			}
		}

		public string GetLocalisedEntry(string entryID, Language languageID)
		{
			return variables.GetVariable(entryID, languageID);
		}

		public Dictionary<string, Dictionary<Language, string>> GetAllEntries()
		{
			return variables.GetEntry();
		}
	}

	[Serializable]
	public class JsonLanguageVariables
	{
		private const string defaultString = "UNDEFINED";

		public List<LanguageVariable> Variables = new List<LanguageVariable>();

		public void AddVariables(JsonLanguageVariables jsonVariables)
		{
			Variables.AddRange(jsonVariables.Variables);
		}

		public string GetVariable(string entryID, Language languageID)
		{
			try
			{
				return GetEntry()[entryID][languageID];
			}
			catch (KeyNotFoundException)
			{
				return defaultString;
			}
		}

		private Dictionary<string, Dictionary<Language, string>> entryPerVariable = null;

		public Dictionary<string, Dictionary<Language, string>> GetEntry()
		{
			return entryPerVariable ??= CalculateLanguageDictionary.GetNestedDictionary(Variables);
		}
	}

	[Serializable]
	public class LanguageKeyValuePair
	{
		public string LanguageID;
		public string Value;
	}

	[Serializable]
	public class LanguageVariable
	{
		public string EntryID;
		public LanguageKeyValuePair[] Languages;

		private Dictionary<Language, string> dictionary = null;

		public Dictionary<Language, string> GetDictionary
		{
			get { return dictionary ??= CalculateLanguageDictionary.GetDictionary(Languages); }
		}
	}

	public static class CalculateLanguageDictionary
	{
		public static Dictionary<Language, string> GetDictionary(IEnumerable<LanguageKeyValuePair> pArray)
		{
			return pArray.ToDictionary(entry => Enum.Parse<Language>(entry.LanguageID), entry => entry.Value);
		}

		public static Dictionary<string, Dictionary<Language, string>> GetNestedDictionary(IEnumerable<LanguageVariable> pArray)
		{
			return pArray.ToDictionary(entry => entry.EntryID, entry => entry.GetDictionary);
		}
	}
}