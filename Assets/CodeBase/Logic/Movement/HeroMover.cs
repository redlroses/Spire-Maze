using CodeBase.Data;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class HeroMover : Mover, IMovableByPlate, ISavedProgress
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private Dodge _dodge;
        [SerializeField] private float _dodgeSpeedFactor = 1.5f;

        private Vector3 _groundMovingDeltaPosition;
        private Vector3 _groundMovingDeltaRotation;

        public void OnMovingPlatformEnter(IPlateMover plateMover) =>
            plateMover.PositionUpdated += OnPlateMoverPositionUpdated;

        public void OnMovingPlatformExit(IPlateMover plateMover) =>
            plateMover.PositionUpdated -= OnPlateMoverPositionUpdated;

        public void LoadProgress(PlayerProgress progress) =>
            Rigidbody.position = progress.WorldData.LevelPositions.InitialPosition.AsUnityVector();

        public void UpdateProgress(PlayerProgress progress) =>
            progress.WorldData.LevelPositions.InitialPosition = Rigidbody.position.AsVectorData();

        protected override void OnLateMove()
        {
            _heroAnimator.SetSpeed(Rigidbody.velocity.RemoveY().magnitude);
            _heroAnimator.SetFallSpeed(Mathf.Abs(Rigidbody.velocity.y));

            if (_groundMovingDeltaPosition.Equals(Vector3.zero))
                return;

            Rigidbody.velocity += _groundMovingDeltaPosition.ChangeY(0) / Time.fixedDeltaTime;
            Rigidbody.rotation = Quaternion.Euler(Rigidbody.rotation.eulerAngles + _groundMovingDeltaRotation);

            _groundMovingDeltaPosition = Vector3.zero;
            _groundMovingDeltaRotation = Vector3.zero;
        }

        protected override void OnEnabled() =>
            _dodge.Dodged += OnDodged;

        protected override void OnDisabled() =>
            _dodge.Dodged -= OnDodged;

        private void OnPlateMoverPositionUpdated(Vector3 deltaPosition, Vector3 deltaRotation)
        {
            _groundMovingDeltaPosition = deltaPosition;
            _groundMovingDeltaRotation = deltaRotation;
        }

        private void OnDodged(MoveDirection direction) =>
            Move(direction, _dodgeSpeedFactor);
    }
}