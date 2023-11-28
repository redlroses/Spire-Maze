using System;
using CodeBase.Services.Pause;
using CodeBase.StaticData;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.StaminaEntity
{
    [RequireComponent(typeof(TimerOperator))]
    public class Stamina : MonoCache, IStamina, IPauseWatcher
    {
        [SerializeField] protected TimerOperator _normalDelay;

        private float _lowerSpeedMultiplier;
        private float _speedReplenish;
        private float _delayBeforeReplenish;
        private float _currentSpeedReplenish;
        private int _currentPoints;
        private bool _isReplenish;

        public event Action Changed = () => { };

        public int CurrentPoints
        {
            get => _currentPoints;
            protected set
            {
                _currentPoints = value;
                Changed.Invoke();
                Debug.Log($"Stamina: {_currentPoints}");
            }
        }

        public int MaxPoints { get; protected set; }

        protected override void Run() =>
            Replenish();

        public void Construct(StaminaStaticData staminaStaticData)
        {
            _lowerSpeedMultiplier = staminaStaticData.LowerSpeedMultiplier;
            _speedReplenish = staminaStaticData.SpeedReplenish;
            _delayBeforeReplenish = staminaStaticData.DelayBeforeReplenish;
            Initialize();
        }

        public bool TrySpend(int spendStamina)
        {
            if (spendStamina <= 0 || CurrentPoints <= 0)
                return false;

            Spend(spendStamina);
            return true;
        }

        private void Initialize()
        {
            _normalDelay ??= Get<TimerOperator>();
            _currentSpeedReplenish = _speedReplenish;
            _normalDelay.SetUp(_delayBeforeReplenish, OnStartReplenish);
        }

        private void Spend(int spendStamina)
        {
            _isReplenish = false;
            CurrentPoints -= spendStamina;

            if (CurrentPoints < 0)
                _currentSpeedReplenish = _speedReplenish * _lowerSpeedMultiplier;

            _normalDelay.Restart();
            _normalDelay.Play();
        }

        private void Replenish()
        {
            if (_isReplenish == false)
                return;

            int newPoints = CurrentPoints + Mathf.RoundToInt(_currentSpeedReplenish * Time.deltaTime);
            CurrentPoints = newPoints > MaxPoints ? MaxPoints : newPoints;

            if (CurrentPoints < MaxPoints)
                return;

            _isReplenish = false;
            _currentSpeedReplenish = _speedReplenish;
        }

        private void OnStartReplenish() =>
            _isReplenish = true;

        public void Resume() =>
            enabled = true;

        public void Pause() =>
            enabled = false;
    }
}