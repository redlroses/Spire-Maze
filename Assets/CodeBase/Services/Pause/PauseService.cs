using CodeBase.Infrastructure.Factory;

namespace CodeBase.Services.Pause
{
    public class PauseService : IPauseService
    {
        private readonly IGameFactory _gameFactory;

        public PauseService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public bool IsPause { get; private set; }

        public void SetPause(bool isPause)
        {
            if (IsPause == isPause)
            {
                return;
            }

            IsPause = isPause;

            UpdatePauseWatchers();
        }

        private void UpdatePauseWatchers()
        {
            if (IsPause)
            {
                foreach (var watcher in _gameFactory.PauseWatchers)
                {
                    watcher.Pause();
                }
            }
            else
            {
                foreach (var watcher in _gameFactory.PauseWatchers)
                {
                    watcher.Unpause();
                }
            }
        }
    }
}