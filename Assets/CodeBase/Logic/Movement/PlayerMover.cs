using CodeBase.Data;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Logic.Movement;
using UnityEngine;

namespace CodeBase.Logic
{
    public class PlayerMover : Mover, IPlateMovable
    {
        private Vector3 _extraMove;
        private Vector3 _extraRotation;
        private bool _isExtraMove;

        public void OnMovingPlatformEnter(IPlateMover plateMover)
        {
            plateMover.PositionUpdated += OnPlateMoverPositionUpdated;
            _isExtraMove = true;
            Debug.Log("OnMovingPlatformEnter");
        }

        public void OnMovingPlatformExit(IPlateMover plateMover)
        {
            plateMover.PositionUpdated -= OnPlateMoverPositionUpdated;
            _isExtraMove = false;
            Debug.Log("OnMovingPlatformExit");
        }

        // protected override void AddExtraMovement(Rigidbody rigidbody)
        // {
        //     if (_isExtraMove == false)
        //     {
        //         return;
        //     }
        //
        //     Debug.Log("updated");
        //     rigidbody.position += _extraMove;
        //     rigidbody.rotation = Quaternion.Euler(rigidbody.rotation.eulerAngles + _extraRotation);
        // }

        private void OnPlateMoverPositionUpdated(Vector3 deltaPosition, Vector3 deltaRotation)
        {
            Debug.Log($"deltaPosition {deltaPosition}, deltaRotation {deltaRotation}");

            Vector3 uncorrectedPosition = Rigidbody.position + deltaPosition;
            Rigidbody.position = (uncorrectedPosition.RemoveY().normalized * Spire.DistanceToCenter)
                .AddY(uncorrectedPosition.y);
            Rigidbody.rotation = Quaternion.Euler(Rigidbody.rotation.eulerAngles + deltaRotation);
        }
    }
}