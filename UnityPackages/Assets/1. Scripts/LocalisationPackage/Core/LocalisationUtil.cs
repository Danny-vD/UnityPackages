using LocalisationPackage.Core.IO.Parsers;

namespace LocalisationPackage.Core
{
	public static class LocalisationUtil
	{
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
	}
}