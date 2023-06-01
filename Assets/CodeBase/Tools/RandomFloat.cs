using System;
using Random = UnityEngine.Random;

namespace CodeBase.Tools
{
    [Serializable]
    public struct RandomFloat
    {
        public float Min;
        public float Max;

        public RandomFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Randomize() => Random.Range(Min, Max);
    }
}