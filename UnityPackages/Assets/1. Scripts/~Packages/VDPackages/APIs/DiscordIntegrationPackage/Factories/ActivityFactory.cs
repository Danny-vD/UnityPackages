using Discord.Sdk;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Enums;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Utility;

namespace VDPackages.APIs.DiscordIntegrationPackage.Factories
{
	public static class ActivityFactory
	{
		/// <summary>
		/// Create an Activity with details
		/// </summary>
		/// <param name="details">Main activity description</param>
		public static Activity CreateActivity(string details)
		{
			Activity activity = new Activity();
			activity.SetDetails(details);

			return activity;
		}

		/// <summary>
		/// Create an activity with details and state
		/// </summary>
		/// <param name="details">Main activity description</param>
		/// <param name="state">Secondary status (e.g., “In Queue”, “In Match, “In a group”)</param>
		/// <returns></returns>
		public static Activity CreateActivity(string details, string state)
		{
			Activity activity = new Activity();
			activity.SetDetails(details);
			activity.SetState(state);

			return activity;
		}

		public static Activity CreateActivity(string details, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = CreateActivity(details);

			return ActivityUtility.AddActivityAssets(ref activity, largeImage, largeImageUrl);
		}

		public static Activity CreateActivity(string details, DiscordImage largeImage, DiscordImage smallImage, string largeImageUrl = "", string smallImageUrl = "")
		{
			Activity activity = CreateActivity(details);

			return ActivityUtility.AddActivityAssets(ref activity, largeImage, largeImageUrl, smallImage, smallImageUrl);
		}

		public static Activity CreateActivity(string details, string state, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = CreateActivity(details, state);

			return ActivityUtility.AddActivityAssets(ref activity, largeImage, largeImageUrl);
		}

		public static Activity CreateActivity(string details, string state, DiscordImage largeImage, DiscordImage smallImage, string largeImageUrl = "", string smallImageUrl = "")
		{
			Activity activity = CreateActivity(details, state);

			return ActivityUtility.AddActivityAssets(ref activity, largeImage, largeImageUrl, smallImage, smallImageUrl);
		}

		public static Activity CreateActivity(string details, string state, ulong startTime)
		{
			Activity activity = CreateActivity(details, state);

			ActivityUtility.AddTimeStampsStart(ref activity, startTime);
			return activity;
		}

		public static Activity CreateActivity(string details, string state, ulong startTime, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = CreateActivity(details, state, largeImage, largeImageUrl);

			return ActivityUtility.AddTimeStampsStart(ref activity, startTime);
		}

		public static Activity CreateActivity(string details, string state, ulong startTime, DiscordImage largeImage, DiscordImage smallImage, string largeImageUrl = "", string smallImageUrl = "")
		{
			Activity activity = CreateActivity(details, state, largeImage, smallImage, largeImageUrl, smallImageUrl);

			return ActivityUtility.AddTimeStampsStart(ref activity, startTime);
		}

		public static Activity CreateActivity(string details, string state, bool showRemainingTime, ulong secondsRemaining)
		{
			Activity activity = CreateActivity(details, state);

			if (!showRemainingTime)
			{
				return activity;
			}

			return ActivityUtility.AddTimeStampsEnd(ref activity, secondsRemaining);
		}

		public static Activity CreateActivity(string details, string state, bool showRemainingTime, ulong secondsRemaining, DiscordImage largeImage, string largeImageUrl = "")
		{
			Activity activity = CreateActivity(details, state, largeImage, largeImageUrl);

			if (!showRemainingTime)
			{
				return activity;
			}

			return ActivityUtility.AddTimeStampsEnd(ref activity, secondsRemaining);
		}

		public static Activity CreateActivity(string details,            string       state, bool showRemainingTime, ulong secondsRemaining,
			DiscordImage                             largeImage,         DiscordImage smallImage,
			string                                   largeImageUrl = "", string       smallImageUrl = "")
		{
			Activity activity = CreateActivity(details, state, largeImage, smallImage, largeImageUrl, smallImageUrl);

			if (!showRemainingTime)
			{
				return activity;
			}

			return ActivityUtility.AddTimeStampsEnd(ref activity, secondsRemaining);
		}
	}
}