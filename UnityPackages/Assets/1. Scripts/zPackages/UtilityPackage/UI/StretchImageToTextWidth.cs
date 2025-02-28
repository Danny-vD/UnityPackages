using LocalisationPackage.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.ObserverPattern.Constants;

namespace UtilityPackage.UI
{
	[DefaultExecutionOrder(50)] // After default to ensure that any other scripts can set the text first
	public class StretchImageToTextWidth : BetterMonoBehaviour
	{
		[Header("Text")]
		[SerializeField]
		private TMP_Text labelTMP;
		
		[SerializeField]
		private Text labelLegacy;

		[Header("Image")]
		[SerializeField]
		private Image image;

		[Header("Settings")]
		[SerializeField]
		private float padding = 35;

		[Header("Localisation")]
		[SerializeField]
		private bool reactToLanguageChangedEvent = true;
		
		// Set the width of the image to match the text width (plus optional padding)
		private RectTransform imageRectTransform;

		private void Reset()
		{
			image = GetComponent<Image>();

			labelTMP = GetComponentInChildren<TMP_Text>();

			if (!labelTMP)
			{
				labelLegacy = GetComponentInChildren<Text>();
			}
		}

		private void Awake()
		{
			if (!image)
			{
				Debug.LogError("No image assigned in the inspector!", this);
				return;
			}
			
			imageRectTransform = image.GetComponent<RectTransform>();
		}

		private void Start()
		{
			UpdateImageWidth();
		}

		private void OnEnable()
		{
			UpdateImageWidth();

			if (reactToLanguageChangedEvent)
			{
				LanguageChangedEvent.AddListener(UpdateImageWidth, Priority.UI);
			}
		}

		private void OnDisable()
		{
			if (reactToLanguageChangedEvent)
			{
				LanguageChangedEvent.RemoveListener(UpdateImageWidth);
			}
		}

		public void UpdateImageWidth()
		{
			// Get the preferred width of the text
			float textWidth;
			
			if (labelTMP)
			{
				textWidth = labelTMP.preferredWidth;
			}
			else if (labelLegacy)
			{
				textWidth = labelLegacy.preferredWidth;
			}
			else
			{
				Debug.LogError("No text assigned in the inspector!", this);
				return;
			}

			if (!image)
			{
				Debug.LogError("No image assigned in the inspector!", this);
				return;
			}

			
			imageRectTransform.sizeDelta = new Vector2(textWidth + padding, imageRectTransform.sizeDelta.y);
		}
	}
}