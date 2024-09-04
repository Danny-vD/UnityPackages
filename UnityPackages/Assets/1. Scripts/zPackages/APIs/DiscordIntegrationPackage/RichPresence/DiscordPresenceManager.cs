using APIs.DiscordIntegrationPackage.Factories;
using APIs.DiscordIntegrationPackage.RichPresence.Enums;
using Discord;

namespace APIs.DiscordIntegrationPackage.RichPresence
{
	public static class DiscordPresenceManager
	{
		private static ActivityManager activityManager;

		public static void Initialize(Discord.Discord discord)
		{
			activityManager = discord.GetActivityManager();
		}

		public static void ClearActivity(ActivityManager.ClearActivityHandler callback = null)
		{
			activityManager.ClearActivity(callback ?? EmptyCallback);
		}

		public static void UpdatePresence(Activity activity, ActivityManager.UpdateActivityHandler callback = null)
		{
			activityManager.UpdateActivity(activity, callback ?? EmptyCallback);
		}

		public static void UpdatePresence(string details, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details), callback);
		}

		public static void UpdatePresence(string details, DiscordImage largeImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, largeImage), callback);
		}

		public static void UpdatePresence(string details, DiscordImage largeImage, DiscordImage smallImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, largeImage, smallImage), callback);
		}

		public static void UpdatePresence(string details, string state, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state), callback);
		}

		public static void UpdatePresence(string details, string state, DiscordImage largeImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, largeImage), callback);
		}

		public static void UpdatePresence(string details, string state, DiscordImage largeImage, DiscordImage smallImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, largeImage, smallImage), callback);
		}

		public static void UpdatePresence(string details, string state, bool showElapsedTime, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, showElapsedTime), callback);
		}

		public static void UpdatePresence(string details, string state, bool showElapsedTime, DiscordImage largeImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, showElapsedTime, largeImage), callback);
		}

		public static void UpdatePresence(string details, string state, bool showElapsedTime, DiscordImage largeImage, DiscordImage smallImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, showElapsedTime, largeImage, smallImage), callback);
		}

		public static void UpdatePresence(string details, string state, bool showRemainingTime, long secondsRemaining, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, showRemainingTime, secondsRemaining), callback);
		}

		public static void UpdatePresence(string details, string state, bool showRemainingTime, long secondsRemaining, DiscordImage largeImage, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, showRemainingTime, secondsRemaining, largeImage), callback);
		}

		public static void UpdatePresence(string  details, string state, bool showRemainingTime, long secondsRemaining, DiscordImage largeImage, DiscordImage smallImage,
			ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateActivity(details, state, showRemainingTime, secondsRemaining, largeImage, smallImage), callback);
		}

		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		//              MULTIPLAYER
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		public static void UpdatePresenceMultiplayer(string state, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets,
			ActivityManager.UpdateActivityHandler           callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(state, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string state, DiscordImage largeImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets,
			ActivityManager.UpdateActivityHandler           callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(state, largeImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string state,           DiscordImage largeImage, DiscordImage smallImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                 activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(state, largeImage, smallImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details, string state, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets,
			ActivityManager.UpdateActivityHandler           callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details,         string state, DiscordImage largeImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                 activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, largeImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details,      string          state, DiscordImage largeImage, DiscordImage smallImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize,
			int                                             maxPartySize, ActivitySecrets activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, largeImage, smallImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details,         string state, bool showElapsedTime, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                 activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, showElapsedTime, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details, string state, bool showElapsedTime, DiscordImage largeImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                 activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, showElapsedTime, largeImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details,          string state, bool showElapsedTime, DiscordImage largeImage, DiscordImage smallImage, string partyID, ActivityPartyPrivacy partyPrivacy,
			int                                             currentPartySize, int    maxPartySize, ActivitySecrets activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, showElapsedTime, largeImage, smallImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details, string state, bool showRemainingTime, long secondsRemaining, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                 activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, showRemainingTime, secondsRemaining, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details,          string state, bool showRemainingTime, long secondsRemaining, DiscordImage largeImage, string partyID, ActivityPartyPrivacy partyPrivacy,
			int                                             currentPartySize, int    maxPartySize, ActivitySecrets activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, showRemainingTime, secondsRemaining, largeImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets), callback);
		}

		public static void UpdatePresenceMultiplayer(string details,      string state,            bool showRemainingTime, long secondsRemaining, DiscordImage largeImage, DiscordImage smallImage, string partyID,
			ActivityPartyPrivacy                            partyPrivacy, int    currentPartySize, int  maxPartySize,      ActivitySecrets activitySecrets, ActivityManager.UpdateActivityHandler callback = null)
		{
			UpdatePresence(ActivityFactory.CreateMultiplayerActivity(details, state, showRemainingTime, secondsRemaining, largeImage, smallImage, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets),
				callback);
		}


		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		//              PRIVATE UTILITY
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		private static void EmptyCallback(Result result)
		{
		}
	}
}