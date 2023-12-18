using System;
using CodeBase.Data;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.HealthEntity.Damage;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Logic.Hero
{
    public sealed class HeroHealth : Health, IHealable, IImmune, IHealthReactive, ISavedProgress
    {
        private const string PointsNegativeException = "Points must be non negative";

        public event Action<int, DamageType> Damaged = (_, _) => { };

        public event Action<int> Healed = _ => { };

        public bool IsImmune { get; private set; }

        public void Heal(int healPoints)
        {
            ValidateHeal(healPoints);
            int newPoints = CurrentPoints + healPoints;

            if (newPoints > MaxPoints)
                newPoints = MaxPoints;

            int deltaPoints = newPoints - CurrentPoints;
            Healed.Invoke(deltaPoints);
            CurrentPoints = newPoints;
        }

        public void ActivateImmunity() =>
            IsImmune = true;

        public void DeactivateImmunity() =>
            IsImmune = false;

        public void LoadProgress(PlayerProgress progress)
        {
            MaxPoints = progress.WorldData.HeroHealthState.MaxHP;
            CurrentPoints = progress.WorldData.HeroHealthState.CurrentHP;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.HeroHealthState.CurrentHP = CurrentPoints;
            progress.WorldData.HeroHealthState.MaxHP = MaxPoints;
        }

        protected override void OnDamaged(int deltaPoints, DamageType damageType) =>
            Damaged.Invoke(deltaPoints, damageType);

        protected override bool CanDamage(DamageType damageType) =>
            IsImmune == false || damageType == DamageType.Lethal;

        private void ValidateHeal(int points)
        {
            if (points <= 0)
                throw new ArgumentOutOfRangeException(nameof(points), PointsNegativeException);
        }
    }
}