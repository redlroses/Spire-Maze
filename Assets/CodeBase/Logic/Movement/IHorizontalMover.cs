namespace CodeBase.Logic.Movement
{
    public interface IHorizontalMover : IMover
    {
        public void HorizontalMove(MoveDirection direction);
    }
}