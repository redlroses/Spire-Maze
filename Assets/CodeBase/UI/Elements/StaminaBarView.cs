using CodeBase.DelayRoutines;
using CodeBase.Logic.StaminaEntity;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public sealed class StaminaBarView : BarView<IStamina>
    {
        [Space] [Header("Background Indication")] [SerializeField]
        private Gradient _backgroundGradient;

        [SerializeField] private Graphic _background;
        [SerializeField] private float _duration;

        [Space] [Header("Fill Indication")] [SerializeField]
        private Gradient _fillGradient;

        [SerializeField] private Graphic _fill;

        private RoutineSequence _flashing;

        protected override void OnConstruct()
        {
            Points.AttemptToEmptyUsed += IndicateEmptyUse;

            _flashing = new RoutineSequence()
                .DoGradient(_background,
                    _backgroundGradient,
                    _duration,
                    AnimationCurve.Linear(0, 0, 1, 1),
                    true);
        }

        protected override void OnChanged()
        {
            base.OnChanged();
            PaintFill();
        }

        protected override void OnDestroyed() =>
            Points.AttemptToEmptyUsed -= IndicateEmptyUse;

        private void PaintFill() =>
            _fill.color = _fillGradient.Evaluate(Points.CurrentPoints / (float)Points.MaxPoints);

        private void IndicateEmptyUse()
        {
            if (_flashing.IsActive == false)
                _flashing.Play();
        }
    }
}