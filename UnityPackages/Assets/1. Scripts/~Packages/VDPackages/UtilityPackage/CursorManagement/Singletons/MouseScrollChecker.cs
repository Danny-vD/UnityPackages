using System;
using UnityEngine;
using VDFramework.Singleton;
using VDFramework.Timer;
using VDFramework.Timer.TimerHandles;
using VDPackages.UtilityPackage.CursorManagement.CursorUtility;

namespace VDPackages.UtilityPackage.CursorManagement.Singletons
{
	[DefaultExecutionOrder(-5)]
	public class MouseScrollChecker : Singleton<MouseScrollChecker>
	{
		public static event Action OnStartedScrolling = delegate { };
		public static event Action OnStoppedScrolling = delegate { };

		/// <summary>
		/// Getting a raw scroll value can be sensitive (hard/impossible to scroll every frame) so use a minimum time so other scripts can properly react to scrolling
		/// </summary>
		[SerializeField, Tooltip("The minimum time in seconds after scrolling started that we should consider it to still be scrolling")]
		private float minimumScrollTime = 0.3f;

		public bool IsScrolling { get; private set; }

		private TimerHandle stopScrollingTimer;

		private void Update()
		{
			if (MouseButtonUtil.IsScrolling)
			{
				if (stopScrollingTimer == null)
				{
					StartScrolling();
				}
				else
				{
					stopScrollingTimer.ResetTimer();
				}
			}
		}

		private void StartScrolling()
		{
			IsScrolling        = true;
			stopScrollingTimer = TimerManager.StartNewTimer(minimumScrollTime, StopScrolling);
			
			OnStartedScrolling.Invoke();
		}

		private void StopScrolling()
		{
			IsScrolling        = false;
			stopScrollingTimer = null;
			
			OnStoppedScrolling.Invoke();
		}
	}
}