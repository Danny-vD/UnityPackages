using Discord.Sdk;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Components
{
	public sealed class SetActivityOnEnable : SetActivityComponent 
	{
		private void OnEnable()
		{
			if (!DiscordManager.IsDiscordConnected) // If discord is not connected, delay execution until it is
			{
				DiscordManager.OnDiscordClientReady += UpdatePresence;
			}
			else
			{
				UpdatePresence();
			}
		}

		private void OnDisable()
		{
			DiscordManager.OnDiscordClientReady -= UpdatePresence;
		}
		
		public override void UpdatePresence()
		{
			DiscordManager.OnDiscordClientReady -= UpdatePresence;
			base.UpdatePresence();
		}
	}
}