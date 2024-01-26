using LocalisationPackage.Core;
using LocalisationPackage.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace LocalisationPackage.UIComponents
{
	public class LocalisedText : BetterMonoBehaviour
	{
		[SerializeField, Tooltip("Will use the current text as the textType variable")]
		private bool UseTextAsEntryID = false;

		[SerializeField]
		private string textType = "PLACEHOLDER";

		private Text text;
		private TMP_Text textTMP;

		private void Awake()
		{
			text    = GetComponent<Text>();
			textTMP = GetComponent<TMP_Text>();
		}

		private void Start()
		{
			if (UseTextAsEntryID)
			{
				textType = text.text;
			}

			ReloadText();
			EventManager.AddListener<LanguageChangedEvent>(ReloadText);
		}

		private void ReloadText()
		{
			SetText(LocalisationUtil.GetLocalisedString(textType));
		}

		private void SetText(string newText)
		{
			if (text)
			{
				text.text = newText;
			}

			if (textTMP)
			{
				textTMP.text = newText;
			}
		}

		private void OnDestroy()
		{
			EventManager.RemoveListener<LanguageChangedEvent>(ReloadText);
		}
	}
}