using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public readonly struct CellPosition
    {
        public float Height { get; }
        public float Angle { get; }

        public CellPosition(float height, float angle)
        {
            Height = height;
            Angle = angle;
        }

        public override string ToString() => $"Height: {Height}, Angle: {Angle}";

        public static bool operator ==(CellPosition self, CellPosition target) =>
            Mathf.Approximately(self.Angle, target.Angle) && Mathf.Approximately(self.Height, target.Height);

        public static bool operator !=(CellPosition self, CellPosition target) =>
            Mathf.Approximately(self.Angle, target.Angle) == false
            || Mathf.Approximately(self.Height, target.Height) == false;
    }
}