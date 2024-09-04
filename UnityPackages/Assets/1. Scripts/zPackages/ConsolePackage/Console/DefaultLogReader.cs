using System;
using UnityEngine;

namespace ConsolePackage.Console
{
	/// <summary>
	/// A class which will "read" the default console log and write it to the custom console
	/// </summary>
	public class DefaultLogReader
	{
		public DefaultLogReader()
		{
			Application.logMessageReceivedThreaded += HandleLog;
		}

		public void OnDestroy()
		{
			Application.logMessageReceivedThreaded -= HandleLog;
		}

		private static void HandleLog(string logString, string stackTrace, LogType logType)
		{
			switch (logType)
			{
				case LogType.Log:
					Log(logString);
					break;
				case LogType.Warning:
					LogWarning(logString);
					break;
				case LogType.Assert:
				case LogType.Error:
				case LogType.Exception:
					LogException(logString, stackTrace);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
			}
		}

		private static void Log(string logString)
		{
			ConsoleManager.Log(logString, false);
		}

		private static void LogWarning(string logString)
		{
			ConsoleManager.LogWarning(logString, false);
		}

		private static void LogError(string logString)
		{
			ConsoleManager.LogError(logString, false);
		}

		private static void LogException(string logString, string stackTrace)
		{
			string exception = $"{logString}\nStackTrace:\n{stackTrace}";

			ConsoleManager.LogError(exception, false);
		}
	}
}