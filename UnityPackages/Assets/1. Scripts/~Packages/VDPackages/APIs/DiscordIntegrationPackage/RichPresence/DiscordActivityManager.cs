using System;
using Discord.Sdk;
using VDPackages.APIs.DiscordIntegrationPackage.Factories;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Enums;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence
{
	public static class DiscordActivityManager
	{
		public static event Action OnActivityUpdated = null;
		
		public static void ClearActivity()
		{
			DiscordManager.DiscordClient.ClearRichPresence();
		}
		
		public static void SetActivity(Activity activity)
		{
			activity.SetType(ActivityTypes.Playing);
			
			DiscordManager.DiscordClient.UpdateRichPresence(activity, TryUpdatingActivityCallback);
		}

		public static void UpdateRichPresence(string details)
		{
			SetActivity(ActivityFactory.CreateActivity(details));
		}

		/// <summary>
		/// Create an activity with details and state
		/// </summary>
		/// <param name="details">Main activity description</param>
		/// <param name="state">Secondary status (e.g., “In Queue”, “In Match, “In a group”)</param>
		/// <returns></returns>
		public static void UpdateRichPresence(string details, string state)
		{
			Activity activity = ActivityFactory.CreateActivity(details, state);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, largeImage, largeImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, DiscordImage largeImage, DiscordImage smallImage, string largeImageUrl = "", string smallImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, largeImage, smallImage, largeImageUrl, smallImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, largeImage, largeImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, DiscordImage largeImage, DiscordImage smallImage, string largeImageUrl = "", string smallImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, largeImage, smallImage, largeImageUrl, smallImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, ulong startTime)
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, startTime);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, ulong startTime, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, startTime, largeImage, largeImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, ulong startTime, DiscordImage largeImage, DiscordImage smallImage, string largeImageUrl = "", string smallImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, startTime, largeImage, smallImage, largeImageUrl, smallImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, bool showRemainingTime, ulong secondsRemaining)
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, showRemainingTime, secondsRemaining);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details, string state, bool showRemainingTime, ulong secondsRemaining, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, showRemainingTime, secondsRemaining, largeImage, largeImageUrl);

			SetActivity(activity);
		}

		public static void UpdateRichPresence(string details,            string       state, bool showRemainingTime, ulong secondsRemaining,
			DiscordImage                             largeImage,         DiscordImage smallImage,
			string                                   largeImageUrl = "", string       smallImageUrl = "")
		{
			Activity activity = ActivityFactory.CreateActivity(details, state, showRemainingTime, secondsRemaining, largeImage, smallImage, largeImageUrl, smallImageUrl);

			SetActivity(activity);
		}

		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		//              CALLBACK
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		private static void TryUpdatingActivityCallback(ClientResult clientResult)
		{
			if (clientResult.Successful())
			{
				OnActivityUpdated?.Invoke();
			}
		}
	}
}