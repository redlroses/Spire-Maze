using CodeBase.Infrastructure.States;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Portal;

namespace CodeBase.Logic
{
    public class FinishPortal : ObserverTarget<TeleportableObserver, ITeleportable>
    {
        private GameStateMachine _stateMachine;

        public void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected override void OnTriggerObserverEntered(ITeleportable target)
        {
            _stateMachine.Enter<FinishState>();
        }
    }
}