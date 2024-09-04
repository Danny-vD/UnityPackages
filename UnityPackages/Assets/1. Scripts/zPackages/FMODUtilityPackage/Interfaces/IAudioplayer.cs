namespace FMODUtilityPackage.Interfaces
{
	public interface IAudioplayer
	{
		void Play();

		void PlayIfNotPlaying();

		void Stop();

		void SetPause(bool paused);
	}
}