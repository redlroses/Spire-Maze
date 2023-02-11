namespace CodeBase.Logic.Lift.PlateMove
{
    public interface IPlateMover
    {
        void Move(LiftDestinationMarker from, LiftDestinationMarker to);
    }
}