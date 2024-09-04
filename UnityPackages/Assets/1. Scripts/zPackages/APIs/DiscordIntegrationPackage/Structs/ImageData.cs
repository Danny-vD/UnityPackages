using System;
using UnityEngine;

namespace APIs.DiscordIntegrationPackage.Structs
{
	[Serializable]
	public struct ImageData
	{
		[SerializeField, Tooltip("The name of the image as defined in the developer portal")]
		private string imageID;

		public string ImageID => imageID.ToLower();
		
		[field: SerializeField, Tooltip("Shown when hovering over the image")]
		public string ImageText { get; private set; }
	}
}