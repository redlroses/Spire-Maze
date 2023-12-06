using System;
using CodeBase.Logic.HealthEntity.Damage;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    public class Health : MonoBehaviour, IHealth
    {
        private int _currentPoints;

        public event Action Died = () => { };
        public event Action Changed = () => { };

        public int CurrentPoints
        {
            get => _currentPoints;
            protected set
            {
                _currentPoints = value;
                Changed.Invoke();
            }
        }

        public int MaxPoints { get; protected set; }
        public bool IsAlive => _currentPoints > 0;

        public void Damage(int damagePoints, DamageType damageType)
        {
            if (IsAlive == false && CanDamage(damageType) == false)
                return;

            Validate(damagePoints);
            int newPoints = CurrentPoints - damagePoints;

            if (damageType == DamageType.Lethal)
                newPoints = 0;

            if (newPoints <= 0)
                Died.Invoke();

            int deltaPoints = CurrentPoints - newPoints;
            CurrentPoints = Mathf.Max(newPoints, 0);
            OnDamaged(deltaPoints, damageType);

            Debug.Log($"Damaged: {deltaPoints}, current health: {CurrentPoints}");
        }

        protected virtual void OnDamaged(int deltaPoints, DamageType damageType) { }

        protected virtual bool CanDamage(DamageType damageType) =>
            true;

        private void Validate(int points)
        {
            if (points <= 0)
                throw new ArgumentOutOfRangeException(nameof(points), "Points must be non negative");
        }
    }
}