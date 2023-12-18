using System;
using CodeBase.DelayRoutines;
using CodeBase.Logic.Hero;
using CodeBase.Services.ADS;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements.Buttons
{
    public class ReviveButton : ButtonObserver
    {
        private const string BeatAnimationName = "Beat";

        [Header("Token")]
        [SerializeField] private TextSetter _tokens;
        [SerializeField] private string _format = "X{0}";

        [Space] [Header("Heart Animation")]
        [SerializeField] private LocationAnimations _locationAnimations;
        [SerializeField] private float _beatCooldown;

        private HeroReviver _reviver;
        private IADService _adService;
        private Action _onRevived;
        private RoutineSequence _heartAnimation;
        private LocationAnimations.LocationAnimationEvent _onFinishAnimation;

        public void Construct(IADService adService, HeroReviver reviver, Action onRevived = null)
        {
            _onRevived = onRevived ?? (() => { });
            _adService = adService;
            _reviver = reviver;
            _onFinishAnimation = new LocationAnimations.LocationAnimationEvent(() => _heartAnimation.Play());
            _heartAnimation = new RoutineSequence().WaitForSeconds(_beatCooldown).Then(HeartBeat);
            _heartAnimation.Play();
            UpdateTokens();
            Subscribe();
        }

        private void HeartBeat() =>
            _locationAnimations.StartAnimation(BeatAnimationName, _onFinishAnimation);

        private void UpdateTokens() =>
            _tokens.SetText(string.Format(_format, _reviver.LeftReviveTokens));

        protected override void Call() =>
            _adService.ShowRewardAd(OnRewarded);

        private void OnRewarded()
        {
            if (_reviver.TryRevive())
                _onRevived.Invoke();

            UpdateTokens();
        }
    }
}