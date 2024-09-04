using System;

namespace APIs.DiscordIntegrationPackage.RichPresence.Enums
{
	[Flags]
	public enum DynamicDataLocation
	{
		Details = 1,
		State = 1 << 1,
	}
}