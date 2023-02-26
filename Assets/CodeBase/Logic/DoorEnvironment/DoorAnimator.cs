using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    public class DoorAnimator : MonoBehaviour
    {
        private static readonly int Direction = UnityEngine.Animator.StringToHash("Direction");
        private static readonly int OpenTrigger = UnityEngine.Animator.StringToHash("Open");
        private static readonly int CloseTrigger = UnityEngine.Animator.StringToHash("Close");

        [SerializeField] private UnityEngine.Animator _animator;

        public void Open(float direction)
        {
            _animator.SetFloat(Direction, direction);
            _animator.SetTrigger(OpenTrigger);
        }

        public void Close()
        {
            _animator.SetTrigger(CloseTrigger);
        }
    }
}