using Enums.Localisation;

namespace IO.Localisation
{
	public static class LanguageUtil
	{
		/// <summary>
		/// Returns the string for a specific key for the current language
		/// </summary>
		public static string GetLocalisedString(string entryID)
		{
			return JsonLanguageParser.GetVariable(entryID, LanguageSettings.Language.ToString());
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