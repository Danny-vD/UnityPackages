using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsolePackage.Console;
using UnityEngine;
using VDFramework.Extensions;

namespace ConsolePackage.Commands
{
	public abstract class AbstractCommand
	{
		public readonly int ParametersCount;

		protected string Name;
		protected readonly List<string> Aliases;

		protected string HelpMessage = "No help message set.";

		public abstract void Invoke(params object[] parameters);

		protected AbstractCommand(int paramsCount)
		{
			ParametersCount = paramsCount;
			Aliases         = new List<string>();
		}

		protected static bool IsValidCast<TType>(object parameter)
		{
			string newTypeName = typeof(TType).Name;

			if (typeof(string).IsAssignableFrom(typeof(TType)))
			{
				return true; // Everything can be converted to a string using object.ToString()
			}

			try
			{
				if (typeof(Component).IsAssignableFrom(typeof(TType)))
				{
					newTypeName = nameof(GameObject);

					// Convert to check if it will throw an error
					GameObject selectedObject = parameter.ConvertTo<GameObject>();

					newTypeName = typeof(TType).Name;

					if (!selectedObject.TryGetComponent(out TType component))
					{
						ConsoleManager.LogError($"{parameter} does not have component {newTypeName}!");
						return false;
					}

					return true;
				}

				// Convert to check if it will throw an error
				parameter.ConvertTo<TType>();

				return true;
			}
			catch
			{
				ConsoleManager.LogError(
					$"{parameter} ({parameter.GetType().Name}) can not be converted to type {newTypeName}!");
				return false;
			}
		}

		protected static TNewType ConvertTo<TNewType>(object parameter)
		{
			if (typeof(string).IsAssignableFrom(typeof(TNewType)))
			{
				return parameter.ToString().ConvertTo<TNewType>();
			}
			
			if (typeof(Component).IsAssignableFrom(typeof(TNewType)))
			{
				GameObject gameobject = (GameObject)parameter;

				if (!gameobject.TryGetComponent(out TNewType component))
				{
					ConsoleManager.LogError($"{parameter} does not have component {typeof(TNewType).Name}!");
				}

				return component;
			}

			return parameter.ConvertTo<TNewType>();
		}

		public void SetName(string name)
		{
			Name = name;
		}

		public string GetName()
		{
			return Name;
		}

		public List<string> GetAllNames()
		{
			List<string> names = new List<string>() { Name };

			Aliases.ForEach(names.Add);

			return names;
		}

		/// <summary>
		/// Returns the name, plus all the parameter types
		/// </summary>
		public abstract string GetFullName();

		public bool HasName(string name)
		{
			return Name == name || Aliases.Contains(name);
		}

		public bool HasName(IEnumerable<string> names)
		{
			if (names.Any(HasName))
			{
				return true;
			}

			return false;
		}

		public void AddAlias(string alias)
		{
			if (Aliases.Contains(alias))
			{
				return;
			}

			Aliases.Add(alias);
		}

		public void RemoveAlias(string name)
		{
			Aliases.Remove(name);
		}

		public void SetHelpMessage(string message)
		{
			HelpMessage = message;
		}

		public string GetHelpMessage()
		{
			return HelpMessage;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(ConsoleManager.Instance.prefix);

			stringBuilder.Append(GetFullName());
			stringBuilder.Append(": ");
			stringBuilder.AppendLine(HelpMessage);

			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Aliases: ");

			if (Aliases.Count == 0)
			{
				stringBuilder.AppendLine("None");
			}

			foreach (string alias in Aliases)
			{
				stringBuilder.AppendLine(alias);
			}

			return stringBuilder.ToString();

			// {Name}: {help}

			// Aliases:
			// Alias1
			// Alias2
			// etc.
		}
	}
}