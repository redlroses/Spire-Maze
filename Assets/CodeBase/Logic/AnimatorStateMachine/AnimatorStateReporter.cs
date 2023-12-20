using UnityEngine;

namespace CodeBase.Logic.AnimatorStateMachine
{
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimationStateReader _stateReader;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            FindAnimationStateReader(animator);

            _stateReader.OnEnteredState(stateInfo.shortNameHash);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            FindAnimationStateReader(animator);

            _stateReader.OnExitedState(stateInfo.shortNameHash);
        }

        private void FindAnimationStateReader(Animator animator)
        {
            if (_stateReader != null)
                return;

            _stateReader = animator.gameObject.GetComponent<IAnimationStateReader>();
        }
    }
}