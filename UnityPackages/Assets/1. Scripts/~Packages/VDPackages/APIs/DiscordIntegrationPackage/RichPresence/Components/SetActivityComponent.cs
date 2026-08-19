using Discord.Sdk;
using EditorAttributes;
using UnityEngine;
using VDFramework;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Enums;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Utility;
using Void = EditorAttributes.Void;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Components
{
	public class SetActivityComponent : BetterMonoBehaviour
	{
		[Header("Details")]
		[SerializeField, HelpBox("Main activity description (e.g., “Playing Capture the Flag”)", MessageMode.None)]
		protected bool showDetails;

		[SerializeField, ShowField(nameof(showDetails))]
		protected string details;

		[Header("State")]
		[SerializeField, HelpBox("Secondary status (e.g., “In Queue”, “In Match, “In a group”)", MessageMode.None)]
		protected bool showState;

		[SerializeField, ShowField(nameof(showState))]
		protected string state;

		[Header("Timer")]
		[SerializeField]
		protected TimerShown timerShown;

		[ShowField(nameof(ShowingTimeRemaining))]
		[SerializeField]
		protected ulong secondsRemaining;

		[Header("Images")]
		[SerializeField]
		protected ImageShown imageShown;
		
		[ShowField(nameof(ShowingSmallImage))]
		[SerializeField, TabGroup(nameof(largeImageGroup), nameof(smallImageGroup))]
		private Void tabGroup;
		
		[ShowField(nameof(ShowingOnlyLargeImage))]
		[SerializeField, TabGroup(nameof(largeImageGroup))]
		private Void tabGroupLargeOnly;
		
		[HideProperty]
		[SerializeField, VerticalGroup(nameof(largeImage), nameof(largeImageURL))]
		private Void largeImageGroup;

		[HideProperty, SerializeField]
		protected DiscordImage largeImage;

		[HideProperty, SerializeField, Prefix("Optional")]
		protected string largeImageURL = "";
		
		[HideInInspector]
		[SerializeField, VerticalGroup(nameof(smallImage), nameof(smallImageURL))]
		private Void smallImageGroup;
		
		[HideProperty, SerializeField]
		protected DiscordImage smallImage;
		
		[HideProperty, SerializeField, Prefix("Optional")]
		protected string smallImageURL = "";

		[Header("Buttons")]
		[SerializeField]
		protected bool addButton;

		[ShowField(nameof(addButton)), SerializeField]
		protected string button1Label = "";
		[ShowField(nameof(addButton)), SerializeField]
		protected string button1URL = "";

		[Space]
		[ShowField(nameof(addButton)), SerializeField]
		protected bool addAnotherButton;
		
		[ShowField(nameof(addAnotherButton)), SerializeField]
		protected string button2Label = "";
		[ShowField(nameof(addAnotherButton)), SerializeField]
		protected string button2URL = "";
		
		
		public virtual void UpdatePresence()
		{
			if (!DiscordManager.IsDiscordConnected)
			{
				return;
			}
			
			Activity activity = new Activity();

			if (showDetails)
			{
				activity.SetDetails(GetDetailsString());
			}

			if (showState)
			{
				activity.SetState(GetStateString());
			}

			switch (timerShown)
			{
				default:
				case TimerShown.None:
					break;
				case TimerShown.TimeElapsed:

					ActivityUtility.AddTimeStampsStart(ref activity, GetStartTime());
					
					break;
				case TimerShown.TimeRemaining:
					ActivityUtility.AddTimeStampsEnd(ref activity, GetTimeRemaining());
					break;
			}

			switch (imageShown)
			{
				default:
				case ImageShown.None:
					break;
				case ImageShown.LargeOnly:
					ActivityUtility.AddActivityAssets(ref activity, GetLargeImage(), GetLargeImageURL());
					break;
				case ImageShown.LargeAndSmall:
					ActivityUtility.AddActivityAssets(ref activity, GetLargeImage(), GetLargeImageURL(), GetSmallImage(), GetSmallImageURL());
					break;
			}

			if (addButton)
			{
				ActivityUtility.AddButton(ref activity, button1Label, button1URL);

				if (addAnotherButton) // Rich presence supports up to 2 buttons
				{
					ActivityUtility.AddButton(ref activity, button2Label, button2URL);
				}
			}
			
			DiscordActivityManager.SetActivity(activity);
		}

		//\\//\\//\\//
		// DETAILS
		//\\//\\//\\//
		
		protected virtual string GetDetailsString()
		{
			return details;
		}
		
		//\\//\\//\\//
		// STATE
		//\\//\\//\\//
		
		protected virtual string GetStateString()
		{
			return state;
		}
		
		//\\//\\//\\//
		// TIMER
		//\\//\\//\\//
		
		protected virtual ulong GetStartTime()
		{
			return 0;
		}
		
		protected virtual ulong GetTimeRemaining()
		{
			return secondsRemaining;
		}

		//\\//\\//\\//
		// IMAGES
		//\\//\\//\\//
		
		protected virtual DiscordImage GetLargeImage()
		{
			return largeImage;
		}
		
		protected virtual string GetLargeImageURL()
		{
			return largeImageURL;
		}

		protected virtual DiscordImage GetSmallImage()
		{
			return smallImage;
		}
		
		protected virtual string GetSmallImageURL()
		{
			return smallImageURL;
		}

		//\\//\\//\\//
		// PRIVATE
		//\\//\\//\\//
		
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