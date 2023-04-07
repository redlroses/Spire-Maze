using CodeBase.Data;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Logic.StaminaEntity
{
    public sealed class PlayerStamina : Stamina, ISavedProgress
    {
        public void LoadProgress(PlayerProgress progress)
        {
            MaxPoints = progress.HeroStaminaState.MaxValue;
            CurrentPoints = progress.HeroStaminaState.CurrentValue;
            TimerDelay.Restart();
            TimerDelay.Play();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroStaminaState.CurrentValue = CurrentPoints;
            progress.HeroStaminaState.MaxValue = MaxPoints;
        }
    }
}