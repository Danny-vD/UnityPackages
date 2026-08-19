namespace VDPackages.APIs
{
	/// <summary>
	/// Contains globally accessible data that is required by the integration APIs
	/// </summary>
	public static class ApplicationData
	{
		//\\//\\//\\//\\//\\//\\//
		// DISCORD
		//\\//\\//\\//\\//\\//\\//
		
		/// <remarks>
		/// Client ID
		/// </remarks>
		public const ulong DISCORD_APPLICATION_ID = 0;

		//\\//\\//\\//\\//\\//\\//
		// STEAM
		//\\//\\//\\//\\//\\//\\//
		public const uint STEAM_APPLICATION_ID = 480; // Default = 480 (Spacewar)

		public static readonly string SteamStartCommand = "steam://rungameid/" + STEAM_APPLICATION_ID;
	}
}