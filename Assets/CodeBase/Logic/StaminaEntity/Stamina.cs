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
        [SerializeField] private TimerOperator _recoveryDelay;

        private float _lowerSpeedMultiplier;
        private float _speedReplenish;
        private float _delayBeforeReplenish;
        private float _currentSpeedReplenish;
        private bool _isReplenish;

        public event Action Changed = () => { };

        public event Action AttemptToEmptyUsed = () => { };

        public int CurrentPoints { get; protected set; }

        public int MaxPoints { get; protected set; }

        public void Construct(StaminaStaticData staminaStaticData)
        {
            _lowerSpeedMultiplier = staminaStaticData.LowerSpeedMultiplier;
            _speedReplenish = staminaStaticData.SpeedReplenish;
            _delayBeforeReplenish = staminaStaticData.DelayBeforeReplenish;
            _recoveryDelay ??= GetComponent<TimerOperator>();
            _currentSpeedReplenish = _speedReplenish;
            _recoveryDelay.SetUp(_delayBeforeReplenish, OnStartReplenish);
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

        protected override void Run() =>
            Replenish();

        private void Spend(int spendStamina)
        {
            _isReplenish = false;
            int newPoints = CurrentPoints - spendStamina;

            if (newPoints < 0)
                _currentSpeedReplenish = _speedReplenish * _lowerSpeedMultiplier;

            newPoints = Mathf.Max(newPoints, 0);
            SetCurrentPointsReactive(newPoints);
            Changed.Invoke();

            _recoveryDelay.Restart();
            _recoveryDelay.Play();
        }

        private void Replenish()
        {
            if (_isReplenish == false)
                return;

            int newPoints = CurrentPoints + Mathf.RoundToInt(_currentSpeedReplenish * Time.deltaTime);

            newPoints = newPoints > MaxPoints ? MaxPoints : newPoints;
            SetCurrentPointsReactive(newPoints);
            Changed.Invoke();

            if (CurrentPoints < MaxPoints)
                return;

            _isReplenish = false;
            _currentSpeedReplenish = _speedReplenish;
        }

        private void SetCurrentPointsReactive(int currentPoints)
        {
            CurrentPoints = currentPoints;
            Changed.Invoke();
        }

        private void OnStartReplenish() =>
            _isReplenish = true;
    }
}