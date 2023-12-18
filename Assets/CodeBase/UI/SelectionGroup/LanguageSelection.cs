using CodeBase.Services.Localization;

namespace CodeBase.UI.SelectionGroup
{
    public class LanguageSelection : ToggleSelectionGroup<LanguageId>
    {
        private ILocalizationService _localization;

        public void Construct(ILocalizationService localization)
        {
            _localization = localization;
            SetDefault(localization.Current);
        }

        protected override void OnSelectionChanged(LanguageId languageId)
        {
            _localization.ChooseLanguage(languageId);
        }
    }
}