using UnityEngine;

namespace CodeBase.Logic.ChestItem
{
    public class ChestAnimator : MonoBehaviour
    {
        private static readonly int OpenTrigger = Animator.StringToHash("Open");
     
        [SerializeField] private Animator _animator;

        public void Open()
        {
            _animator.SetTrigger(OpenTrigger);
        }
    }
}