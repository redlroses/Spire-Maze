using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity.Damage
{
    [RequireComponent(typeof(TimerOperator))]
    public class TemporaryDamageImmunity : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(IImmunable))] private MonoBehaviour _health;
        [SerializeField] private TimerOperator _timerOperator;

        [SerializeField] private float _immunityTime;

        private IImmunable Health => (IImmunable) _health;

        private void Awake()
        {
            _timerOperator.SetUp(_immunityTime, OnImmunityEnd);
        }

        private void OnEnable()
        {
            Health.Damaged += OnDamagedHealth;
        }

        private void OnDisable()
        {
            Health.Damaged -= OnDamagedHealth;
            _timerOperator.Pause();
        }

        private void OnDamagedHealth(int damage, DamageType type)
        {
            if (type == DamageType.Periodic)
            {
                return;
            }

            Health.IsImmune = true;
            _timerOperator.Restart();
            _timerOperator.Play();

            Debug.Log($"Immunity enabled");
        }

        private void OnImmunityEnd()
        {
            Health.IsImmune = false;

            Debug.Log($"Immunity disabled");
        }
    }
}