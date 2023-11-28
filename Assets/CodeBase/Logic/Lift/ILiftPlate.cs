using System;
using CodeBase.Logic.Lift.PlateMove;

namespace CodeBase.Logic.Lift
{
    public interface ILiftPlate
    {
        LiftState State { get; }
        IPlateMover Mover { get; }
        event Action<LiftState> StateChanged;
    }
}