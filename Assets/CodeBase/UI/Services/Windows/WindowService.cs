using System;
using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
  public class WindowService : IWindowService
  {
    private readonly IUIFactory _uiFactory;

    public WindowService(IUIFactory uiFactory)
    {
      _uiFactory = uiFactory;
    }

    public void Open(WindowId windowId)
    {
      switch (windowId)
      {
        case WindowId.None:
          break;
        case WindowId.Leaderboard:
          _uiFactory.CreateLeaderboard();
          break;
        case WindowId.Settings:
          _uiFactory.CreateSettings();
          break;
        case WindowId.Pause:
          _uiFactory.CreatePause();
          break;
        case WindowId.Results:
          _uiFactory.CreateResults();
          break;
        case WindowId.Lose:
          _uiFactory.CreateLose();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
      }
    }
  }
}