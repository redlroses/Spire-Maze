using System;
using CodeBase.Logic.Lift.PlateMove;

namespace CodeBase.Logic.Lift
{
    public interface ILiftPlate
    {
        event Action<LiftState> StateChanged;

        LiftState State { get; }

        IPlateMover Mover { get; }
    }
}