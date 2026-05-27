using System.Collections.Generic;
using FMOD.Studio;
using VDPackages.FMODUtilityPackage.Structs;

namespace VDPackages.FMODUtilityPackage.ExtentionMethods
{
	public static class EventInstanceExtensions
	{
		public static void SetParameters(this EventInstance instance, EventParameters parameters)
		{
			foreach (KeyValuePair<string, float> pair in parameters)
			{
				instance.setParameterByName(pair.Key, pair.Value);
			}
		}
	}
}