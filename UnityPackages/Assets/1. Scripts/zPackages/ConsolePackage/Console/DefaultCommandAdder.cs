using System;
using ConsolePackage.Commands;
using UnityEngine;
using UnityEngine.Serialization;

namespace ConsolePackage.Console
{
	[Serializable]
	public class DefaultCommandAdder
	{
		[FormerlySerializedAs("helpCommand")]
		[Tooltip("The command to display the help page")]
		public string[] helpCommands = { "help", "?" };

		[FormerlySerializedAs("clearCommand")]
		[Tooltip("The command to clear the console")]
		public string[] clearCommands = { "clear", "cls" };

		[FormerlySerializedAs("exitCommand")]
		[Tooltip("The commands to exit the application")]
		public string[] exitCommands = { "exit", "quit" };

		public void AddCommands()
		{
			if (helpCommands.Length == 0)
			{
				helpCommands = new[] { "help" };
			}
			
			CommandManager.SetHelp(helpCommands);
			AddClear();
			AddExit();
		}

		private void AddClear()
		{
			if (clearCommands.Length == 0)
			{
				return;
			}
			
			AbstractCommand clear = new Command(clearCommands[0], ConsoleManager.Clear);
			clear.SetHelpMessage("Clears the console.");
			
			for (int i = 1; i < clearCommands.Length; i++)
			{
				clear.AddAlias(clearCommands[i]);
			}

			CommandManager.AddCommand(clear);
		}

		private void AddExit()
		{
			if (exitCommands.Length == 0)
			{
				return;
			}

			AbstractCommand exit = new Command(exitCommands[0], ExitApplication);
			exit.SetHelpMessage("Quits the program immediately.");

			for (int i = 1; i < exitCommands.Length; i++)
			{
				exit.AddAlias(exitCommands[i]);
			}
			
			CommandManager.AddCommand(exit);
		}

		private void ExitApplication()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}