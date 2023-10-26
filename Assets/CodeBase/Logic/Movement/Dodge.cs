using System;
using CodeBase.Logic.StaminaEntity;
using CodeBase.Services.Pause;
using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(TimerOperator))]
    public class Dodge : MonoCache, IDodge, IPauseWatcher
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private GameObject _hitBox;
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

        public event Action<MoveDirection> Dodged;

        public bool CanDodge => enabled && _isDodged == false && _stamina.TrySpend(_fatigue);

        private void Awake() =>
            enabled = true;

        private void Start() =>
            _timer.SetUp(_invulnerabilityTime, OnTurnOff);

        public void Evade(MoveDirection direction)
        {
            _hitBox.SetActive(false);
            Dodged?.Invoke(direction);
            _collider.height = _rollCollideHeight;
            _collider.center = _collider.center.ChangeY(_rollCollideCenter);
            _timer.Restart();
            _timer.Play();
        }

        private void OnTurnOff()
        {
            _collider.height = _normalCollideHeight;
            _collider.center = _collider.center.ChangeY(_normalCollideCenter);
            _hitBox.SetActive(true);
        }

        public void Pause() =>
            enabled = false;

        public void Resume() =>
            enabled = true;
    }
}