using AYellowpaper;
using UnityEngine;
using VDPackages.SerializableDictionaryPackage.SerializableDictionary;
using VDPackages.UtilityPackage.Utility.CodeUtils.Interfaces;

namespace VDPackages.APIs.DiscordIntegrationPackage.RichPresence.Components
{
	public sealed class SetActivityOnEnable : SetActivityComponent
	{
		[SerializeField]
		private SerializableDictionary<InterfaceReference<IWillSoonBeRemoved>, int> test;
		
		[SerializeField]
		private SerializableDictionary<GameObject, int> test2;
		
		private void OnEnable()
		{
			if (!DiscordManager.IsDiscordConnected) // If discord is not connected, delay execution until it is
			{
				DiscordManager.OnDiscordClientReady += UpdatePresence;
			}
			else
			{
				UpdatePresence();
			}
		}

		private void OnDisable()
		{
			DiscordManager.OnDiscordClientReady -= UpdatePresence;
		}
		
		public override void UpdatePresence()
		{
			DiscordManager.OnDiscordClientReady -= UpdatePresence;
			base.UpdatePresence();
		}
	}
}