using Discord;
using VDFramework.Logger;

namespace VDPackages.APIs.DiscordIntegrationPackage
{
	public static class DiscordDebugLogger
	{
		public static void Initialize(Discord.Discord discord)
		{
			discord.SetLogHook(LogLevel.Debug, LogToConsole);
		}

		private static void LogToConsole(LogLevel level, string message)
		{
			switch (level)
			{
				case LogLevel.Error:
					LogManager.LogError(message);
					break;
				case LogLevel.Warn:
					LogManager.LogWarning(message);
					break;
				case LogLevel.Info:
					LogManager.LogInfo(message);
					break;
				case LogLevel.Debug:
					LogManager.LogDebug(message);
					break;
				default:
					LogManager.LogInfo(message);
					break;
			}
		}
	}
}