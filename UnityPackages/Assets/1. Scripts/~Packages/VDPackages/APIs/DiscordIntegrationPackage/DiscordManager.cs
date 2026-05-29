using System;
using Discord.Sdk;
using EditorAttributes;
using UnityEngine;
using VDFramework.Logger;
using VDFramework.Singleton;
using VDFramework.Timer;
using VDFramework.Timer.TimerHandles;
using VDPackages.APIs.DiscordIntegrationPackage.Enums;
using VDPackages.APIs.DiscordIntegrationPackage.RichPresence;

namespace VDPackages.APIs.DiscordIntegrationPackage
{
	public class DiscordManager : Singleton<DiscordManager>
	{
		public static event Action OnCanSetActivity = delegate { };
		public static event Action OnDiscordClientReady = delegate { };
		public static event Action OnDiscordClientDisconnected = delegate { };

		public static event Action OnDiscordAuthorised = delegate { };
		public static event Action OnDiscordUnauthorised = delegate { };
		public static event Action OnDiscordAuthorisationFailed = delegate { };

		/// <summary>
		/// Will be called if Discord did not (re)connect and we reached the maximum connection attemps
		/// </summary>
		public static event Action OnDiscordUnableToAuthorise = delegate { };

		public static bool IsDiscordConnected { get; private set; }
		public static bool CanSetActivity { get; private set; }

		public static Client DiscordClient { get; private set; }

		[Header("Connection parameters")]
		[SerializeField, Tooltip("If the Discord authorisation failed, try again every x seconds")]
		private float authoriseAttemptTimer = 30;

		[SerializeField, Tooltip("How many times we attempt to authorise with Discord before giving up, <=0 will be considered infinite attempts")]
		private int maximumConnectionAttempts = 3;

		private TimerHandle tryAuthoriseTimer;
		private int currentAuthorisationAttempt = 0;


		[SerializeField]
		private bool requireAuthorisationFromDiscord;

		[SerializeField, ShowField(nameof(requireAuthorisationFromDiscord))]
		private DiscordOAuth2Scope oAuth2Scope = DiscordOAuth2Scope.None;

		[ShowField(nameof(UsingCustomOAuth2Scope)), SerializeField]
		private string customoAuth2Scope = Client.GetDefaultPresenceScopes();

		public bool RequireDiscordAuthorisation => requireAuthorisationFromDiscord && oAuth2Scope != DiscordOAuth2Scope.None;
		public bool DiscordAuthorised { get; private set; }

		public string AccessToken { get; private set; }
		public string RefreshToken { get; private set; }

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

			if (RequireDiscordAuthorisation)
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

