using System.Linq;
using Events.Localisation;
using UnityEngine;
using VDFramework.EventSystem;
using VDFramework.Extensions;

namespace Enums.Localisation
{
	// ReSharper disable InconsistentNaming
	public enum Language
	{
		NL = SystemLanguage.Dutch,
		EN = SystemLanguage.English,
		DE = SystemLanguage.German,
	}

	public static class LanguageSettings
	{
		private const bool UseSystemLanguageAsDefault = true;

		private const Language DefaultLanguage = Language.EN;

		static LanguageSettings()
		{
#pragma warning disable CS0162 // Heuristically unreachable code
			if (UseSystemLanguageAsDefault)
			{
				SystemLanguage = Application.systemLanguage;
			}
			else
			{
				Language = DefaultLanguage;
			}
#pragma warning restore CS0162
		}

		private static Language language;

		public static Language Language
		{
			get => language;
			set
			{
				language = IsValidLanguage(value) ? value : DefaultLanguage;

				EventManager.RaiseEvent(new LanguageChangedEvent());
			}
		}

		/// <summary>
		/// The current language as a systemlanguage
		/// </summary>
		public static SystemLanguage SystemLanguage
		{
			get => (SystemLanguage)Language;
			set => Language = (Language)value;
		}

		private static bool IsValidLanguage(Language checkLanguage)
		{
			return default(Language).GetNames().Contains(checkLanguage.ToString());
		}
	}
}