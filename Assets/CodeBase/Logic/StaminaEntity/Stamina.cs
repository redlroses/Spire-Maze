using System;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.StaminaEntity
{
    [RequireComponent(typeof(TimerOperator))]
    public class Stamina : MonoCache, IStamina
    {
        private const float LowerSpeedMultiplier = 0.3f;
        private const float MinimumPercent = 0.1f;

        [SerializeField] protected TimerOperator TimerDelay;

        [SerializeField] private int _currentPoints;
        [SerializeField] private float _speedReplenish;
        [SerializeField] private float _delayBeforeReplenish = 1.5f;

        private float _currentSpeedReplenish;
        private bool _isSpent;
        private bool _isReplenish;

        public event Action Changed;

        public int CurrentPoints
        {
            get => _currentPoints;
            protected set
            {
                _currentPoints = value;
                Changed?.Invoke();
            }
        }

        public int MaxPoints { get; protected set; }

        private void Awake()
        {
            TimerDelay ??= Get<TimerOperator>();
            _currentSpeedReplenish = _speedReplenish;
            TimerDelay.SetUp(_delayBeforeReplenish, OnStartReplenish);
        }

        protected override void Run()
        {
            Replenish();
        }

        public bool TrySpend(int spendStamina)
        {
            if (spendStamina <= 0 || CurrentPoints - spendStamina < 0)
            {
                return false;
            }

            Spend(spendStamina);
            return true;
        }

        private void Spend(int spendStamina)
        {
            _isReplenish = false;
            CurrentPoints -= spendStamina;

            _isSpent = CurrentPoints <= CurrentPoints * MinimumPercent;
            TimerDelay.Restart();
            TimerDelay.Play();
        }

        private void Replenish()
        {
            if (_isReplenish == false)
            {
                return;
            }

            int newPoints = _isSpent
                ? CurrentPoints + (int)(_currentSpeedReplenish * LowerSpeedMultiplier * Time.deltaTime)
                : CurrentPoints + (int)(_currentSpeedReplenish * Time.deltaTime);
            CurrentPoints = newPoints > MaxPoints ? MaxPoints : newPoints;

            if (CurrentPoints >= MaxPoints)
            {
                _isReplenish = false;
                _isSpent = false;
                _currentSpeedReplenish = _speedReplenish;
            }
        }

        private void OnStartReplenish()
        {
            _isReplenish = true;
        }
    }
}