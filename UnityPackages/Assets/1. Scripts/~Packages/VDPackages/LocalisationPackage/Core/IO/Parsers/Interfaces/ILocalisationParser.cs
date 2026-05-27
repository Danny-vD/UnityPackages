using System.Collections.Generic;
using VDPackages.LocalisationPackage.Core.Enums;

namespace VDPackages.LocalisationPackage.Core.IO.Parsers.Interfaces
{
	/// <summary>
	/// Provides an API for reading individual entries for localisation or reading all available entries at once
	/// </summary>
	public interface ILocalisationParser
	{
		/// <summary>
		/// Used to determine if <see cref="GetLocalisedEntry"/> will be called for every entry when it is accessed or <see cref="GetAllEntries"/> once during initialisation (after which it is cached)
		/// </summary>
		public bool CanPreReadAllEntries { get; }
		
		/// <summary>
		/// Get a single localised string for the given entry and language
		/// </summary>
		/// <param name="entryID">The ID of the localisation</param>
		/// <param name="languageID">the ID of the language</param>
		/// <returns>A localised <see langword="string"/> for the given language</returns>
		/// <remarks>
		/// It is up to the implementation on how to handle the case where the given entry or language is not valid<br/>
		///	Ideally the implementation also checks if <see cref="LanguageSettings.DEFAULT_LANGUAGE"/> *is* valid so it can be used as a fallback option
		/// </remarks>
		public string GetLocalisedEntry(string entryID, Language languageID);

		/// <summary>
		/// <para>Reads all entries and return them in a dictionary[EntryID, [LanguageID, LocalisedString]]</para>
		/// <para>Calling this function when <see cref="CanPreReadAllEntries"/> is false means the function might not necessarily return all available entries</para>
		/// </summary>
		/// <returns>A dictionary that contains all (available) localisation entries grouped per language and then per entry ID</returns>
		public Dictionary<string, Dictionary<Language, string>> GetAllEntries();
	}
}