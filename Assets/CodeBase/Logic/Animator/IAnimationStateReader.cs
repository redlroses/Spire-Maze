namespace CodeBase.Logic.Animator
{
  public interface IAnimationStateReader
  {
    void EnteredState(int stateHash);
    void ExitedState(int stateHash);
    AnimatorState State { get; }
  }
}