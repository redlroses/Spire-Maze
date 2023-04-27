using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PositionOnLevel
  {
    public Vector3Data Position;

    public PositionOnLevel(Vector3Data position)
    {
      Position = position;
    }

    public PositionOnLevel()
    {
    }
  }
}