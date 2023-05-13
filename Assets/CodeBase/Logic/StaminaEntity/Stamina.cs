using System;
using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.StaminaEntity
{
    [RequireComponent(typeof(TimerOperator))]
    public class Stamina : MonoCache, IStamina, IPauseWatcher
    {
        [SerializeField] protected TimerOperator _timerDelay;
        
        protected float LowerSpeedMultiplier;
        protected float SpeedReplenish;
        protected float DelayBeforeReplenish;

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

        protected override void Run()
        {
            Replenish();
        }

        public bool TrySpend(int spendStamina)
        {
            if (spendStamina <= 0 || CurrentPoints == 0)
            {
                return false;
            }

            Spend(spendStamina);
            return true;
        }

        protected void Initialize()
        {
            _timerDelay ??= Get<TimerOperator>();
            _currentSpeedReplenish = SpeedReplenish;
            _timerDelay.SetUp(DelayBeforeReplenish, OnStartReplenish);
        }

        private void Spend(int spendStamina)
        {
            _isReplenish = false;
            
            int newPoints = CurrentPoints - spendStamina;
            CurrentPoints = newPoints < 0 ? 0 : newPoints;

            if (_isSpent == false)
            {
                _isSpent = CurrentPoints == 0;
            }

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
                _currentSpeedReplenish = SpeedReplenish;
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