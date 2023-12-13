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
        [SerializeField] protected TimerOperator _recoveryDelay;

        private float _lowerSpeedMultiplier;
        private float _speedReplenish;
        private float _delayBeforeReplenish;
        private float _currentSpeedReplenish;
        private int _currentPoints;
        private bool _isReplenish;

        public event Action Changed = () => { };
        public event Action AttemptToEmptyUsed = () => { };

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

        protected override void Run() =>
            Replenish();

        public void Construct(StaminaStaticData staminaStaticData)
        {
            _lowerSpeedMultiplier = staminaStaticData.LowerSpeedMultiplier;
            _speedReplenish = staminaStaticData.SpeedReplenish;
            _delayBeforeReplenish = staminaStaticData.DelayBeforeReplenish;
            Initialize();
        }

        public void Resume() =>
            enabled = true;

        public void Pause() =>
            enabled = false;

        public bool TrySpend(int spendStamina)
        {
            if (spendStamina <= 0 || CurrentPoints <= 0)
            {
                AttemptToEmptyUsed.Invoke();
                return false;
            }

            Spend(spendStamina);
            return true;
        }

        private void Initialize()
        {
            _recoveryDelay ??= Get<TimerOperator>();
            _currentSpeedReplenish = _speedReplenish;
            _recoveryDelay.SetUp(_delayBeforeReplenish, OnStartReplenish);
        }

        private void Spend(int spendStamina)
        {
            _isReplenish = false;
            int newPoints = CurrentPoints - spendStamina;

            if (newPoints < 0)
                _currentSpeedReplenish = _speedReplenish * _lowerSpeedMultiplier;

            CurrentPoints = Mathf.Max(newPoints, 0);

            _recoveryDelay.Restart();
            _recoveryDelay.Play();
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
    }
}