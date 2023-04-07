using System.Linq;
using Events.Localisation;
using TMPro;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace IO.Localisation
{
	public class LocalisedDropDown : BetterMonoBehaviour
	{
		private string[] entryIDs;

		private Dropdown dropdown;
		private TMP_Dropdown dropdownTMP;

		private void Awake()
		{
			dropdown = GetComponent<Dropdown>();

			dropdownTMP = GetComponent<TMP_Dropdown>();
		}

		private void Start()
		{
			if (dropdown)
			{
				entryIDs = dropdown.options.Select(option => option.text).ToArray();
			}
			else if (dropdownTMP)
			{
				entryIDs = dropdownTMP.options.Select(option => option.text).ToArray();
			}

			ReloadText();

			EventManager.AddListener<LanguageChangedEvent>(ReloadText);
		}

		private void ReloadText()
		{
			if (dropdown)
			{
				for (int i = 0; i < entryIDs.Length; i++)
				{
					dropdown.options[i].text = LanguageUtil.GetLocalisedString(entryIDs[i]);
				}
			}
			else if (dropdownTMP)
			{
				for (int i = 0; i < entryIDs.Length; i++)
				{
					dropdownTMP.options[i].text = LanguageUtil.GetLocalisedString(entryIDs[i]);
				}
			}
		}

		private void OnDestroy()
		{
			EventManager.RemoveListener<LanguageChangedEvent>(ReloadText);
		}
	}
}