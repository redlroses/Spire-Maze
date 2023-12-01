using System;
using AYellowpaper;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Pause;
using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(TimerOperator))]
    public class Dodge : MonoCache, IPauseWatcher
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private PlayerStamina _stamina;
        [SerializeField] private float _invulnerabilityTime = 0.7f;
        [SerializeField] private int _fatigue;

        [Space] [Header("Collider settings")]
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private float _normalCollideHeight = 1.8f;
        [SerializeField] private float _normalCollideCenter = 0.98f;
        [SerializeField] private float _rollCollideHeight = 0.8f;
        [SerializeField] private float _rollCollideCenter = 0.45f;

        private bool _isDodged;

        public event Action<MoveDirection> Dodged = _ => { };

        private void Awake() =>
            enabled = true;

        private void Start() =>
            _timer.SetUp(_invulnerabilityTime, OnTurnOff);

        public bool TryDodge(MoveDirection direction)
        {
            if (IsDodgeAvailable() == false)
                return false;

            Dodged.Invoke(direction);
            _collider.height = _rollCollideHeight;
            _collider.center = _collider.center.ChangeY(_rollCollideCenter);
            _timer.Restart();
            _timer.Play();
            return true;
        }

        private void OnTurnOff()
        {
            _collider.height = _normalCollideHeight;
            _collider.center = _collider.center.ChangeY(_normalCollideCenter);
        }

        public void Pause() =>
            enabled = false;

        public void Resume() =>
            enabled = true;

        private bool IsDodgeAvailable() =>
            enabled && _isDodged == false && _stamina.TrySpend(_fatigue);
    }
}