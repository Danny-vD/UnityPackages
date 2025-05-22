using UnityEngine;
using VDFramework;
using VDFramework.Logger;
using VDFramework.Logger.Implementations;

namespace UtilityPackage.Utility.LoggerUtil
{
	/// <summary>
	/// Simple class to set the <see cref="LogManager.LoggerImplementation"/> in the <see cref="LogManager"/><br/>
	/// Necessary because the LogManager is environment agnostic so it cannot default to Unity's logger
	/// </summary>
	[DefaultExecutionOrder(-1000)] // Since it sets the logger, ensure it always happens first
	public class LoggerImplementationSetter : BetterMonoBehaviour
	{
		private void Awake()
		{
			LogManager.LoggerImplementation = new DebugLogger();
			
			Destroy(this);
		}
	}
}