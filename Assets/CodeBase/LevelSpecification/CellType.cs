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
        MovingMarkerLeft = 1 << 4,
        MovingMarkerUp = 1 << 5,
        MovingMarkerRight = 1 << 6,
        MovingMarkerDown = 1 << 7,
        MovingPlate = 1 << 8,
    }
}