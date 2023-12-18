namespace CodeBase.Logic.AnimatorStateMachine
{
    public interface IAnimationStateReader
    {
        AnimatorState State { get; }

        void OnEnteredState(int stateHash);

        void OnExitedState(int stateHash);
    }
}