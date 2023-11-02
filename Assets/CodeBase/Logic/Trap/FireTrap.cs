using CodeBase.Logic.HealthEntity;
using CodeBase.Services.Pause;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(TimerOperator))]
    public class FireTrap : Trap, IPauseWatcher
    {
        [SerializeField]
        [RequireInterface(typeof(IDamageTrigger))] private MonoBehaviour _damageTrigger;
        [SerializeField] private ParticleSystem[] _effects;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _turnOffDelay;

        private bool _isActivated;

        private IDamageTrigger DamageTrigger => (IDamageTrigger) _damageTrigger;

        private void Awake() =>
            _timer.SetUp(_turnOffDelay, OnTurnOff);

        public void Resume()
        {
            if (_isActivated)
                PlayEffects();
        }

        public void Pause()
        {
            if (_isActivated)
                StopEffects();
        }

        private void OnTurnOff()
        {
            DamageTrigger.Disable();
            StopEffects();
            _isActivated = false;
        }

        protected override void Activate()
        {
            if (_isActivated)
                return;

            _timer.Restart();
            _timer.Play();
            PlayEffects();
            DamageTrigger.Enable();
            _isActivated = true;
        }

        private void PlayEffects()
        {
            foreach (var effect in _effects)
            {
                effect.Play();
            }
        }

        private void StopEffects()
        {
            foreach (var effect in _effects)
            {
                effect.Stop();
            }
        }
    }
}