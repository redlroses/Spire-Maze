namespace CodeBase.Logic.Lift
{
    public interface ILiftPlate
    {
        LiftState State { get; }
        void Move();
    }
}