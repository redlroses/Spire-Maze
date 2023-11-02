namespace CodeBase.Logic.Movement
{
    public interface IMover
    {
        public void Move(MoveDirection direction, float speedFactor = 1f);
    }
}