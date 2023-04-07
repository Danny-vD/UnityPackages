using Enums.Localisation;
using Events.Localisation;
using UnityEngine;
using UnityEngine.UI;
using Utility.SerializableDictionary;
using VDFramework;
using VDFramework.EventSystem;

namespace Localisation.UI
{
	public class LocalisedSprite : BetterMonoBehaviour
	{
		[SerializeField]
		private SerializableEnumDictionary<Language, Sprite> localisedSprites;

		private Image image;
		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			image          = GetComponent<Image>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			ReloadSprite();
			EventManager.AddListener<LanguageChangedEvent>(ReloadSprite);
		}

		private void ReloadSprite()
		{
			SetSprite(localisedSprites[LanguageSettings.Language]);
		}

		private void SetSprite(Sprite newSprite)
		{
			if (image != null)
			{
				image.sprite = newSprite;
			}

			if (spriteRenderer != null)
			{
				spriteRenderer.sprite = newSprite;
			}
		}
		
		private void OnDestroy()
		{
			EventManager.RemoveListener<LanguageChangedEvent>(ReloadSprite);
		}
	}
}