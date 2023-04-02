using UnityEngine;

namespace CodeBase.Logic.DoorEnvironment
{
    public class DoorAnimator : MonoBehaviour
    {
        private static readonly int Direction = Animator.StringToHash("Direction");
        private static readonly int OpenTrigger = Animator.StringToHash("Open");
        private static readonly int CloseTrigger = Animator.StringToHash("Close");

        [SerializeField] private Animator _animator;

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