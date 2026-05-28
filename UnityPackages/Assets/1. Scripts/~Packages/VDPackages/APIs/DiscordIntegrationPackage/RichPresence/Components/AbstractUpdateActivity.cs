using Discord.Sdk;
using EditorAttributes;
using UnityEngine;
using VDFramework;
using VDPackages.APIs.DiscordIntegrationPackage.Factories;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Enums;
using Void = EditorAttributes.Void;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Components
{
	public abstract class AbstractUpdateActivity : BetterMonoBehaviour
	{
		[Header("Details")]
		[SerializeField, HelpBox("Main activity description (e.g., “Playing Capture the Flag”)", MessageMode.None)]
		private bool showDetails;

		[SerializeField, ShowField(nameof(showDetails))]
		private string details;

		[Header("State")]
		[SerializeField, HelpBox("Secondary status (e.g., “In Queue”, “In Match, “In a group”)", MessageMode.None)]
		private bool showState;

		[SerializeField, ShowField(nameof(showState))]
		private string state;

		[Header("Timer")]
		[SerializeField]
		private TimerShown timerShown;

		[ShowField(nameof(ShowingTimeRemaining))]
		[SerializeField]
		private ulong secondsRemaining;

		[Header("Images")]
		[SerializeField]
		private ImageShown imageShown;
		
		[ShowField(nameof(ShowingSmallImage))]
		[SerializeField, TabGroup(nameof(largeImageGroup), nameof(smallImageGroup))]
		private Void tabGroup;
		
		[ShowField(nameof(ShowingOnlyLargeImage))]
		[SerializeField, TabGroup(nameof(largeImageGroup))]
		private Void tabGroupLargeOnly;
		
		[HideProperty]
		[SerializeField, Rename("Large Image"), VerticalGroup(nameof(largeImage), nameof(largeImageURL))]
		private Void largeImageGroup;

		[HideProperty, SerializeField]
		private DiscordImage largeImage;

		[HideProperty, SerializeField, Prefix("Optional")]
		private string largeImageURL = "";
		
		[HideInInspector]
		[SerializeField, Rename("Large Image"), VerticalGroup(nameof(smallImage), nameof(smallImageURL))]
		private Void smallImageGroup;
		
		[HideProperty, SerializeField]
		private DiscordImage smallImage;
		
		[HideProperty, SerializeField, Prefix("Optional")]
		private string smallImageURL = "";
		
		protected void UpdatePresence()
		{
			if (!DiscordManager.IsDiscordConnected)
			{
				return;
			}
			
			Activity activity = new Activity();

			if (showDetails)
			{
				activity.SetDetails(details);
			}

			if (showState)
			{
				activity.SetState(state);
			}

			switch (timerShown)
			{
				default:
				case TimerShown.None:
					break;
				case TimerShown.TimeElapsed:

					ActivityFactory.AddTimeStampsStart(ref activity, 0);
					
					break;
				case TimerShown.TimeRemaining:
					ActivityFactory.AddTimeStampsEnd(ref activity, secondsRemaining);
					break;
			}

			switch (imageShown)
			{
				default:
				case ImageShown.None:
					break;
				case ImageShown.LargeOnly:
					ActivityFactory.AddActivityAssets(ref activity, largeImage, largeImageURL);
					break;
				case ImageShown.LargeAndSmall:
					ActivityFactory.AddActivityAssets(ref activity, largeImage, largeImageURL, smallImage, smallImageURL);
					break;
			}
			
			//DiscordPresenceManager.SetActivity(activity);
		}

		private bool ShowingTimeRemaining()
		{
			return timerShown == TimerShown.TimeRemaining;
		}
		
		private bool ShowingOnlyLargeImage()
		{
			return imageShown == ImageShown.LargeOnly;
		}
		
		private bool ShowingSmallImage()
		{
			return imageShown == ImageShown.LargeAndSmall;
		}
	}
}