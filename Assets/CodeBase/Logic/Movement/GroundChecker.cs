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

        [ShowNonSerializedField] private float _coyoteTime;

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

            if (IsGroundLost(isTouchGround))
            {
                if (_coyoteTime < _coyoteTimeDuration)
                {
                    _coyoteTime += Time.fixedDeltaTime;
                    return;
                }
            }

            _coyoteTime = 0;
            State = isTouchGround ? GroundState.Grounded : GroundState.InAir;
        }

        private bool IsGroundLost(bool isTouchGround) =>
            isTouchGround == false && State == GroundState.Grounded;

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