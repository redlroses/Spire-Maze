using System;

namespace CodeBase.LevelSpecification
{
    [Flags]
    public enum CellType
    {
        Air = 0,
        Plate = 1 << 0,
        Wall = 1 << 1,
        Door = (1 << 2) | Plate,
        Key = (1 << 3) | Plate,
        Left = 1 << 4,
        Up = 1 << 5,
        Right = 1 << 6,
        Down = 1 << 7,
        MovingPlate = 1 << 8,
        MovingMarker = Down | Left |
                       Right | Up,
        All = ~0
    }
}