using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsolePackage.Console;
using UnityEngine;

namespace ConsolePackage.Commands
{
	public static class CommandManager
	{
		private const string helpHelpMessage = "Displays all commands.";
		private const string help1HelpMessage = "Displays the help page of a given command.";

		private static readonly List<AbstractCommand> commands = new List<AbstractCommand>();

		/// <summary>
		/// Invokes a given command with given parameters (does not respect user-defined conversions between types, except IConvertible)
		/// <para>Does not work with arrays (unless you invoke directly through code)</para>
		/// </summary>
		public static void Invoke(string commandName, params object[] parameters)
		{
			// 16 is the max amount of parameters we allow, because system.Action only goes up to 16 generics
			int paramsCount = Mathf.Min(parameters.Length, 16);

			AbstractCommand command = GetCommandInternal(commandName, paramsCount);

			if (command == null)
			{
				// If a command with those parameters does not exist, fall back to the same command without any parameters if possible
				AbstractCommand commandNoParams = GetCommandInternal(commandName, 0);

				if (commandNoParams == null)
				{
					ConsoleManager.LogWarning(
						$"A command with name {commandName} and {paramsCount} parameter(s) does not exist!\n" +
						$"Type {ConsoleManager.Instance.prefix}{ConsoleManager.Instance.HelpCommand} to see a list of all commands!",
						false);
					return;
				}

				commandNoParams.Invoke();
				return;
			}

			command?.Invoke(parameters);
		}

		public static void RenameCommand(string commandName, int paramsCount, string newName)
		{
			AbstractCommand command = GetCommand(commandName, paramsCount);

			if (commands.Any(item => item.HasName(newName)))
			{
				ConsoleManager.LogError($"Rename failed! A command with name {newName} already exists!");
				return;
			}

			command.SetName(newName);
		}

		public static void AddAlias(string commandName, int paramsCount, string alias)
		{
			AbstractCommand command = GetCommand(commandName, paramsCount);

			if (commands.Any(item => item.HasName(alias)))
			{
				ConsoleManager.LogError($"Adding alias failed! A command with name {alias} already exists!");
				return;
			}

			command.AddAlias(alias);
		}

		public static void RemoveAlias(string commandName, int paramsCount, string alias)
		{
			AbstractCommand command = GetCommand(commandName, paramsCount);

			command.RemoveAlias(alias);
		}

		private static AbstractCommand GetCommandInternal(string commandName, int paramsCount)
		{
			AbstractCommand command = commands.FirstOrDefault(item => item.HasName(commandName) &&
																	  item.ParametersCount == paramsCount);

			return command;
		}

		/// <summary>
		/// Returns the respective command with a given name and specified amount of parameters
		/// <para>Disclaimer: there is no error checking for adding aliases / renaming the returned command, use CommandManager instead to do so</para>
		/// </summary>
		public static AbstractCommand GetCommand(string commandName, int paramsCount)
		{
			AbstractCommand command = GetCommandInternal(commandName, paramsCount);

			if (command == null)
			{
				ConsoleManager.LogWarning(
					$"A command with name {commandName} and {paramsCount} parameter(s) does not exist!\n" +
					$"Type {ConsoleManager.Instance.prefix}{ConsoleManager.Instance.HelpCommand} to see a list of all commands!",
					false);
			}

			return command;
		}

		/// <summary>
		/// Returns a list of all commands with a given name
		/// <para>Disclaimer: there is no error checking for adding aliases / renaming the returned commands, use CommandManager instead to do so</para>
		/// </summary>
		public static List<AbstractCommand> GetCommands(string commandName)
		{
			List<AbstractCommand> tempCommands = commands.Where(command => command.HasName(commandName)).ToList();

			if (tempCommands.Count == 0)
			{
				ConsoleManager.LogWarning($"A command with name {commandName} does not exist!\n" +
										  $"Type {ConsoleManager.Instance.prefix}{ConsoleManager.Instance.HelpCommand} without any arguments to see a list of all commands!\n" +
										  "All known commands:",
					false);
			}

			return tempCommands;
		}

		public static void AddCommand(AbstractCommand command)
		{
			if (command.GetAllNames().Any(name => name.Contains(" ")))
			{
				ConsoleManager.LogError("A command name cannot contain a whitespace character!");
				return;
			}

			if (commands.Any(item => item.ParametersCount == command.ParametersCount &&
								     item.HasName(command.GetAllNames())))
			{
				ConsoleManager.LogError(
					$"A command with name {ToString(command.GetAllNames())} with {command.ParametersCount} parameter(s) already exists!");
				return;
			}

			commands.Add(command);

			string ToString(List<string> list)
			{
				if (list.Count == 0)
				{
					return string.Empty;
				}

				StringBuilder stringBuilder = new StringBuilder(list[0]);

				for (int i = 1; i < list.Count; i++)
				{
					stringBuilder.Append($", {list[i]}");
				}

				return stringBuilder.ToString();
			}
		}

		public static void RemoveCommand(string commandName, int paramsCount)
		{
			AbstractCommand command = GetCommand(commandName, paramsCount);

			commands.Remove(command);
		}

		public static void RemoveCommand(AbstractCommand command)
		{
			commands.Remove(command);
		}

		public static void SetHelp(string[] helpCommand)
		{
			string helpCommandName = helpCommand[0];

			Command help = new Command(helpCommandName, Help);
			help.SetHelpMessage(helpHelpMessage);

			Command<string> help1 = new Command<string>(helpCommandName, Help);
			help1.SetHelpMessage(help1HelpMessage);

			for (int i = 1; i < helpCommand.Length; i++)
			{
				help.AddAlias(helpCommand[i]);
				help1.AddAlias(helpCommand[i]);
			}

			AddCommand(help);
			AddCommand(help1);
		}

		private static void Help()
		{
			foreach (AbstractCommand command in commands)
			{
				ConsoleManager.Log(command.ToString(), false);
			}
		}

		private static void Help(string commandName)
		{
			List<AbstractCommand> abstractCommands = GetCommands(commandName);

			if (abstractCommands.Count == 0)
			{
				Help();
				return;
			}

			foreach (AbstractCommand command in abstractCommands)
			{
				ConsoleManager.Log(command.ToString(), false);
			}
		}
	}
}