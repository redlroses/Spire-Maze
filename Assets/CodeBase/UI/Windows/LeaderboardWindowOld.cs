using CodeBase.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
  public class LeaderboardWindowOld : WindowBase
  {
    public TextMeshProUGUI SkullText;

    public void Construct(IPersistentProgressService progressService)
    {
      base.Construct(progressService);
    }
    
    protected override void Initialize()
    {
    }

    protected override void SubscribeUpdates()
    {
      // Progress.WorldData.LootData.Changed += RefreshSkullText;
    }

    protected override void Cleanup()
    {
      base.Cleanup();
      // Progress.WorldData.LootData.Changed -= RefreshSkullText;
    }
  }
}