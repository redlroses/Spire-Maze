namespace CodeBase.Services.Sound
{
    public interface ISoundService : IService
    {
        float SfxDefaultVolume { get; }
        float MusicDefaultVolume { get; }
        bool IsMuted { get; }
        void Load();
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
        void Mute(bool isSmooth = false, SoundLocker locker = null);
        void Unmute(bool isSmooth = false, SoundLocker locker = null);
    }
}