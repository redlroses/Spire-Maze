namespace CodeBase.Tools.Extension
{
    public static class AngleExtension
    {
        public static float Clamp360(this float angle)
        {
            angle %= Constants.Trigonometry.TwoPiGrade;

            if (angle < 0)
            {
                angle += Constants.Trigonometry.TwoPiGrade;
            }

            return angle;
        }
    }
}