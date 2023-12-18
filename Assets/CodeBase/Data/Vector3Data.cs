using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Vector3Data
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3Data()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3Data(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}