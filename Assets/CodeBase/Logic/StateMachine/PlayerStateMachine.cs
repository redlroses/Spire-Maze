using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine
{
    public class PlayerStateMachine : EntityStateMachine
    {
        private readonly IInputService _inputService;

        public PlayerStateMachine(
            HeroAnimator heroAnimator,
            IInputService inputService,
            HeroMover heroMover,
            HeroHealth heroHealth,
            Dodge dodge,
            Jumper jumper)
        {
            _inputService = inputService;
            States.Add(typeof(IdleState), new IdleState(this, inputService, heroMover, jumper, dodge));
            States.Add(typeof(MoveState), new MoveState(this, inputService, heroMover, jumper, dodge));
            States.Add(typeof(DodgeState), new DodgeState(this, heroAnimator, inputService, heroHealth));
            States.Add(typeof(JumpState), new JumpState(this, heroAnimator, inputService, heroMover));
            States.Add(typeof(DiedState), new DiedState(heroAnimator, heroMover));
            States.Add(typeof(ReviveState), new ReviveState(this, heroAnimator, heroHealth));

            inputService.MoveStopped += OnMoveStopped;
        }

        public override void Cleanup()
        {
            base.Cleanup();
            _inputService.MoveStopped -= OnMoveStopped;
        }

        private void OnMoveStopped() =>
            Enter<IdleState>();
    }
}