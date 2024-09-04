using System;
using APIs.DiscordIntegrationPackage.RichPresence;
using APIs.DiscordIntegrationPackage.RichPresence.Enums;
using APIs.DiscordIntegrationPackage.Structs;
using Discord;

namespace APIs.DiscordIntegrationPackage.Factories
{
	public static class ActivityFactory
	{
		public static Activity CreateActivity(string details)
		{
			return new Activity
			{
				Details = details,
			};
		}

		public static Activity CreateActivity(string details, DiscordImage largeImage)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);

			return new Activity
			{
				Details = details,

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, DiscordImage largeImage, DiscordImage smallImage)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);

			return new Activity
			{
				Details = details,

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
					SmallImage = smallImageData.ImageID,
					SmallText  = smallImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, string state)
		{
			return new Activity
			{
				Details = details,
				State   = state,
			};
		}

		public static Activity CreateActivity(string details, string state, DiscordImage largeImage)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);

			return new Activity
			{
				Details = details,
				State   = state,

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, string state, DiscordImage largeImage, DiscordImage smallImage)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);

			return new Activity
			{
				Details = details,
				State   = state,

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
					SmallImage = smallImageData.ImageID,
					SmallText  = smallImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, string state, bool showElapsedTime)
		{
			if (!showElapsedTime)
			{
				return CreateActivity(details, state);
			}

			return new Activity
			{
				Details = details,
				State   = state,

				Timestamps =
				{
					Start = DateTimeOffset.Now.ToUnixTimeSeconds(),
				},
			};
		}

		public static Activity CreateActivity(string details, string state, bool showElapsedTime, DiscordImage largeImage)
		{
			if (!showElapsedTime)
			{
				return CreateActivity(details, state, largeImage);
			}

			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);

			return new Activity
			{
				Details = details,
				State   = state,

				Timestamps =
				{
					Start = DateTimeOffset.Now.ToUnixTimeSeconds(),
				},

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, string state, bool showElapsedTime, DiscordImage largeImage, DiscordImage smallImage)
		{
			if (!showElapsedTime)
			{
				return CreateActivity(details, state, largeImage, smallImage);
			}

			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);

			return new Activity
			{
				Details = details,
				State   = state,

				Timestamps =
				{
					Start = DateTimeOffset.Now.ToUnixTimeSeconds(),
				},

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
					SmallImage = smallImageData.ImageID,
					SmallText  = smallImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, string state, bool showRemainingTime, long secondsRemaining)
		{
			if (!showRemainingTime)
			{
				return CreateActivity(details, state);
			}

			return new Activity
			{
				Details = details,
				State   = state,

				Timestamps =
				{
					End = DateTimeOffset.Now.ToUnixTimeSeconds() + secondsRemaining,
				},
			};
		}

		public static Activity CreateActivity(string details, string state, bool showRemainingTime, long secondsRemaining, DiscordImage largeImage)
		{
			if (!showRemainingTime)
			{
				return CreateActivity(details, state, largeImage);
			}

			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);

			return new Activity
			{
				Details = details,
				State   = state,

				Timestamps =
				{
					End = DateTimeOffset.Now.ToUnixTimeSeconds() + secondsRemaining,
				},

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
				},
			};
		}

		public static Activity CreateActivity(string details, string state, bool showRemainingTime, long secondsRemaining, DiscordImage largeImage, DiscordImage smallImage)
		{
			if (!showRemainingTime)
			{
				return CreateActivity(details, state, largeImage, smallImage);
			}

			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);

			return new Activity
			{
				Details = details,
				State   = state,

				Timestamps =
				{
					End = DateTimeOffset.Now.ToUnixTimeSeconds() + secondsRemaining,
				},

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
					SmallImage = smallImageData.ImageID,
					SmallText  = smallImageData.ImageText,
				},
			};
		}

		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		//              MULTIPLAYER
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		public static Activity CreateMultiplayerActivity(string state, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets)
		{
			return new Activity()
			{
				State = state,

				Party =
				{
					Id      = partyID,
					Privacy = partyPrivacy,

					Size =
					{
						CurrentSize = currentPartySize,
						MaxSize     = maxPartySize,
					},
				},

				Secrets = activitySecrets,
			};
		}

		public static Activity CreateMultiplayerActivity(string state, DiscordImage largeImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);

			return new Activity()
			{
				State = state,

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
				},

				Party =
				{
					Id      = partyID,
					Privacy = partyPrivacy,

					Size =
					{
						CurrentSize = currentPartySize,
						MaxSize     = maxPartySize,
					},
				},

				Secrets = activitySecrets,
			};
		}

		public static Activity CreateMultiplayerActivity(string state, DiscordImage largeImage, DiscordImage smallImage, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                     activitySecrets)
		{
			ImageData largeImageData = DiscordImageManager.Instance.GetImageID(largeImage);
			ImageData smallImageData = DiscordImageManager.Instance.GetImageID(smallImage);

			return new Activity()
			{
				State = state,

				Assets =
				{
					LargeImage = largeImageData.ImageID,
					LargeText  = largeImageData.ImageText,
					SmallImage = smallImageData.ImageID,
					SmallText  = smallImageData.ImageText,
				},

				Party =
				{
					Id      = partyID,
					Privacy = partyPrivacy,

					Size =
					{
						CurrentSize = currentPartySize,
						MaxSize     = maxPartySize,
					},
				},

				Secrets = activitySecrets,
			};
		}

		public static Activity CreateMultiplayerActivity(string details, string state, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string               state,        DiscordImage largeImage,
			string                                              partyID, ActivityPartyPrivacy partyPrivacy, int          currentPartySize, int maxPartySize, ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, largeImage);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string               state,        DiscordImage largeImage,       DiscordImage smallImage,
			string                                              partyID, ActivityPartyPrivacy partyPrivacy, int          currentPartySize, int          maxPartySize, ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, largeImage, smallImage);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string state, bool showElapsedTime, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize,
			ActivitySecrets                                     activitySecrets)
		{
			Activity activity = CreateActivity(details, state, showElapsedTime);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string               state,        bool showElapsedTime,  DiscordImage largeImage,
			string                                              partyID, ActivityPartyPrivacy partyPrivacy, int  currentPartySize, int          maxPartySize, ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, showElapsedTime, largeImage);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string               state,        bool showElapsedTime,  DiscordImage largeImage,   DiscordImage    smallImage,
			string                                              partyID, ActivityPartyPrivacy partyPrivacy, int  currentPartySize, int          maxPartySize, ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, showElapsedTime, largeImage, smallImage);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string               state,        bool showRemainingTime, long secondsRemaining,
			string                                              partyID, ActivityPartyPrivacy partyPrivacy, int  currentPartySize,  int  maxPartySize, ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, showRemainingTime, secondsRemaining);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details, string               state,        bool showRemainingTime, long secondsRemaining, DiscordImage    largeImage,
			string                                              partyID, ActivityPartyPrivacy partyPrivacy, int  currentPartySize,  int  maxPartySize,     ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, showRemainingTime, secondsRemaining, largeImage);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		public static Activity CreateMultiplayerActivity(string details,      string state,            bool showRemainingTime, long secondsRemaining, DiscordImage largeImage, DiscordImage smallImage, string partyID,
			ActivityPartyPrivacy                                partyPrivacy, int    currentPartySize, int  maxPartySize,      ActivitySecrets activitySecrets)
		{
			Activity activity = CreateActivity(details, state, showRemainingTime, secondsRemaining, largeImage, smallImage);

			AddMultiplayerInformation(ref activity, partyID, partyPrivacy, currentPartySize, maxPartySize, activitySecrets);

			return activity;
		}

		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		//              PRIVATE UTILITY
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		private static void AddMultiplayerInformation(ref Activity activity, string partyID, ActivityPartyPrivacy partyPrivacy, int currentPartySize, int maxPartySize, ActivitySecrets activitySecrets)
		{
			activity.Party = new ActivityParty
			{
				Id      = partyID,
				Privacy = partyPrivacy,
				Size =
				{
					CurrentSize = currentPartySize,
					MaxSize     = maxPartySize,
				},
			};

			activity.Secrets = activitySecrets;
		}
	}
}