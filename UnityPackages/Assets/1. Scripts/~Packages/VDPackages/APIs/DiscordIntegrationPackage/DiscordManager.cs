using System;
using Discord.Sdk;
using EditorAttributes;
using UnityEngine;
using VDFramework.Logger;
using VDFramework.Singleton;
using VDFramework.Timer.TimerHandles;
using VDPackages.APIs.DiscordIntegrationPackage.Enums;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence;

namespace VDPackages.APIs.DiscordIntegrationPackage
{
	public class DiscordManager : Singleton<DiscordManager>
	{
		public static event Action OnCanSetActivity = delegate { };

		public static event Action<Client> OnDiscordConnected = delegate { };
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
		public static bool CanSetActivity { get; private set; }

		public static Client DiscordClient { get; private set; }

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


		[SerializeField]
		private bool requireAuthorisationFromDiscord;

		[SerializeField, ShowField(nameof(requireAuthorisationFromDiscord))]
		private DiscordOAuth2Scope oAuth2Scope = DiscordOAuth2Scope.None;

		[ShowField(nameof(UsingCustomOAuth2Scope)), SerializeField]
		private string customoAuth2Scope = Client.GetDefaultPresenceScopes();

		private string codeVerifier;


		protected override void Awake()
		{
			base.Awake();

			if (!transform.parent)
			{
				DontDestroyOnLoad(true);
			}
		}

		private void Start()
		{
			Initialise();

			if (requireAuthorisationFromDiscord && oAuth2Scope != DiscordOAuth2Scope.None)
			{
				StartOAuthFlow();
				return;
			}

			DiscordClient.SetApplicationId(ApplicationData.DISCORD_APPLICATION_ID);

			CanSetActivity = true;
			OnCanSetActivity.Invoke();
		}

		private void Initialise()
		{
			DiscordClient = new Client();
			
			DiscordDebugLogger.Initialize(DiscordClient, LoggingSeverity.Warning);
			DiscordClient.SetStatusChangedCallback(OnStatusChanged);
		}

		private void StartOAuthFlow()
		{
			AuthorizationCodeVerifier authorizationCodeVerifier = DiscordClient.CreateAuthorizationCodeVerifier();
			codeVerifier = authorizationCodeVerifier.Verifier();

			AuthorizationArgs args = new AuthorizationArgs();
			args.SetClientId(ApplicationData.DISCORD_APPLICATION_ID);
			args.SetScopes(GetAuthorisationScope());
			args.SetCodeChallenge(authorizationCodeVerifier.Challenge());
			DiscordClient.Authorize(args, OnAuthoriseResult);
		}

		private void OnAuthoriseResult(ClientResult result, string code, string redirectUri)
		{
			LogManager.LogInfo($"Discord Authorisation result: [{result.Error()}] [{code}] [{redirectUri}]");

			if (!result.Successful())
			{
				return;
			}

			GetTokenFromCode(code, redirectUri);
		}

		private void GetTokenFromCode(string code, string redirectUri)
		{
			DiscordClient.GetToken(ApplicationData.DISCORD_APPLICATION_ID, code, codeVerifier, redirectUri, GetTokenCallback);
		}

		private void GetTokenCallback(ClientResult result, string accesstoken, string refreshtoken, AuthorizationTokenType tokentype, int expiresin, string scopes)
		{
			if (!string.IsNullOrWhiteSpace(accesstoken))
			{
				LogManager.LogInfo($"Discord Token received: {accesstoken}");
				DiscordClient.UpdateToken(AuthorizationTokenType.Bearer, accesstoken, UpdateTokenCallback);
			}
		}

		private static void UpdateTokenCallback(ClientResult result)
		{
			if (result.Successful())
			{
				DiscordClient.Connect();
			}
		}

		private void OnStatusChanged(Client.Status status, Client.Error error, int errordetail)
		{
			if (status == Client.Status.Ready)
			{
				CanSetActivity     = true;
				IsDiscordConnected = true;
				OnDiscordConnected.Invoke(DiscordClient);
			}
			else if (status == Client.Status.Disconnected)
			{
				if (!IsDiscordConnected)
				{
					OnDiscordDisconnected.Invoke();
				}
				
				IsDiscordConnected = false;
				CanSetActivity     = false;
			}
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
				DiscordClient = new Discord.Discord(ApplicationData.DISCORD_APPLICATION_ID, GetDiscordFlag());

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

		private void DiscordConnected()
		{
			tryReconnectTimer?.Stop(); // Stop trying to reconnect if we connected
			currentConnectionAttempt = 0;

			IsDiscordConnected = true;

			InitializeManagers();

			OnDiscordConnected.Invoke(DiscordClient);
		}

		private void DiscordDisconnected()
		{
			IsDiscordConnected = false;

			DiscordClient.Dispose();
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
					DiscordClient.RunCallbacks();
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

				DiscordClient.Dispose();
				OnDiscordDisconnected.Invoke();
			}
			else
			{
				tryReconnectTimer?.Stop();
				tryReconnectTimer = null;
			}

			if (CanSetActivity)
			{
				DiscordActivityManager.ClearActivity();
			}
		}

		private bool UsingCustomOAuth2Scope()
		{
			return oAuth2Scope == DiscordOAuth2Scope.Custom;
		}

		private string GetAuthorisationScope()
		{
			return oAuth2Scope switch
			{
				DiscordOAuth2Scope.None => throw new ArgumentException("Cannot get the OAuth2 scope for None"),
				DiscordOAuth2Scope.DefaultPresence => Client.GetDefaultPresenceScopes(),
				DiscordOAuth2Scope.DefaultCommunication => Client.GetDefaultCommunicationScopes(),
				DiscordOAuth2Scope.Custom => customoAuth2Scope,
				_ => throw new ArgumentOutOfRangeException(),
			};
		}

		protected override void OnDestroy()
		{
			Cleanup();

			base.OnDestroy();
		}
	}
}