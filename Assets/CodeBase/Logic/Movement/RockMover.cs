using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class RockMover : Mover
    {
        private Transform _selfTransform;

        private void Awake()
        {
            _selfTransform = transform;
        }
    }
}