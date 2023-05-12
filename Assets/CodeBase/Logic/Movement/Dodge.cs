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
        private readonly LayerMask _player = 10;
        private readonly LayerMask _trapActivator = 11;

        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _slidingTime;
        [SerializeField] private GameObject _hitBox;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private int _fatigue;

        private bool _isDodged;
        private IPlayerInputService _inputService;

        public event Action<MoveDirection> Dodged;

        public bool CanDodge => (_isDodged == false) & _stamina.TrySpend(_fatigue);

        private void Start() =>
            _timer.SetUp(_slidingTime, OnTurnOff);

        public void Evade(MoveDirection direction)
        {
            _isDodged = true;
            _hitBox.SetActive(false);
            Dodged?.Invoke(direction);
            _timer.Restart();
            _timer.Play();
        }

        private void OnTurnOff()
        {
            _isDodged = false;
            _hitBox.SetActive(true);
        }
    }
}