		private static void OnStatusChanged(Client.Status status, Client.Error error, int errordetail)
		{
			if (status == Client.Status.Ready)
			{
				CanSetActivity     = true;
				IsDiscordConnected = true;

				OnDiscordClientReady.Invoke();
			}
			else if (status == Client.Status.Disconnected)
			{
				if (IsDiscordConnected)
				{
					IsDiscordConnected = false;
					CanSetActivity     = false;
					
					OnDiscordClientDisconnected.Invoke();
				}
			}
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

		private void OnAuthoriseResult(ClientResult result, string code, string redirectUri)
		{
			LogManager.LogInfo($"Discord Authorisation result: [{result.Error()}] [{code}] [{redirectUri}]");

			if (!result.Successful())
			{
				OnAuthorisationFailed();
				return;
			}

			currentAuthorisationAttempt = 0; // Reset connection attempt

			GetTokenFromCode(code, redirectUri);
		}

		/// <summary>
		/// Exchange the authorisation token for an access token (valid for 7 days) and a refresh token (no limit, used to refresh the access token)
		/// </summary>
		/// <param name="code"></param>
		/// <param name="redirectUri"></param>
		private void GetTokenFromCode(string code, string redirectUri)
		{
			DiscordClient.GetToken(ApplicationData.DISCORD_APPLICATION_ID, code, codeVerifier, redirectUri, GetTokenCallback);
		}

		private void GetTokenCallback(ClientResult result, string accessToken, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
		{
			if (result.Successful() && !string.IsNullOrWhiteSpace(accessToken))
			{
				LogManager.LogInfo($"Discord Token received: {accessToken}");
				DiscordClient.UpdateToken(AuthorizationTokenType.Bearer, accessToken, UpdateTokenCallback);

				OnAuthorisationSuccess(accessToken, refreshToken);
			}
			else
			{
				OnAuthorisationFailed();
			}
		}

		private static void UpdateTokenCallback(ClientResult result)
		{
			if (result.Successful())
			{
				DiscordClient.Connect();
			}
		}
		
		private void OnAuthorisationSuccess(string accessToken, string refreshToken)
		{
			tryAuthoriseTimer?.Stop();
			tryAuthoriseTimer = null;
			
			AccessToken  = accessToken;
			RefreshToken = refreshToken;
			
			DiscordAuthorised = true;
			OnDiscordAuthorised.Invoke();
		}

		private void OnAuthorisationFailed()
		{
			AccessToken  = string.Empty;
			RefreshToken = string.Empty;

			if (DiscordAuthorised)
			{
				DiscordAuthorised = false;
				OnDiscordUnauthorised.Invoke();
			}
			
			++currentAuthorisationAttempt;

			if (currentAuthorisationAttempt == maximumConnectionAttempts)
			{
				tryAuthoriseTimer?.Stop();
				tryAuthoriseTimer = null;

				OnDiscordUnableToAuthorise.Invoke();
			}
			else
			{
				if (tryAuthoriseTimer == null)
				{
					tryAuthoriseTimer = TimerManager.StartNewTimer(authoriseAttemptTimer, StartOAuthFlow, true);
				}
			}
		}

		private void RefreshAccessToken()
		{
			if (string.IsNullOrWhiteSpace(RefreshToken))
			{
				return;
			}

			DiscordClient.RefreshToken(ApplicationData.DISCORD_APPLICATION_ID, RefreshToken, RefreshTokenCallback);
		}

		private void RefreshTokenCallback(ClientResult result, string accessToken, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
		{
			if (result.Successful() && !string.IsNullOrWhiteSpace(accessToken))
			{
				LogManager.LogInfo($"Discord Token received: {accessToken}");
				DiscordClient.UpdateToken(AuthorizationTokenType.Bearer, accessToken, UpdateTokenCallback);

				AccessToken  = accessToken;
				RefreshToken = refreshToken;
			}
			else // The refresh token is not valid for any reason (user banned, user removed authorisation etc.)
			{
				StartOAuthFlow();
			}
		}

		private void RevokeAccessToken()
		{
			if (string.IsNullOrWhiteSpace(AccessToken))
			{
				return;
			}

			DiscordClient.RevokeToken(ApplicationData.DISCORD_APPLICATION_ID, AccessToken, RevokeTokenCallback);
		}

		private void RevokeTokenCallback(ClientResult result)
		{
			if (result.Successful())
			{
				AccessToken  = string.Empty;
				RefreshToken = string.Empty;
			}
		}

		private void Cleanup()
		{
			if (IsDiscordConnected)
			{
				IsDiscordConnected = false;

				DiscordClient.Disconnect();
				DiscordClient.Dispose();
				OnDiscordUnauthorised.Invoke();
			}
			else
			{
				tryAuthoriseTimer?.Stop();
				tryAuthoriseTimer = null;
			}

			if (CanSetActivity)
			{
				DiscordActivityManager.ClearActivity();
				CanSetActivity = false;
			}
		}

		protected override void OnDestroy()
		{
			Cleanup();

			base.OnDestroy();
		}


		//\\//\\//\\//\\//\\//\\//
		// Editor Attributes Functions
		//\\//\\//\\//\\//\\//\\//
		private bool UsingCustomOAuth2Scope()
		{
			return oAuth2Scope == DiscordOAuth2Scope.Custom;
		}
	}
}