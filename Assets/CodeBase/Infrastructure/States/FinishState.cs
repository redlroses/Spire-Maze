using System.Linq;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Ranked;
using CodeBase.Services.Score;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.States
{
    public class FinishState : IPayloadedState<bool>
    {
        private readonly IWindowService _windowService;
        private readonly IScoreService _scoreService;
        private readonly IRankedService _rankedService;
        private readonly IPersistentProgressService _progressService;

        public FinishState(IWindowService windowService, IScoreService scoreService, IRankedService rankedService,
            IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _rankedService = rankedService;
            _windowService = windowService;
            _scoreService = scoreService;
        }

        public void Enter(bool isLose)
        {
            _scoreService.Calculate(isLose);
            _scoreService.UpdateProgress();

            int fullScore = _progressService.Progress.GlobalData.Levels.Sum(level => level.Score);
            _rankedService.SetScore(fullScore);

            _windowService.Open(isLose ? WindowId.Lose : WindowId.Results);
        }

        public void Exit() { }
    }
}