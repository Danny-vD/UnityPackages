namespace FMODUtilityPackage.Interfaces
{
	/// <summary>
	/// An interface that provides basic functions to Play, Pause and Stop audio.
	/// </summary>
	public interface IAudioplayer
	{
		/// <summary>
		/// Play the audio
		/// </summary>
		void Play();

		/// <summary>
		/// Play the audio if the audio is not already playing
		/// </summary>
		void PlayIfNotPlaying();

		/// <summary>
		/// Stop playing the audio
		/// </summary>
		void Stop();

		/// <summary>
		/// Set the pause state of the audio
		/// </summary>
		void SetPause(bool paused);
	}
}