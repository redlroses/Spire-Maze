namespace CodeBase.Services.Localization
{
    public interface ILocalizationService : IService
    {
        LanguageId Current { get; }

        void ChooseLanguage(LanguageId languageId);
    }
}