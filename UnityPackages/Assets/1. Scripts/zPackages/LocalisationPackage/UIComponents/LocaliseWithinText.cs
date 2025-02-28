using LocalisationPackage.Core;
using LocalisationPackage.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace LocalisationPackage.UIComponents
{
	/// <summary>
	/// Used to localise every EntryID found in a text (given by an entryOpen and entryClose pair)
	/// </summary>
	public class LocaliseWithinText : BetterMonoBehaviour
	{
		[SerializeField, Tooltip("Used to determine the start of an area in the text that need to be localised")]
		private string localisedEntryOpen = LocalisationUtil.ENTRY_OPENING_STRING;
		
		[SerializeField, Tooltip("Used to determine the end of an area in the text that need to be localised")]
		private string localisedEntryClose = LocalisationUtil.ENTRY_CLOSING_STRING;

		private Text labelLegacy;
		private TMP_Text labelTMP;

		private string originalText;

		private void Awake()
		{
			labelLegacy = GetComponent<Text>();
			labelTMP    = GetComponent<TMP_Text>();
		}

		private void Start()
		{
			StoreText();
			
			ReloadText();
			EventManager.AddListener<LanguageChangedEvent>(ReloadText);
		}

		private void ReloadText()
		{
			SetText(LocalisationUtil.LocaliseWithinString(originalText, localisedEntryOpen, localisedEntryClose));
		}

		private void StoreText()
		{
			if (labelTMP)
			{
				originalText = labelTMP.text;
			}
			else if (labelLegacy)
			{
				originalText = labelLegacy.text;
			}
		}

		private void SetText(string newText)
		{
			if (labelTMP)
			{
				labelTMP.text = newText;
			}
			else if (labelLegacy)
			{
				labelLegacy.text = newText;
			}
		}

		private void OnDestroy()
		{
			EventManager.RemoveListener<LanguageChangedEvent>(ReloadText);
		}
	}
}