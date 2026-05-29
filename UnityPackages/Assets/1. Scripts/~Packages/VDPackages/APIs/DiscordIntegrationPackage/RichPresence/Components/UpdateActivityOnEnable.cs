using Discord.Sdk;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Components
{
	public sealed class UpdateActivityOnEnable : UpdateActivityComponent 
	{
		private void OnEnable()
		{
			if (!DiscordManager.CanSetActivity) // If discord is not connected, delay execution until it is
			{
				DiscordManager.OnCanSetActivity += UpdatePresence;
			}
			else
			{
				UpdatePresence();
			}
		}

		private void OnDisable()
		{
			DiscordManager.OnCanSetActivity -= UpdatePresence;
		}
		
		public override void UpdatePresence()
		{
			DiscordManager.OnCanSetActivity -= UpdatePresence;
			base.UpdatePresence();
		}
	}
}