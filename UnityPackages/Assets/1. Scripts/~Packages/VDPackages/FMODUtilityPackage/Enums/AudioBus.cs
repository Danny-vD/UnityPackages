namespace VDPackages.FMODUtilityPackage.Enums
{
	/// <summary>
	/// A enum representation of the FMOD busses (value 0 is considered the master bus)<br/>
	/// To help automatic path mapping, try to give the values the same name as the groups defined in FMOD<br/><br/>
	/// You can set the path manually in the <see cref="FMODUtilityPackage.Core.AudioManager"/> inspector
	/// </summary>
	public enum AudioBus // Value 0 is considered the Master bus
	{
		Master = 0, // Master bus, do not delete!
		Ambient,
		Music,
		SFX,
	}
}