using CodeBase.Services.SaveLoad;
using CodeBase.Services.Score;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.States
{
    public class FinishState : IState
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IWindowService _windowService;
        private readonly IScoreService _scoreService;

        public FinishState(ISaveLoadService saveLoadService,
            IWindowService windowService, IScoreService scoreService)
        {
            _saveLoadService = saveLoadService;
            _windowService = windowService;
            _scoreService = scoreService;
        }

        public void Enter()
        {
            _scoreService.CalculateScore();
            _saveLoadService.SaveProgress();
            _windowService.Open(WindowId.Results);
        }

        public void Exit()
        {
        }
    }
}