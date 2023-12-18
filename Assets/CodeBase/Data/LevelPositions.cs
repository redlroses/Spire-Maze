using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LevelPositions
    {
        public Vector3Data InitialPosition;
        public Vector3Data FinishPosition;

        public LevelPositions(Vector3Data initialPosition, Vector3Data finishPosition)
        {
            InitialPosition = initialPosition;
            FinishPosition = finishPosition;
        }

        public LevelPositions()
        {
        }
    }
}