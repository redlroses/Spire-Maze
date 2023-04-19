using UnityEngine;

namespace CodeBase.UI.Elements
{
    public sealed class HealthBarView : BarView
    {
        [SerializeField] private TextSetter _textSetter;

        protected override void OnChanged()
        {
            base.OnChanged();
            ApplyTextHealth();
        }

        private void ApplyTextHealth()
        {
            _textSetter.SetText($"{Parameter.CurrentPoints}/{Parameter.MaxPoints}");
        }
    }
}
