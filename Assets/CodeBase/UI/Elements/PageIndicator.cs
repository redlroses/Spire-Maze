using System;
using NTC.Global.System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class PageIndicator : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        [Space] [Header("Indication Template")]
        [SerializeField] private Transform _template;

        public event Action<int> PageIndexUpdated = _ => { };

        private void OnValidate() =>
            _slider ??= GetComponent<Slider>();

        private void OnDestroy() =>
            _slider.onValueChanged.RemoveAllListeners();

        public void Construct(int pagesCount)
        {
            InitSlider(pagesCount);
            GenerateIndicators(pagesCount);
            _slider.onValueChanged.AddListener(index => PageIndexUpdated.Invoke(Mathf.RoundToInt(index)));
        }

        public void SetPage(int page) =>
            _slider.SetValueWithoutNotify(page);

        private void InitSlider(int pagesCount)
        {
            _slider.minValue = 0;
            _slider.maxValue = pagesCount - 2;
            _slider.wholeNumbers = this;
        }

        private void GenerateIndicators(int count)
        {
            for (int i = 1; i < count - 1; i++)
                Instantiate(_template, _template.parent);

            _template.gameObject.Disable();
        }
    }
}
