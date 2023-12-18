using UnityEngine;

namespace CodeBase.Logic.Сollectible.Chests
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