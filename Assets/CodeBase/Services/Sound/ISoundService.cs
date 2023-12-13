using CodeBase.Tools;

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
        void Mute(bool isSmooth = false, Locker locker = null);
        void Unmute(bool isSmooth = false, Locker locker = null);
    }
}