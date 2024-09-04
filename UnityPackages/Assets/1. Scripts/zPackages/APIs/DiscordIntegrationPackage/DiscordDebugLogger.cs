using Discord;
using UnityEngine;

namespace APIs.DiscordIntegrationPackage
{
	public static class DiscordDebugLogger
	{
		public static void Initialize(Discord.Discord discord)
		{
			discord.SetLogHook(LogLevel.Debug, LogToConsole);
		}

		private static void LogToConsole(LogLevel level, string message)
		{
			Debug.Log($"[{level}]: {message}");
		}
	}
}