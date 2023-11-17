using System;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.DelayRoutines
{
    public partial class RoutineSequence
    {
        public RoutineSequence DoScale(Transform transform, float toScale, float duration, AnimationCurve curve, bool isPulse = false)
        {
            Vector3 initialScale = transform.localScale;
            Vector3 finishScale = initialScale * toScale;

            TowardMover<Vector3> towardMover = new TowardMover<Vector3>(initialScale, finishScale, Vector3.Lerp, curve);

            bool ScaleTween()
            {
                bool isProcess = towardMover.TryUpdate(Time.deltaTime / duration, out Vector3 scale);
                transform.localScale = scale;
                return isProcess;
            }

            Then(towardMover.Forward);
            WaitWhile(ScaleTween);

            if (isPulse)
            {
                Then(new Executor(() => towardMover.Switch()));
                WaitWhile(ScaleTween);
            }

            return this;
        }
    }
}