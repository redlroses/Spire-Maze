using CodeBase.Logic.DoorEnvironment;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LobbyDoor : MonoBehaviour
    {
        [SerializeField] private DoorAnimator _animator;
        [SerializeField] private int _targetLevelId;

        public void Construct(int lastCompletedLevelId)
        {
            TryOpen(lastCompletedLevelId);
        }

        private void TryOpen(int lastCompletedLevelId)
        {
            if (_targetLevelId <= lastCompletedLevelId)
            {
                _animator.Open();
            }
        }
    }
}