using System;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class MathExtensions
    {
        public static bool EqualsApproximately(this float self, float to) =>
            Mathf.Approximately(self, to);

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

        public static float ClampRound(this float self, float min, float max)
        {
            float difference = max - min + 1;

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

        public static int AsSign(this bool condition) =>
            condition ? 1 : -1;
    }
}