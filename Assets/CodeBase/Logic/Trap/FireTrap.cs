using UnityEngine;

namespace CodeBase.Logic.Trap
{
    [RequireComponent(typeof(TimerOperator))]
    public class FireTrap : Trap
    {
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private float _delayBetweenDamage;
        [SerializeField] private float _rayDistance;
        [SerializeField] private TimerOperator _timer;

        private Transform _selfTransform;
        private IDamagable _target;
        private bool _isActavated;

        private void Awake()
        {
            _selfTransform = transform;
            _timer ??= Get<TimerOperator>();
            _timer.SetUp(_delayBetweenDamage, TakeDamage);
        }

        protected override void Run()
        {
            DetectTarget();
        }

        protected override void Activate()
        {
            _effect.Play();
            _isActavated = true;
        }

        private void DetectTarget()
        {
            if (_isActavated == false)
            {
                return;
            }

            Ray ray = new Ray(_selfTransform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance))
            {
                if (hit.collider.TryGetComponent(out IDamagable target) == false)
                {
                    return;
                }

                _timer.Play();
                _target = target;
            }
        }

        private void TakeDamage()
        {
            if (_target == null)
                return;

            _target.ReceiveDamage(Damage);
            _timer.Restart();
        }
    }
}