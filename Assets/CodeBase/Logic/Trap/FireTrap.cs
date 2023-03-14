using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(TimerOperator))]
    public class FireTrap : Trap
    {
        [SerializeField] [RequireInterface(typeof(IDamageTrigger))] private MonoBehaviour _damageTrigger;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _turnOffDelay;

        private bool _isActivated;

        private IDamageTrigger DamageTrigger => (IDamageTrigger) _damageTrigger;

        public override void Construct(TrapActivator activator)
        {
            base.Construct(activator);
            _timer.SetUp(_turnOffDelay, OnTurnOff);
        }

        private void OnTurnOff()
        {
            DamageTrigger.Disable();
            _effect.Stop();
            _isActivated = false;
        }

        protected override void Activate(IDamagable damagable)
        {
            if (_isActivated)
            {
                return;
            }

            _timer.Restart();
            _timer.Play();
            _effect.Play();
            DamageTrigger.Enable();
            _isActivated = true;
        }
    }
}