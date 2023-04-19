namespace CodeBase.Services.Sound
{
    public interface ISoundService : IService
    {
        void MusicVolume(float volume);
        void SoundVolume(float volume);
    }
}