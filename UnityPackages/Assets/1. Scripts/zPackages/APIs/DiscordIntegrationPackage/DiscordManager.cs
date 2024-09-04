using System;
using APIs.DiscordIntegrationPackage.RichPresence;
using Discord;
using UnityEngine;
using VDFramework.Singleton;
using VDFramework.Utility.TimerUtil;
using VDFramework.Utility.TimerUtil.TimerHandles;

namespace APIs.DiscordIntegrationPackage
{
	public class DiscordManager : Singleton<DiscordManager>
	{
		public static event Action<Discord.Discord> OnDiscordConnected = delegate { };
		public static event Action OnDiscordDisconnected = delegate { };

		/// <summary>
		/// Will be called if a connection attempt fails
		/// </summary>
		/// <seealso cref="Result"/>
		public static event Action OnDiscordConnectionFailed = delegate { };

		/// <summary>
		/// Will be called if Discord did not (re)connect and we reached the maximum connection attemps
		/// </summary>
		public static event Action OnDiscordUnableToConnect = delegate { };

		/// <summary>
		/// Will be called if the initial connection returns a <see cref="Result.NotInstalled"/>
		/// </summary>
		public static event Action OnDiscordNotInstalled = delegate { };

		public static bool IsDiscordConnected { get; private set; }

		public static Discord.Discord Discord { get; private set; }

		[Header("Platform settings")]
		[SerializeField, Tooltip("If discord is not open when the game starts, discord will:\n1. close the game\n2. attempt to open discord\n3. attempt to reopen the game")]
		private bool discordIsRequiredForGameToWork = false;

		[Header("Connection paramaters")]
		[SerializeField, Tooltip("If Discord is disconnected, try to reconnect every x seconds")]
		private float reconnectTimer = 2.0f;

		[SerializeField, Tooltip("How many times we attempt to connect with discord before giving up, <=0 will be considered infinite attempts")]
		private int maximumConnectionAttempts = 6;

		private TimerHandle tryReconnectTimer;
		private int currentConnectionAttempt = 0;

		private void Start()
		{
			if (!transform.parent)
			{
				DontDestroyOnLoad(true);
			}

			TryConnectingWithDiscord();
		}

		private void OnDisable()
		{
			Cleanup();
		}
		
		/// <summary>
		/// Will reset the current connection count and attempt to connect with discord
		/// </summary>
		public static void ResetConnection()
		{
			DiscordManager discordManager = Instance;
			
			discordManager.Cleanup();
			discordManager.currentConnectionAttempt = 0;
			
			discordManager.TryConnectingWithDiscord();
		}

		private ulong GetDiscordFlag()
		{
			return (ulong)(discordIsRequiredForGameToWork ? CreateFlags.Default : CreateFlags.NoRequireDiscord);
		}

		private void TryConnectingWithDiscord()
		{
			try
			{
				Discord = new Discord.Discord(ApplicationData.DISCORD_APPLICATION_ID, GetDiscordFlag());

				DiscordConnected();
			}
			catch (ResultException resultException)
			{
				// Any result exception is bad and means the connection did not go through

				if (resultException.Result is Result.NotInstalled)
				{
					OnDiscordNotInstalled.Invoke();
					Cleanup();
				}
				else
				{
					DiscordConnectionFailed();
				}
			}
		}
		
		private static void InitializeManagers()
		{
#if UNITY_EDITOR
			DiscordDebugLogger.Initialize(Discord);
#endif

			DiscordPresenceManager.Initialize(Discord);
		}

		private void DiscordConnected()
		{
			tryReconnectTimer?.Stop(); // Stop trying to reconnect if we connected
			currentConnectionAttempt = 0;

			IsDiscordConnected = true;

			InitializeManagers();

			OnDiscordConnected.Invoke(Discord);
		}

		private void DiscordDisconnected()
		{
			IsDiscordConnected = false;

			Discord.Dispose();
			OnDiscordDisconnected.Invoke();

			tryReconnectTimer?.Stop();
			tryReconnectTimer = null;

			tryReconnectTimer = TimerManager.StartNewTimer(reconnectTimer, TryConnectingWithDiscord, true);
		}

		private void DiscordConnectionFailed()
		{
			OnDiscordConnectionFailed.Invoke();
			++currentConnectionAttempt;

			if (currentConnectionAttempt == maximumConnectionAttempts)
			{
				tryReconnectTimer?.Stop();
				tryReconnectTimer = null;

				OnDiscordUnableToConnect.Invoke();
			}
			else
			{
				if (tryReconnectTimer == null)
				{
					tryReconnectTimer = TimerManager.StartNewTimer(reconnectTimer, TryConnectingWithDiscord, true);
				}
			}
		}

		private void Update()
		{
			if (IsDiscordConnected)
			{
				try
				{
					Discord.RunCallbacks();
				}
				catch (ResultException)
				{
					// Any result exception is bad and means something is wrong with the connection
					DiscordDisconnected();
				}
			}
		}

		private void Cleanup()
		{
			if (IsDiscordConnected)
			{
				IsDiscordConnected = false;

				Discord.Dispose();
				OnDiscordDisconnected.Invoke();
			}
			else
			{
				tryReconnectTimer?.Stop();
				tryReconnectTimer = null;
			}
		}

		protected override void OnDestroy()
		{
			Cleanup();

			base.OnDestroy();
		}
	}
}