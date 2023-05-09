using System;
using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.StaminaEntity
{
    [RequireComponent(typeof(TimerOperator))]
    public class Stamina : MonoCache, IStamina, IPauseWatcher
    {
        private const float LowerSpeedMultiplier = 0.5f;
        private const float MinimumPercent = 0.1f;

        [SerializeField] protected TimerOperator _timerDelay;
        [SerializeField] private float _speedReplenish;
        [SerializeField] private float _delayBeforeReplenish = 1.5f;

        private int _currentPoints;
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
            _timerDelay ??= Get<TimerOperator>();
            _currentSpeedReplenish = _speedReplenish;
            _timerDelay.SetUp(_delayBeforeReplenish, OnStartReplenish);
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

            _isSpent = CurrentPoints <= MaxPoints * MinimumPercent;
            _timerDelay.Restart();
            _timerDelay.Play();
        }

        private void Replenish()
        {
            if (_isReplenish == false)
            {
                return;
            }

            int newPoints = _isSpent
                ? CurrentPoints + Mathf.RoundToInt(_currentSpeedReplenish * LowerSpeedMultiplier * Time.deltaTime)
                : CurrentPoints + Mathf.RoundToInt(_currentSpeedReplenish * Time.deltaTime);
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

        public void Resume() =>
            enabled = true;

        public void Pause() =>
            enabled = false;
    }
}