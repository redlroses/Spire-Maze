using System;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(TimerOperator))]
    public class Dodge : MonoCache, IDodge
    {
        [SerializeField] private PlayerInput _input;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _slidingTime;

        private bool _isSliding;
        
        public event Action<MoveDirection> Slided;
        
        private void Start()
        {
           _timer.SetUp(_slidingTime,OnTurnOff);
        }

        public void Evade(MoveDirection direction)
        {
            if(_isSliding)
                return;

            _isSliding = true;
            Slided?.Invoke(direction);
            _animator.PlayDodge();
            _timer.Restart();
            _timer.Play();
        }
        
        private void OnTurnOff()
        {
            _isSliding = false;
        }
    }
}
