using CodeBase.Services.Localization;
using UnityEngine;

namespace CodeBase.UI.SelectionGroup
{
    public class LanguageSelection : ToggleSelectionGroup<LanguageId>
    {
        private ILocalizationService _localization;

        private void Awake()
        {
            SetDefault(LanguageId.Russian);
        }

        public void Construct(ILocalizationService localization)
        {
            _localization = localization;
        }
        
        protected override void OnSelectionChanged(LanguageId languageId)
        {
            Debug.Log($"Select language: {languageId}");
            _localization.ChooseLanguage(languageId);
        }
    }
}