using CodeBase.Logic.Hero;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LiftFrictionChanger : ObserverTargetExited<HeroObserver, HeroRoot>
    {
        [SerializeField] private PhysicMaterial _plateMaterial;
        [SerializeField] private float _normalStaticFriction = 1f;
        [SerializeField] private float _enteredStaticFriction = 0f;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            _plateMaterial.staticFriction = _normalStaticFriction;
        }

        protected override void OnTriggerObserverEntered(HeroRoot target)
        {
            _plateMaterial.staticFriction = _enteredStaticFriction;
        }

        protected override void OnTriggerObserverExited(HeroRoot target)
        {
            _plateMaterial.staticFriction = _normalStaticFriction;
        }
    }
}