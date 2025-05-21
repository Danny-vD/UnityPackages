using LocalisationPackage.Core.Enums;
using LocalisationPackage.Events;
using UnityEngine;
using VDFramework.EventSystem;
using VDFramework.Utility;

namespace LocalisationPackage.Core
{
	public static class LanguageSettings
	{
		/// <summary>
		/// <para>The default language of the application if no other language was set</para>
		/// <para>The default language will be the same as the <see cref="SystemLanguage"/> if <see cref="useSystemLanguageAsDefault"/> is set</para>
		/// <para>This constant is also used as a 'fallback' option if a given entry is not localised in the desired language</para>
		/// </summary>
		public const Language DEFAULT_LANGUAGE = Language.EN;
		
		private const bool useSystemLanguageAsDefault = true; // If false, will use the 'defaultLanguage' as default

		static LanguageSettings()
		{
#pragma warning disable CS0162 // Heuristically unreachable code
			if (useSystemLanguageAsDefault)
			{
				SystemLanguage = Application.systemLanguage;
			}
			else
			{
				// ReSharper disable once HeuristicUnreachableCode
				Language = DEFAULT_LANGUAGE;
			}
#pragma warning restore CS0162
		}

		private static Language language;

		public static Language Language
		{
			get => language;
			set
			{
				language = EnumUtil.IsValidEnumValue(value) ? value : DEFAULT_LANGUAGE;

				EventManager.RaiseEvent(new LanguageChangedEvent());
			}
		}

		/// <summary>
		/// The current language as a systemlanguage
		/// </summary>
		public static SystemLanguage SystemLanguage
		{
			get
			{
				SystemLanguage systemLanguage = (SystemLanguage)Language;
				
				return EnumUtil.IsValidEnumValue(systemLanguage) ? systemLanguage : SystemLanguage.Unknown; // Return Unknown if the current language does not translate to a system language
			}
			set => Language = (Language)value;
		}
	}
}