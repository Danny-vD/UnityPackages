using System;
using APIs.DiscordIntegrationPackage.RichPresence.Enums;
using APIs.DiscordIntegrationPackage.Structs;
using Discord;
using UnityEngine;
using VDFramework;
using VDFramework.Utility;

namespace APIs.DiscordIntegrationPackage.RichPresence.Components.DynamicData
{
	public abstract class AbstractDynamicActivity : BetterMonoBehaviour
	{
		[SerializeField]
		private DynamicDataLocation dynamicDataLocation = DynamicDataLocation.State;
		
		[Header("Details")]
		[SerializeField]
		private bool showDetails;

		[SerializeField]
		private string details;

		[Header("Status")]
		[SerializeField]
		private bool showState;

		[SerializeField]
		private string state;

		[Header("Timer")]
		[SerializeField]
		private TimerShown timerShown;

		[SerializeField]
		private long secondsRemaining;

		[Header("Images")]
		[SerializeField]
		private ImageShown imageShown;

		[SerializeField]
		private DiscordImage largeImage;
		
		[SerializeField]
		private DiscordImage smallImage;

		private StringVariableWriter detailsWriter;
		private StringVariableWriter stateWriter;

		protected virtual void Awake()
		{
			detailsWriter = new StringVariableWriter(details);
			stateWriter   = new StringVariableWriter(state);
		}

		public void UpdatePresence()
		{
			if (!DiscordManager.IsDiscordConnected)
			{
				return;
			}
			
			Activity activity = new Activity();

			if (showDetails)
			{
				activity.Details = dynamicDataLocation.HasFlag(DynamicDataLocation.Details) ? detailsWriter.UpdateText(GetData()) : details;
			}

			if (showState)
			{
				activity.State = dynamicDataLocation.HasFlag(DynamicDataLocation.State) ? stateWriter.UpdateText(GetData()) : state;
			}

			switch (timerShown)
			{
				default:
				case TimerShown.None:
					break;
				case TimerShown.TimeElapsed:
					activity.Timestamps = new ActivityTimestamps
					{
						Start = DateTimeOffset.Now.ToUnixTimeSeconds(),
					};
					break;
				case TimerShown.TimeRemaining:
					activity.Timestamps = new ActivityTimestamps
					{
						End = DateTimeOffset.Now.ToUnixTimeSeconds() + secondsRemaining,
					};
					break;
			}
			
			ImageData largeImageData = default;

			switch (imageShown)
			{
				default:
				case ImageShown.None:
					break;
				case ImageShown.LargeOnly:
					largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
					
					activity.Assets = new ActivityAssets
					{
						LargeImage = largeImageData.ImageID,
						LargeText  = largeImageData.ImageText,
					};
					break;
				case ImageShown.LargeAndSmall:
					largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
					ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);
					
					activity.Assets = new ActivityAssets
					{
						LargeImage = largeImageData.ImageID,
						LargeText  = largeImageData.ImageText,
						SmallImage = smallImageData.ImageID,
						SmallText  = smallImageData.ImageText,
					};
					break;
			}
			
			DiscordPresenceManager.UpdatePresence(activity);
		}

		protected abstract object[] GetData();
	}
}