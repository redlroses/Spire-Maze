using AYellowpaper;
using CodeBase.DelayRoutines;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity.Damage
{
    [RequireComponent(typeof(HeroObserver))]
    public sealed class LiftKillZone : ObserverTargetExited<HeroObserver, HeroRoot>
    {
        [SerializeField] private MeshCollider _liftSolidCollider;
        [SerializeField] private InterfaceReference<IDamageTrigger, MonoBehaviour> _damageTrigger;

        private GroundChecker _gravityScaler;
        private RoutineSequence _killZoneHeroEntryUpdater;

        private void OnDestroy() =>
            _killZoneHeroEntryUpdater.Kill();

        protected override void OnAwake()
        {
            _killZoneHeroEntryUpdater = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .Then(UpdateDamageTriggerState);
            _damageTrigger.Value.Disable();
        }

        protected override void OnTriggerObserverEntered(HeroRoot target)
        {
            _gravityScaler = target.GetComponent<GroundChecker>();
            _killZoneHeroEntryUpdater.Play();
        }

        protected override void OnTriggerObserverExited(HeroRoot target) =>
            _killZoneHeroEntryUpdater.Stop();

        private void UpdateDamageTriggerState()
        {
            if (_gravityScaler.State == GroundState.Grounded)
            {
                _damageTrigger.Value.Enable();
                _liftSolidCollider.enabled = false;
            }
            else
            {
                _damageTrigger.Value.Disable();
                _liftSolidCollider.enabled = true;
            }
        }
    }
}