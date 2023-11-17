using System;
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

                if (IsAlive == false)
                {
                    Died.Invoke();
                }
            }
        }

        public int MaxPoints { get; protected set; }
        public bool IsAlive => _currentPoints > 0;

        public void Damage(int damagePoints, DamageType damageType)
        {
            if (IsAlive == false)
            {
                return;
            }

            if (CanDamage() == false)
            {
                return;
            }

            Validate(damagePoints);

            int newPoints = CurrentPoints - damagePoints;

            if (newPoints <= 0)
            {
                Died.Invoke();
                newPoints = 0;
                Debug.Log($"Died!");
            }

            int deltaPoints = CurrentPoints - newPoints;
            CurrentPoints = newPoints;

            OnDamaged(deltaPoints, damageType);

            Debug.Log($"Damaged: {deltaPoints}, current health: {CurrentPoints}");
        }

        protected virtual void OnDamaged(int deltaPoints, DamageType damageType) { }

        protected virtual bool CanDamage() =>
            true;

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