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

        [SerializeField] protected TimerOperator TimerDelay;

        [SerializeField] private float _speedReplenish;
        [SerializeField] private float _delayBeforeReplenish = 1.5f;

        private int _currentPoints;
        private float _currentSpeedReplenish;
        private bool _isSpent;
        private bool _isReplenish;
        private IPauseReactive _pauseReactive;

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

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }

        protected override void Run()
        {
            Replenish();
        }

        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
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
        
        private void OnResume()
        {
            enabled = true;
        }

        private void OnPause()
        {
            enabled = false;
        }
    }
}