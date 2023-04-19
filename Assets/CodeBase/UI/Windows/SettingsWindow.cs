using CodeBase.Services.Localization;
using CodeBase.Services.Sound;
using CodeBase.UI.Elements;
using CodeBase.UI.SelectionGroup;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private SoundSettings _soundSettings;
        [SerializeField] private LanguageSelection _languageSelection;

        public void Construct(ILocalizationService localizationService, ISoundService soundService)
        {
            _languageSelection.Construct(localizationService);
            _soundSettings.Construct(soundService);
        }
    }
}