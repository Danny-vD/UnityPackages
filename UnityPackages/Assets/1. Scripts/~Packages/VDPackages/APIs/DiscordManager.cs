using Discord.Sdk;
using EditorAttributes;
using UnityEngine;
using VDFramework;
using VDPackages.APIs.DiscordIntegrationPackage;

namespace VDPackages.APIs
{
	public class DiscordManager : BetterMonoBehaviour
	{
		private Client client;
		private string codeVerifier;

		[SerializeField, TextArea(minLines: 8, maxLines: 16)]
		private string messageContent; 

		private void Start()
		{
			client = new Client();
			
			DiscordDebugLogger.Initialize(client, LoggingSeverity.Info);
			
			client.SetStatusChangedCallback(OnStatusChanged);
		}

		private void OnStatusChanged(Client.Status status, Client.Error error, int errordetail)
		{
			if (status == Client.Status.Ready)
			{
				ClientReady();
			}
		}

		[Button("OAuth")]
		private void StartOAuthFlow()
		{
			AuthorizationCodeVerifier authorizationCodeVerifier = client.CreateAuthorizationCodeVerifier();
			codeVerifier = authorizationCodeVerifier.Verifier();

			AuthorizationArgs args = new AuthorizationArgs();
			args.SetClientId(ApplicationData.DISCORD_APPLICATION_ID);
			args.SetScopes(Client.GetDefaultCommunicationScopes());
			args.SetCodeChallenge(authorizationCodeVerifier.Challenge());
			client.Authorize(args, OnAuthoriseResult);
		}

		private void OnAuthoriseResult(ClientResult result, string code, string redirectUri)
		{
			Debug.Log($"Authorization result: [{result.Error()}] [{code}] [{redirectUri}]");

			if (!result.Successful())
			{
				return;
			}

			GetTokenFromCode(code, redirectUri);
		}

		private void GetTokenFromCode(string code, string redirectUri)
		{
			client.GetToken(ApplicationData.DISCORD_APPLICATION_ID, code, codeVerifier, redirectUri, Callback);
		}

		private void Callback(ClientResult result, string accesstoken, string refreshtoken, AuthorizationTokenType tokentype, int expiresin, string scopes)
		{
			if (string.IsNullOrWhiteSpace(accesstoken))
			{
				OnRetrieveTokenFailed();
			}
			else
			{
				OnReceivedToken(accesstoken);
			}
		}

		private void OnRetrieveTokenFailed()
		{
			Debug.Log("Failed to retrieve token");
		}
		
		private void OnReceivedToken(string accesstoken)
		{
			client.UpdateToken(AuthorizationTokenType.Bearer, accesstoken, (_ => client.Connect()));
		}

		private void ClientReady()
		{
			Activity activity = new Activity();
			activity.SetType(ActivityTypes.Playing);
			activity.SetDetails("Developing a game");
			activity.SetState("Little Chef: Cozy Cooking");
			
			client.UpdateRichPresence(activity, (_) => { });
		}

		[ContextMenu("Send Message")]
		private void SendMessage()
		{
			client.SendUserMessage(250356636923854858, messageContent, (result, id) => { });
		}

		private void OnDestroy()
		{
			client.ClearRichPresence();
		}
	}
}