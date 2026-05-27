using UnityEngine;
using VDFramework.Singleton;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Enums;
using VDPackages.APIs.DiscordIntegrationPackage.Structs;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence
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