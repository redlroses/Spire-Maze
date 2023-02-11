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
    }
}