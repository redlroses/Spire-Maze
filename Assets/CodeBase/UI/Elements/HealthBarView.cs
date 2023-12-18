using CodeBase.Logic.HealthEntity;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public sealed class HealthBarView : BarView<IHealth>
    {
        [SerializeField] private TextSetter _textSetter;

        protected override void OnChanged()
        {
            base.OnChanged();
            ApplyTextHealth();
        }

        private void ApplyTextHealth()
        {
            _textSetter.SetText($"{Points.CurrentPoints}/{Points.MaxPoints}");
        }
    }
}