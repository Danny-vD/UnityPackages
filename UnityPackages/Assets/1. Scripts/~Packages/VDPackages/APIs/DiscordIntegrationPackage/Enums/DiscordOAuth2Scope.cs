namespace VDPackages.APIs.DiscordIntegrationPackage.Enums
{
	public enum DiscordOAuth2Scope
	{
		None,
		DefaultPresence, // core features like account linking, friends list, and rich presence (not needed for rich presence to work)
		DefaultCommunication, // lobbies, voice chat, or direct messaging
		Custom,
	}
}