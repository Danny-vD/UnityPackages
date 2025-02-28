using System.Linq;
using LocalisationPackage.Core;
using LocalisationPackage.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace LocalisationPackage.UIComponents
{
	public class LocalisedDropDown : BetterMonoBehaviour
	{
		[Header("Nested EntryIDs")]
		[SerializeField, Tooltip("Used to determine the start of an area in the text that need to be localised")]
		private string localisedEntryOpen = LocalisationUtil.ENTRY_OPENING_STRING;
		
		[SerializeField, Tooltip("Used to determine the end of an area in the text that need to be localised")]
		private string localisedEntryClose = LocalisationUtil.ENTRY_CLOSING_STRING;
		
		private string[] entryIDs;

		private Dropdown dropdown;
		private TMP_Dropdown dropdownTMP;

		private void Awake()
		{
			dropdownTMP = GetComponent<TMP_Dropdown>();

			// It is impossible for there to be both a dropdown and a TMP dropdown, so only test for the other if one is null
			if (ReferenceEquals(dropdownTMP, null))
			{
				dropdown = GetComponent<Dropdown>();

				if (ReferenceEquals(dropdown, null))
				{
					Debug.LogError("No dropdown found on this object, destroying this component...", gameObject);
					Destroy(this);
				}
			}
		}

		private void Start()
		{
			LoadEntryIDs();

			EventManager.AddListener<LanguageChangedEvent>(ReloadOptionText);
		}

		/// <summary>
		/// Force this component to read all options in the dropdown and store them as EntryIDs 
		/// </summary>
		public void LoadEntryIDs()
		{
			if (!ReferenceEquals(dropdownTMP, null))
			{
				entryIDs = dropdownTMP.options.Select(option => option.text).ToArray();
			}
			else // use legacy dropdown if no TMP dropdown
			{
				entryIDs = dropdown.options.Select(option => option.text).ToArray();
			}

			ReloadOptionText();
		}

		private void ReloadOptionText()
		{
			if (!ReferenceEquals(dropdownTMP, null))
			{
				for (int i = 0; i < entryIDs.Length; i++)
				{
					dropdownTMP.options[i].text = LocalisationUtil.GetLocalisedStringNested(entryIDs[i], localisedEntryOpen, localisedEntryClose);
				}
			}
			else // use legacy dropdown if no TMP dropdown
			{
				for (int i = 0; i < entryIDs.Length; i++)
				{
					dropdown.options[i].text = LocalisationUtil.GetLocalisedStringNested(entryIDs[i], localisedEntryOpen, localisedEntryClose);
				}
			}
		}

		private void OnDestroy()
		{
			EventManager.RemoveListener<LanguageChangedEvent>(ReloadOptionText);
		}
	}
}