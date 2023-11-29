using CodeBase.Services.Pause;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(SphereCaster))]
    public class GroundChecker : MonoCache, IPauseWatcher
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCaster _sphereCaster;
        [SerializeField] private float _coyoteTimeDuration;

        [ShowNativeProperty]
        public GroundState State { get; private set; } = GroundState.Grounded;

        private void Awake()
        {
            _sphereCaster ??= GetComponent<SphereCaster>();
            _rigidbody ??= GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
        }

        protected override void FixedRun()
        {
            bool isTouchGround = IsTouchGround();
            State = isTouchGround ? GroundState.Grounded : GroundState.InAir;
        }

        public void Enable() =>
            enabled = true;

        public void Disable()
        {
            enabled = false;
            State = GroundState.InAir;
        }

        public void Resume() =>
            enabled = true;

        public void Pause() =>
            enabled = false;

        private bool IsTouchGround() =>
            _sphereCaster.CastSphere(Vector3.down, Jumper.GroundCheckDistance);
    }
}