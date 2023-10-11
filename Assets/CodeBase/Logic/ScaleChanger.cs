using System;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Logic
{
    public class ScaleChanger : Tools.ShowHide<Vector3>, IShowHide
    {
        protected override Func<Vector3, Vector3, float, Vector3> GetLerpFunction() =>
            Vector3.Lerp;

        protected override void ApplyLerpValue(Vector3 lerpValue) =>
            transform.localScale = lerpValue;
    }
}