using UnityEngine;
using UtilityPackage.CursorManagement.CursorUtility;
using VDFramework.Singleton;
using VDFramework.Utility.TimerUtil;
using VDFramework.Utility.TimerUtil.TimerHandles;

namespace UtilityPackage.CursorManagement.Singletons
{
	public class MouseScrollChecker : Singleton<MouseScrollChecker>
	{
		/// <summary>
		/// Getting a raw scroll value can be sensitive (hard/impossible to scroll every frame) so use a minimum time so other scripts can properly react to scrolling
		/// </summary>
		[SerializeField, Tooltip("The minimum time in seconds after scrolling started that we should consider it to still be scrolling")]
		private float minimumScrollTime = 0.3f;

		public bool IsScrolling { get; private set; }

		private TimerHandle timerHandle;

		private void Update()
		{
			if (MouseButtonUtil.IsScrolling)
			{
				if (timerHandle == null)
				{
					StartScrolling();
				}
				else
				{
					timerHandle.ResetTimer();
				}
			}
		}

		private void StartScrolling()
		{
			IsScrolling = true;
			timerHandle = TimerManager.StartNewTimer(minimumScrollTime, StopScrolling);	
		}

		private void StopScrolling()
		{
			IsScrolling = false;
			timerHandle = null;
		}
	}
}