namespace CodeBase.Logic.Lift.PlateMove
{
    public interface IMovableByPlate
    {
        void OnMovingPlatformEnter(IPlateMover plateMover);

        void OnMovingPlatformExit(IPlateMover plateMover);
    }
}