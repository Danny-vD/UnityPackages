using System.Linq;
using LocalisationPackage.Core.Enums;
using LocalisationPackage.Events;
using UnityEngine;
using VDFramework.EventSystem;
using VDFramework.Extensions;

namespace LocalisationPackage.Core
{
	public static class LanguageSettings
	{
		private const bool useSystemLanguageAsDefault = true;

		private const Language defaultLanguage = Language.EN;

		static LanguageSettings()
		{
#pragma warning disable CS0162 // Heuristically unreachable code
			if (useSystemLanguageAsDefault)
			{
				SystemLanguage = Application.systemLanguage;
			}
			else
			{
				Language = defaultLanguage;
			}
#pragma warning restore CS0162
		}

		private static Language language;

		public static Language Language
		{
			get => language;
			set
			{
				language = IsValidLanguage(value) ? value : defaultLanguage;

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