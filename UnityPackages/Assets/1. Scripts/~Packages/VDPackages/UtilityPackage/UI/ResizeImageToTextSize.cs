using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Logger;
using VDFramework.ObserverPattern.Constants;
using VDPackages.LocalisationPackage.Events;

namespace VDPackages.UtilityPackage.UI
{
	[DefaultExecutionOrder(50)] // After default to ensure that any other scripts can set the text first
	public class ResizeImageToTextSize : BetterMonoBehaviour
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
		[SerializeField, Tooltip("Should resize: Horizontally | Vertically")]
		private bool2 resizeHorizontalVertical = new bool2(true, true);

		[SerializeField, Tooltip("Additional padding: Horizontally | Vertically")]
		private float2 paddingHorizontalVertical = new float2(35, 5);

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
				LogManager.LogError("No image assigned in the inspector!", this);
				return;
			}
			
			imageRectTransform = image.GetComponent<RectTransform>();
		}

		private void Start()
		{
			UpdateImageSize();
		}

		private void OnEnable()
		{
			UpdateImageSize();

			if (reactToLanguageChangedEvent)
			{
				LanguageChangedEvent.AddListener(UpdateImageSize, Priority.UI);
			}
		}

		private void OnDisable()
		{
			if (reactToLanguageChangedEvent)
			{
				LanguageChangedEvent.RemoveListener(UpdateImageSize);
			}
		}

		public void UpdateImageSize()
		{
			// The preferred width and height of the text
			float textWidth;
			float textHeight;
			
			if (labelTMP)
			{
				textWidth  = labelTMP.preferredWidth;
				textHeight = labelTMP.preferredHeight;
			}
			else if (labelLegacy)
			{
				textWidth  = labelLegacy.preferredWidth;
				textHeight = labelLegacy.preferredHeight;
			}
			else
			{
				LogManager.LogError("No text assigned in the inspector!", this);
				return;
			}

			if (!image)
			{
				LogManager.LogError("No image assigned in the inspector!", this);
				return;
			}

			Vector2 newSize = new Vector2(imageRectTransform.sizeDelta.x, imageRectTransform.sizeDelta.y);

			if (resizeHorizontalVertical.x)
			{
				newSize.x = textWidth + paddingHorizontalVertical.x;
			}

			if (resizeHorizontalVertical.y)
			{
				newSize.y = textHeight + paddingHorizontalVertical.y;
			}
			
			imageRectTransform.sizeDelta = newSize;
		}
	}
}