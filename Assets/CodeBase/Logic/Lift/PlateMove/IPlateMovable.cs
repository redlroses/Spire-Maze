namespace CodeBase.Logic.Lift.PlateMove
{
    public interface IPlateMovable
    {
        void OnMovingPlatformEnter(IPlateMover plateMover);
        void OnMovingPlatformExit(IPlateMover plateMover);
    }
}