using LocalisationPackage.Core;
using LocalisationPackage.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;
using VDFramework.Extensions;

namespace LocalisationPackage.UIComponents
{
	public class LocalisedText : BetterMonoBehaviour
	{
		[SerializeField, Tooltip("Will use the current text as the textType variable")]
		private bool UseTextAsEntryID = false;

		[SerializeField]
		private string entryID = "PLACEHOLDER";
		
		[Header("Nested EntryIDs")]
		[SerializeField, Tooltip("Used to determine the start of an area in the text that need to be localised")]
		private string localisedEntryOpen = LocalisationUtil.ENTRY_OPENING_STRING;
		
		[SerializeField, Tooltip("Used to determine the end of an area in the text that need to be localised")]
		private string localisedEntryClose = LocalisationUtil.ENTRY_CLOSING_STRING;

		[Header("Settings")]
		[SerializeField]
		private bool capitaliseFirstLetter;

		private Text labelLegacy;
		private TMP_Text labelTMP;

		private void Awake()
		{
			labelLegacy = GetComponent<Text>();
			labelTMP    = GetComponent<TMP_Text>();
		}

		private void Start()
		{
			if (UseTextAsEntryID)
			{
				entryID = labelLegacy.text;
			}

			ReloadText();
			EventManager.AddListener<LanguageChangedEvent>(ReloadText);
		}

		private void ReloadText()
		{
			SetText(LocalisationUtil.GetLocalisedStringNested(entryID, localisedEntryOpen, localisedEntryClose));
		}

		private void SetText(string newText)
		{
			if (capitaliseFirstLetter)
			{
				newText = newText.CapitaliseFirstLetter();
			}
			
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