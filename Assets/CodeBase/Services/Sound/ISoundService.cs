namespace CodeBase.Services.Sound
{
    public interface ISoundService : IService
    {
        void Load();
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
        void Mute(bool isSmooth = false);
        void Unmute(bool isSmooth = false);
        float SfxVolume { get; }
        float MusicVolume { get; }
    }
}