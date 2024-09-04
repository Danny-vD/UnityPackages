using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LocalisationPackage.Core.IO.Parsers
{
	public static class JsonLocalisationParser
	{
		private static readonly JsonLanguageVariables variables;

		static JsonLocalisationParser()
		{
			variables = new JsonLanguageVariables();

			foreach (TextAsset file in Resources.LoadAll<TextAsset>("Localisation"))
			{
				variables.AddVariables(JsonUtility.FromJson<JsonLanguageVariables>(file.ToString()));
			}
		}

		public static string GetVariable(string entryID, string languageID)
		{
			return variables.GetVariable(entryID, languageID);
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

		public string GetVariable(string entryID, string languageID)
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

		private Dictionary<string, Dictionary<string, string>> entryPerVariable = null;

		private Dictionary<string, Dictionary<string, string>> GetEntry()
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

		private Dictionary<string, string> dictionary = null;

		public Dictionary<string, string> GetDictionary
		{
			get { return dictionary ??= CalculateLanguageDictionary.GetDictionary(Languages); }
		}
	}

	public static class CalculateLanguageDictionary
	{
		public static Dictionary<string, string> GetDictionary(IEnumerable<LanguageKeyValuePair> pArray)
		{
			return pArray.ToDictionary(entry => entry.LanguageID, entry => entry.Value);
		}

		public static Dictionary<string, Dictionary<string, string>> GetNestedDictionary(IEnumerable<LanguageVariable> pArray)
		{
			return pArray.ToDictionary(entry => entry.EntryID, entry => entry.GetDictionary);
		}
	}
}