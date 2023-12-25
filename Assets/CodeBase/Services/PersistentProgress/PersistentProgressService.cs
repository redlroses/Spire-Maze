using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        private readonly ProgressUpdater _progressUpdater;

        public PersistentProgressService()
        {
            _progressUpdater = new ProgressUpdater();
        }

        public PlayerProgress Progress { get; set; }

        public TemporalProgress TemporalProgress { get; set; }

        public void Register(ISavedProgressReader progressReader) =>
            _progressUpdater.Register(progressReader);

        public void InformReaders() =>
            _progressUpdater.InformReaders(Progress);

        public void UpdateWriters() =>
            _progressUpdater.Update(Progress);

        public void CleanupReadersAndWriters() =>
            _progressUpdater.Cleanup();
    }
}