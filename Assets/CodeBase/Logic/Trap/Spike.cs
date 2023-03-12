using CodeBase.Logic.Movement;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public class Spike : Trap
    {
        [SerializeField] private SpikeMover _mover;

        private void Awake()
        {
            _mover ??= GetComponent<SpikeMover>();
        }

        protected override void Activate() =>
            _mover.enabled = true;
    }
}