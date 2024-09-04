namespace APIs.DiscordIntegrationPackage.RichPresence.Components
{
	public sealed class UpdateActivityOnEnable : AbstractUpdateActivity 
	{
		private void OnEnable()
		{
			if (!DiscordManager.IsDiscordConnected) // If discord is not connected, delay execution until it is
			{
				DiscordManager.OnDiscordConnected += UpdatePresence;
			}
			else
			{
				UpdatePresence();
			}
		}

		private void OnDisable()
		{
			DiscordManager.OnDiscordConnected -= UpdatePresence;
		}
		
		private void UpdatePresence(Discord.Discord discord)
		{
			UpdatePresence();
			DiscordManager.OnDiscordConnected -= UpdatePresence;
		}
	}
}