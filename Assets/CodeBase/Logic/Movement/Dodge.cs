using System;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Input;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(TimerOperator))]
    public class Dodge : MonoCache, IDodge
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private GameObject _hitBox;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private float _invulnerabilityTime = 0.7f;
        [SerializeField] private int _fatigue;

        private bool _isDodged;
        
        public event Action<MoveDirection> Dodged;

        public bool CanDodge => (_isDodged == false) & _stamina.TrySpend(_fatigue);

        private void Start() =>
            _timer.SetUp(_invulnerabilityTime, OnTurnOff);

        public void Evade(MoveDirection direction)
        {
            _hitBox.SetActive(false);
            Dodged?.Invoke(direction);
            _timer.Restart();
            _timer.Play();
        }

        private void OnTurnOff()
        {
            _hitBox.SetActive(true);
        }
    }
}