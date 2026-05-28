using Discord;
using Discord.Sdk;
using VDFramework.Logger;

namespace VDPackages.APIs.DiscordIntegrationPackage
{
	public static class DiscordDebugLogger
	{
		public static void Initialize(Client client, LoggingSeverity severity)
		{
			client.AddLogCallback(LogToConsole, severity);
		}

		private static void LogToConsole(string message, LoggingSeverity severity)
		{
			switch (severity)
			{
				case LoggingSeverity.Error:
					LogManager.LogError(message);
					break;
				case LoggingSeverity.Warning:
					LogManager.LogWarning(message);
					break;
				case LoggingSeverity.Info:
					LogManager.LogInfo(message);
					break;
				case LoggingSeverity.Verbose:
				case LoggingSeverity.None:
					LogManager.LogDebug(message);
					break;
				default:
					LogManager.LogInfo(message);
					break;
			}
		}
	}
}