using System.Collections;
using CodeBase.Data;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class HeroMover : Mover, IPlateMovable, ISavedProgress
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private Dodge _dodge;

        private Coroutine _inputDelay;

        protected override void OnEnabled()
        {
            _dodge.Dodged += OnDodged;
        }

        protected override void OnDisabled()
        {
            _dodge.Dodged -= OnDodged;
        }

        protected override void Run()
        {
            _heroAnimator.SetSpeed(Rigidbody.velocity.RemoveY().magnitude);
            _heroAnimator.SetFallSpeed(Mathf.Abs(Rigidbody.velocity.y));
        }

        public void OnMovingPlatformEnter(IPlateMover plateMover)
        {
            plateMover.PositionUpdated += OnPlateMoverPositionUpdated;
        }

        public void OnMovingPlatformExit(IPlateMover plateMover)
        {
            plateMover.PositionUpdated -= OnPlateMoverPositionUpdated;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            Rigidbody.position = progress.WorldData.PositionOnLevel.Position.AsUnityVector();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel.Position = Rigidbody.position.AsVectorData();
        }

        private void OnPlateMoverPositionUpdated(Vector3 deltaPosition, Vector3 deltaRotation)
        {
            Vector3 uncorrectedPosition = Rigidbody.position + deltaPosition;
            Rigidbody.position = (uncorrectedPosition.RemoveY().normalized * Spire.DistanceToCenter)
                .AddY(uncorrectedPosition.y);
            Rigidbody.rotation = Quaternion.Euler(Rigidbody.rotation.eulerAngles + deltaRotation);
        }

        private void OnDodged(MoveDirection direction)
        {
            Move(direction);
        }
    }
}