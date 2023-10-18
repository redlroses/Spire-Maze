using System;
using System.Collections.Generic;
using I2.Loc;

namespace CodeBase.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private Dictionary<LanguageId, string> _locales;

        public LanguageId Current { get; }

        public LocalizationService()
        {
            LocalizationManager.InitializeIfNeeded();
            List<string> languages = LocalizationManager.GetAllLanguages();

            _locales = new Dictionary<LanguageId, string>
            {
                [LanguageId.English] = ParseLanguage(languages, LanguageId.English),
                [LanguageId.Russian] = ParseLanguage(languages, LanguageId.Russian),
                [LanguageId.Turkish] = ParseLanguage(languages, LanguageId.Turkish),
            };
        }

        private string ParseLanguage(List<string> languages, LanguageId languageId)
        {
            string languageName = languageId.ToString();

            if (languages.Contains(languageName))
                return languageName;

            throw new ArgumentException($"LocalizationManager does not contains language with id - {languageId}");
        }

        public void ChooseLanguage(LanguageId languageId)
        {
            LocalizationManager.CurrentLanguage = _locales[languageId];
        }
    }
}