using System;

namespace GridPackage.Structs
{
	[Serializable]
	public struct TileProperties
	{
		/// <summary>
		/// The default value of the properties
		/// </summary>
		public static TileProperties Default = new TileProperties()
		{
			Walkable = true,
		};

		// Example property, it is not used anywhere by default
		public bool Walkable;
	}
}