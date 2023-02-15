using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class FloatExtension
    {
        public static bool EqualsApproximately(this float self, float to) =>
            Mathf.Approximately(self, to);
    }
}