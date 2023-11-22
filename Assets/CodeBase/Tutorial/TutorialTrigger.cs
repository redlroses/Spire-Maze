using System;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Observer;
using NTC.Global.System;

namespace CodeBase.Tutorial
{
    public class TutorialTrigger : ObserverTarget<HeroObserver, HeroRoot>
    {
        public event Action Triggered = () => { };

        protected override void OnTriggerObserverEntered(HeroRoot hero)
        {
            this.Disable();
            Triggered.Invoke();
        }
    }
}