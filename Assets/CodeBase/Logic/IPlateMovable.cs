using CodeBase.Logic.Lift.PlateMove;

namespace CodeBase.Logic
{
    public interface IPlateMovable
    {
        void OnMovingPlatformEnter(IPlateMover plateMover);
        void OnMovingPlatformExit(IPlateMover plateMover);
    }
}