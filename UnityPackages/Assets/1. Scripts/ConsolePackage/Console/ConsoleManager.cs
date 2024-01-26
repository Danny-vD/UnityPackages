using System;
using ConsolePackage.ObjectSelection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VDFramework.Singleton;

namespace ConsolePackage.Console
{
	public class ConsoleManager : Singleton<ConsoleManager>
	{
		[Header("Components"), SerializeField]
		private InputField inputField = null;

		[SerializeField]
		private Text text = null;

		[SerializeField]
		private GameObject console = null;

		[SerializeField]
		private GameObject selectedObjectWindow = null;

		[Header("Command properties"), Space(20), Tooltip("Symbol(s) that must precede all commands")]
		public string prefix = "";

		[Tooltip("The character to tell the console that your argument is a string")]
		public char stringChar = '"';

		[SerializeField]
		private DefaultCommandAdder defaultCommands;

		[Header("Console properties"), SerializeField, Tooltip("The combination of buttons to press to toggle the console")]
		private ConsoleInputChecker inputChecker;

		[Space, SerializeField, Tooltip("The time (in seconds) before you can toggle the console again")]
		private float toggleCooldown = 0.3f;

		[Space, SerializeField, Range(100, 10000)]
		private int characterLimit = 10000;

		[Space, SerializeField]
		private string normalColorHex = "000000"; // Black

		[SerializeField]
		private string warningColorHex = "D4D422"; // Yellow

		[SerializeField]
		private string errorColorHex = "882222"; // Red

		[SerializeField]
		private string commandColorHex = "FFFFFF"; // White

		[NonSerialized]
		public ObjectSelector ObjectSelector;

		public string HelpCommand => defaultCommands.helpCommands[0];

		public string ClearCommand => defaultCommands.clearCommands.Length > 0 ? defaultCommands.clearCommands[0] : string.Empty;

		private float time = 0;

		private ScrollRect scrollRect;

		private CommandHandler commandHandler;
		private DefaultLogReader logReader;

		private bool submittedCommand = false;

		private void Reset()
		{
			if (FindObjectOfType<EventSystem>() == null)
			{
				new GameObject(nameof(EventSystem)).AddComponent<EventSystem>();
			}
		}

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);

			ObjectSelector = GetComponent<ObjectSelector>();
			commandHandler = new CommandHandler();
			logReader      = new DefaultLogReader();

			scrollRect = console.GetComponentInChildren<ScrollRect>();

			defaultCommands.AddCommands();

			inputField.onEndEdit.AddListener(OnSubmitCommand);

			ObjectSelector.enabled = console.activeSelf;
			selectedObjectWindow.SetActive(console.activeSelf);
		}

		private void OnEnable()
		{
			inputChecker.OnEnable();
		}

		private void OnDisable()
		{
			inputChecker.OnDisable();
		}

		private void Update()
		{
			if (time > 0)
			{
				time -= Time.unscaledDeltaTime;
			}

			if (time <= 0 && inputChecker.OpenConsoleKeysPressed())
			{
				ToggleConsoleActive();
			}
		}

		private void LateUpdate()
		{
			if (submittedCommand)
			{
				ObjectSelector.CheckValid(); // In case the command modified the object
				submittedCommand = false;
			}
		}

		protected override void OnDestroy()
		{
			commandHandler  = null;
			defaultCommands = null;

			logReader.OnDestroy();
			logReader = null;

			base.OnDestroy();
		}

		public static void Clear()
		{
			Instance.text.text = string.Empty;
		}

		public static void EnterCommand(string command)
		{
			Instance.OnSubmitCommand(command);
		}

		private static void Log(object @object, string hex, bool endWithStripe = true)
		{
			string toLog = @object.ToString();
			int characterLimit = Instance.characterLimit;

			if (toLog.Length > characterLimit)
			{
				LogError($"The message you're trying to log is too long! Maximum size is {characterLimit}!");
				return;
			}

			string formattedLog = $"<color=#{hex}>{toLog}</color>\n";

			if (endWithStripe)
			{
				formattedLog += "---------------------------------------------------\n";
			}

			Text console = Instance.text;
			int historyLength = console.text.Length;

			if (historyLength + formattedLog.Length > characterLimit)
			{
				string consoleHistory = console.text + formattedLog;
				int newLength = consoleHistory.Length;

				int earliestAllowedStart = newLength - characterLimit;
				int indexOfLastCommand = consoleHistory.IndexOf("---------------------------------------------------", earliestAllowedStart, StringComparison.Ordinal);

				consoleHistory = consoleHistory.Substring(indexOfLastCommand);
				console.text   = consoleHistory;
			}
			else
			{
				console.text += formattedLog;
			}
			
			Instance.SetScrollbarToBottom();
		}

		public static void Log(object @object, bool logUnityConsole = true)
		{
			if (logUnityConsole)
			{
				UnityEngine.Debug.Log(@object);
				return;
			}

			Log(@object, Instance.normalColorHex);
		}

		public static void LogWarning(object @object, bool logUnityConsole = true)
		{
			if (logUnityConsole)
			{
				UnityEngine.Debug.LogWarning(@object);
				return;
			}

			Log(@object, Instance.warningColorHex);
		}

		public static void LogError(object @object, bool logUnityConsole = true)
		{
			if (logUnityConsole)
			{
				UnityEngine.Debug.LogError(@object);
				return;
			}

			Log(@object, Instance.errorColorHex);
		}

		private static void LogCommand(string command)
		{
			Log(command, Instance.commandColorHex, false);
		}

		private void OnSubmitCommand(string command)
		{
			if (command == string.Empty)
			{
				return;
			}

			LogCommand(command);
			inputField.text = string.Empty;

			commandHandler.OnSubmitCommand(command);

			submittedCommand = true;
		}

		private void ToggleConsoleActive()
		{
			time = toggleCooldown;

			console.SetActive(!console.activeSelf);
			ObjectSelector.enabled = console.activeSelf;
			selectedObjectWindow.SetActive(console.activeSelf);

			SetScrollbarToBottom();
		}

		private void SetScrollbarToBottom()
		{
			if (scrollRect)
			{
				scrollRect.normalizedPosition = Vector2.zero;
			}
		}
	}
}