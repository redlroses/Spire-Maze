using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    public sealed class PlayerHealth : Health, IHealable, IImmune, ISavedProgress
    {
        public event Action<int, DamageType> Damaged;
        public event Action<int> Healed;

        public bool IsImmune { get; set; }

        protected override void OnDamaged(int deltaPoints, DamageType damageType)
        {
            Damaged?.Invoke(deltaPoints, damageType);
        }

        public void Heal(int healPoints)
        {
            Validate(healPoints);

            int newPoints = CurrentPoints + healPoints;

            if (newPoints > MaxPoints)
            {
                newPoints = MaxPoints;
            }

            int deltaPoints = newPoints - CurrentPoints;
            Healed?.Invoke(deltaPoints);
            CurrentPoints = newPoints;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            MaxPoints = progress.HeroHealthState.MaxHP;
            CurrentPoints = progress.HeroHealthState.CurrentHP;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroHealthState.CurrentHP = CurrentPoints;
            progress.HeroHealthState.MaxHP = MaxPoints;
        }

        private void Validate(int points)
        {
            if (IsAlive == false)
            {
                throw new InvalidOperationException($"Entity {name} already dead");
            }

            if (points <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points), "Points must be non negative");
            }
        }
    }
}