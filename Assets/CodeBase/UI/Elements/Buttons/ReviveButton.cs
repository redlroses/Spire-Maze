using System;
using CodeBase.Logic.Player;
using CodeBase.Services.ADS;
using UnityEngine;

namespace CodeBase.UI.Elements.Buttons
{
    public class ReviveButton : ButtonObserver
    {
        [SerializeField] private TextSetter _tokens;
        [SerializeField] private string _format = "X{0}";

        private HeroReviver _reviver;
        private IADService _adService;
        private Action _onRevived;

        public void Construct(IADService adService, HeroReviver reviver, Action onRevived = null)
        {
            _onRevived = onRevived ?? (() => { });
            _adService = adService;
            _reviver = reviver;
            UpdateTokens();
        }

        private void UpdateTokens()
        {
            _tokens.SetText(string.Format(_format, _reviver.LeftReviveTokens));
        }

        protected override void Call()
        {
            _adService.ShowRewardAd(OnRewarded);
        }

        private void OnRewarded()
        {
            if (_reviver.TryRevive())
            {
                _onRevived.Invoke();
            }

            UpdateTokens();
        }
    }
}