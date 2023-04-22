using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;

namespace CodeBase.Services.Score
{
    public class ScoreService : IScoreService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;

        public ScoreService(IPersistentProgressService progressService, IStaticDataService staticData)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public int CurrentScore { get; private set; }

        public int CalculateScore()
        {
            //_progressService.Progress.ScoreAccumulationData;
            //_staticData.ForScore;
            //TODO: Подсчёт очков по модификаторам из статик даты
            //CurrentScore = 
            //TODO: Кеширование текущих очков на всякий случай чтобы не пересчитывать каждый раз
            return CurrentScore;
        }
    }
}