using System.Collections.Generic;
using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public sealed class ProgressUpdater
    {
        private readonly List<ISavedProgressReader> _progressReaders = new List<ISavedProgressReader>();
        private readonly List<ISavedProgress> _progressWriters = new List<ISavedProgress>();

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                _progressWriters.Add(progressWriter);

            _progressReaders.Add(progressReader);
        }

        public void Update(PlayerProgress progress)
        {
            foreach (ISavedProgress progressWriter in _progressWriters)
                progressWriter.UpdateProgress(progress);
        }

        public void InformReaders(PlayerProgress progress)
        {
            foreach (ISavedProgressReader progressReader in _progressReaders)
                progressReader.LoadProgress(progress);
        }

        public void Cleanup()
        {
            _progressReaders.Clear();
            _progressWriters.Clear();
        }
    }
}