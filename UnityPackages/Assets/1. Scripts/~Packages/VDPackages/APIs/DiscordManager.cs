using EditorAttributes;
using UnityEngine;
using VDFramework;
using VDPackages.APIs.DiscordIntegrationPackage;

namespace VDPackages.APIs
{
	public class DiscordManagerOLD : BetterMonoBehaviour
	{
		[SerializeField, TextArea(minLines: 8, maxLines: 16)]
		private string messageContent; 

		[Button("Send message")]
		private void SendDiscordMessage()
		{
			DiscordManager.DiscordClient.SendUserMessage(250356636923854858, messageContent, (result, id) => { });
			messageContent = string.Empty;
		}
	}
}