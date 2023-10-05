using System;

namespace CodeBase.Data
{
  [Serializable]
  public class LevelPositions
  {
    public Vector3Data InitialPosition;
    public Vector3Data FinishPosition;

    public LevelPositions(Vector3Data initialPosition, Vector3Data finishPosition)
    {
      InitialPosition = initialPosition;
      FinishPosition = finishPosition;
    }

    public LevelPositions() { }
  }
}