using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
        public TemporalProgress TemporalProgress { get; set; }
    }
}