using CodeBase.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.DelayRoutines
{
    public partial class RoutineSequence
    {
        public RoutineSequence DoScale(Transform transform,
            float toScale,
            float duration,
            AnimationCurve curve,
            bool isPulse = false)
        {
            Vector3 initialScale = transform.localScale;
            Vector3 finishScale = initialScale * toScale;

            TowardMover<Vector3> towardMover = new TowardMover<Vector3>(initialScale, finishScale, Vector3.Lerp, curve);

            bool Tween()
            {
                bool isProcess = towardMover.TryUpdate(Time.deltaTime / duration, out Vector3 scale);
                transform.localScale = scale;
                return isProcess;
            }

            Then(towardMover.Forward);
            WaitWhile(Tween);

            if (isPulse)
            {
                Then(new Executor(() => towardMover.Switch()));
                WaitWhile(Tween);
            }

            return this;
        }

        public RoutineSequence DoGradient(Graphic graphics,
            Gradient gradient,
            float duration,
            AnimationCurve curve,
            bool isPulse = false)
        {
            TowardMover<float> towardMover = new TowardMover<float>(0f, 1f, Mathf.Lerp, curve);

            bool Tween()
            {
                bool isProcess = towardMover.TryUpdate(Time.deltaTime / duration, out float value);
                graphics.color = gradient.Evaluate(value);
                return isProcess;
            }

            Then(towardMover.Forward);
            WaitWhile(Tween);

            if (isPulse)
            {
                Then(new Executor(() => towardMover.Switch()));
                WaitWhile(Tween);
            }

            return this;
        }
    }
}