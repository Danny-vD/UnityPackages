namespace VDPackages.APIs
{
	/// <summary>
	/// Contains globally accessible data that is required by the integration APIs
	/// </summary>
	public static class ApplicationData
	{
		public const ulong DISCORD_APPLICATION_ID = 1374698555398291496; // AKA Client ID

		public const uint STEAM_APPLICATION_ID = 480; // Default = 480 (Spacewar)

		public static readonly string SteamStartCommand = "steam://rungameid/" + STEAM_APPLICATION_ID;
	}
}