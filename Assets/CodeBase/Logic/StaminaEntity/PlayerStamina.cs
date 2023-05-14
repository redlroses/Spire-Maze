using CodeBase.Data;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Logic.StaminaEntity
{
    public sealed class PlayerStamina : Stamina, ISavedProgress
    {
        public void LoadProgress(PlayerProgress progress)
        {
            MaxPoints = progress.WorldData.HeroStaminaState.MaxValue;
            CurrentPoints = progress.WorldData.HeroStaminaState.CurrentValue;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.HeroStaminaState.CurrentValue = CurrentPoints;
            progress.WorldData.HeroStaminaState.MaxValue = MaxPoints;
        }
    }
}