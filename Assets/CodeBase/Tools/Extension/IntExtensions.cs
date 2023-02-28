namespace CodeBase.Tools.Extension
{
    public static class IntExtensions
    {
        public static int ClampRound(this int self, int min, int max)
        {
            int difference = max - min;

            while (self >= max)
            {
                self -= difference;
            }

            while (self < min)
            {
                self += difference;
            }

            return self;
        }
    }
}