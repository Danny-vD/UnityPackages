using System;
using Discord.Sdk;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Enums;
using VDPackages.APIs.DiscordIntegrationPackage.Structs;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Utility
{
	public static class ActivityUtility
	{
		public static Activity AddPartyInformation(ref Activity activity, string partyID, int currentPartySize, int maxPartySize, ActivityPartyPrivacy partyPrivacy)
		{
			ActivityParty activityParty = new ActivityParty();
			activityParty.SetId(partyID);
			
			activityParty.SetCurrentSize(currentPartySize);
			activityParty.SetMaxSize(maxPartySize);
			
			activityParty.SetPrivacy(partyPrivacy);
			
			activity.SetParty(activityParty);
			return activity;
		}

		public static Activity SetMultiplayerSecrets(ref Activity activity, string joinSecret)
		{
			ActivitySecrets activitySecrets = new ActivitySecrets();
			activitySecrets.SetJoin(joinSecret);
			
			activity.SetSecrets(activitySecrets);
			return activity;
		}
		
		public static Activity AddButton(ref Activity activity, string label, string url)
		{
			ActivityButton activityButton = new ActivityButton();

			activityButton.SetLabel(label);
			activityButton.SetUrl(url);

			activity.AddButton(activityButton);
			return activity;
		}

		public static Activity AddTimeStampsStart(ref Activity activity, ulong startTime)
		{
			ActivityTimestamps timestamps = new ActivityTimestamps();

			startTime = startTime != 0 ? startTime : (ulong)DateTimeOffset.Now.ToUnixTimeSeconds();
			timestamps.SetStart(startTime);

			activity.SetTimestamps(timestamps);
			return activity;
		}

		public static Activity AddTimeStampsEnd(ref Activity activity, ulong timeRemaining)
		{
			ActivityTimestamps timestamps = new ActivityTimestamps();

			ulong endTime = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds() + timeRemaining;

			timestamps.SetEnd(endTime);

			activity.SetTimestamps(timestamps);
			return activity;
		}

		public static Activity AddActivityAssets(ref Activity activity, DiscordImage largeImage, string largeImageUrl)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			
			ActivityAssets activityAssets = new ActivityAssets();

			activityAssets.SetLargeImage(largeImageData.ImageID);
			activityAssets.SetLargeText(largeImageData.ImageText);

			if (!string.IsNullOrWhiteSpace(largeImageUrl))
			{
				activityAssets.SetLargeUrl(largeImageUrl);
			}

			activity.SetAssets(activityAssets);
			return activity;
		}

		public static Activity AddActivityAssets(ref Activity activity, DiscordImage largeImage, string largeImageUrl, DiscordImage smallImage, string smallImageUrl)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);
			
			ActivityAssets activityAssets = new ActivityAssets();

			activityAssets.SetLargeImage(largeImageData.ImageID);
			activityAssets.SetLargeText(largeImageData.ImageText);

			if (!string.IsNullOrWhiteSpace(largeImageUrl))
			{
				activityAssets.SetLargeUrl(largeImageUrl);
			}

			activityAssets.SetSmallImage(smallImageData.ImageID);
			activityAssets.SetSmallText(smallImageData.ImageText);

			if (!string.IsNullOrWhiteSpace(smallImageUrl))
			{
				activityAssets.SetSmallUrl(smallImageUrl);
			}

			activity.SetAssets(activityAssets);
			return activity;
		}
	}
}