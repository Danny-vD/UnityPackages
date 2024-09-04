using APIs.DiscordIntegrationPackage.RichPresence.Enums;
using APIs.DiscordIntegrationPackage.Structs;
using SerializableDictionaryPackage.SerializableDictionary;
using UnityEngine;
using VDFramework.Singleton;

namespace APIs.DiscordIntegrationPackage.RichPresence
{
	public class DiscordImageManager : Singleton<DiscordImageManager>
	{
		[SerializeField]
		private SerializableEnumDictionary<DiscordImage, ImageData> images;

		public ImageData GetImageID(DiscordImage discordImage)
		{
			return images[discordImage];
		}
	}
}