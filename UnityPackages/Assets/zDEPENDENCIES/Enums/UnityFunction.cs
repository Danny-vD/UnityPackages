using System;

namespace Enums
{
	/// <summary>
	/// Represents basic Unity Event Functions
	/// </summary>
	[Flags]
	public enum UnityFunction
	{
		OnEnable = 1 << 1,
		Start = 1 << 2,
		OnDisable = 1 << 3,
		OnDestroy = 1 << 4,
	}
}