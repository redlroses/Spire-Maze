using CodeBase.Data;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Logic.StaminaEntity
{
    public sealed class PlayerStamina : Stamina, ISavedProgress
    {
        public void LoadProgress(PlayerProgress progress)
        {
            LowerSpeedMultiplier = progress.WorldData.HeroStaminaState.LowerSpeedMultiplier;
            MaxPoints = progress.WorldData.HeroStaminaState.MaxValue;
            CurrentPoints = progress.WorldData.HeroStaminaState.CurrentValue;
            SpeedReplenish = progress.WorldData.HeroStaminaState.SpeedReplenish;
            DelayBeforeReplenish = progress.WorldData.HeroStaminaState.DelayBeforeReplenish;
            Initialize();
            _timerDelay.Restart();
            _timerDelay.Play();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.HeroStaminaState.CurrentValue = CurrentPoints;
            progress.WorldData.HeroStaminaState.MaxValue = MaxPoints;
        }
    }
}