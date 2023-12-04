using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
        TemporalProgress TemporalProgress { get; set; }

        void Register(ISavedProgressReader progressReader);
        void InformReaders();
        void UpdateWriters();
        void CleanupReadersAndWriters();
    }
}