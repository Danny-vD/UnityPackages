using UnityEngine;
using VDFramework;

namespace UtilityPackage.Utility.CopyTargetUtils
{
	[DisallowMultipleComponent]
	public class FollowTargetAlongLine : BetterMonoBehaviour
	{
		[SerializeField]
		private GameObject target;

		[SerializeField]
		private Vector3 point1;

		[SerializeField]
		private Vector3 point2;

		[SerializeField]
		private bool clampBetweenPoints = true;

		[Header("OPTIONAL: Use transform position instead of vector")]
		[SerializeField]
		private Transform pointTransform1;

		[SerializeField]
		private Transform pointTransform2;

		[Header("DEBUG")]
		[Tooltip("red: line between points | green: perpendicular to red, towards target position | yellow: perpendicular to green, towards actual target position")]
		[SerializeField]
		private bool drawDebugLines = false;

		private void Update()
		{
			if (pointTransform1)
			{
				point1 = pointTransform1.position;
			}

			if (pointTransform2)
			{
				point2 = pointTransform2.position;
			}

			if (target == null)
			{
				// Set to 50% between point1 and point2 if no target
				CachedTransform.position = Vector3.Lerp(point1, point2, 0.5f);
				return;
			}

			CachedTransform.position = ProjectOntoLine(target.transform.position);
		}

		private Vector3 ProjectOntoLine(Vector3 position)
		{
			Vector3 line = point2 - point1;
			Vector3 directionToPosition = position - point1;

			float dot = Vector3.Dot(directionToPosition, line.normalized); // Project the target onto the line
			dot /= line.magnitude;                                         // Normalize the result

			if (clampBetweenPoints)
			{
				dot = Mathf.Clamp01(dot);
			}

			Vector3 newPosition = point1 + line * dot;

			if (drawDebugLines)
			{
				Vector3 lineToTarget = position - newPosition;
				Vector3 lineNormal = Vector3.Cross(Vector3.up, line.normalized);
				float distanceToPerpendicularTarget = Vector3.Dot(lineToTarget, lineNormal);
				Vector3 perpendicularTargetPosition = newPosition + lineNormal * distanceToPerpendicularTarget;

				Debug.DrawLine(point1, point2, Color.red);                             // Line between points
				Debug.DrawLine(newPosition, perpendicularTargetPosition, Color.green); // Line towards target, perpendicular to red
				Debug.DrawLine(perpendicularTargetPosition, position, Color.yellow);   // Line towards actual target position, perpendicular to green
			}

			return newPosition;
		}
	}
}