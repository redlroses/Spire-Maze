using CodeBase.Data;
using CodeBase.Logic.Lift.PlateMove;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class PlayerMover : Mover, IPlateMovable
    {
        public void OnMovingPlatformEnter(IPlateMover plateMover)
        {
            plateMover.PositionUpdated += OnPlateMoverPositionUpdated;
            Debug.Log("OnMovingPlatformEnter");
        }

        public void OnMovingPlatformExit(IPlateMover plateMover)
        {
            plateMover.PositionUpdated -= OnPlateMoverPositionUpdated;
            Debug.Log("OnMovingPlatformExit");
        }

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