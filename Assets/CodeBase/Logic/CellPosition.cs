namespace CodeBase.Logic
{
    public readonly struct CellPosition
    {
        public CellPosition(float height, float angle)
        {
            Height = height;
            Angle = angle;
        }

        public float Height { get; }

        public float Angle { get; }
    }
}