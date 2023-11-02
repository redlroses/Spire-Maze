using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class EnemyMover : Mover, IAccelerable
    {
        [SerializeField] private float _bonusSpeed;

        private float _currentBonusSpeed;
        private float _timerBonusSpeed;
        private bool _isBonusSpeedEnabled;

        protected override float CalculateSpeed() =>
            Speed + _currentBonusSpeed;

        public void EnableBonusSpeed()
        {
            if (_isBonusSpeedEnabled)
                return;

            _currentBonusSpeed = _bonusSpeed;
            _isBonusSpeedEnabled = true;
        }

        public void DisableBonusSpeed()
        {
            if (_isBonusSpeedEnabled == false)
                return;

            _currentBonusSpeed = 0;
            _isBonusSpeedEnabled = false;
        }
    }
}