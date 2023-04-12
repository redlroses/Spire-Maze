using System;
using CodeBase.Logic.Player;
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

        [SerializeField] private PlayerInput _input;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _slidingTime;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private int _fatigue;

        private bool _isDodged;

        public event Action<MoveDirection> Dodged;

        private void Start()
        {
            _timer.SetUp(_slidingTime, OnTurnOff);
        }

        public void Evade(MoveDirection direction)
        {
            if (_isDodged || _stamina.TrySpend(_fatigue) == false)
            {
                return;
            }

            _isDodged = true;
            Physics.IgnoreLayerCollision(_player, _trapActivator, true);
            Dodged?.Invoke(direction);
            _animator.PlayDodge();
            _timer.Restart();
            _timer.Play();
        }

        private void OnTurnOff()
        {
            _isDodged = false;
            Physics.IgnoreLayerCollision(_player, _trapActivator, false);
        }
    }
}