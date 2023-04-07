using System.Collections.Generic;
using Audio.Core;
using FMOD.Studio;
using Structs.Audio;

namespace ExtentionMethods.FMODUnity
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