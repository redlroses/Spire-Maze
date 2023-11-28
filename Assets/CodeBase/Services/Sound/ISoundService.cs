namespace CodeBase.Services.Sound
{
    public interface ISoundService : IService
    {
        void Load();
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
        void Mute(bool isSmooth = false, object locker = null);
        void Unmute(bool isSmooth = false, object locker = null);
        float SfxVolumeLoaded { get; }
        float MusicVolumeLoaded { get; }
    }
}