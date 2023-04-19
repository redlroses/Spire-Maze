namespace CodeBase.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public LanguageId Current { get; }
        public void ChooseLanguage(LanguageId languageId)
        {
            throw new System.NotImplementedException();
        }
    }
}