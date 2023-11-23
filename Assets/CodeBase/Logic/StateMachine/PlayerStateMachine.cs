using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine
{
    public class PlayerStateMachine : EntityStateMachine
    {
        private readonly IInputService _inputService;

        public PlayerStateMachine(HeroAnimator heroAnimator, IInputService inputService, HeroMover heroMover, HeroHealth heroHealth, Dodge dodge, Jumper jumper)
        {
            _inputService = inputService;
            States.Add(typeof(PlayerIdleState), new PlayerIdleState(this, heroAnimator, inputService, heroMover, jumper, dodge));
            States.Add(typeof(PlayerMoveState), new PlayerMoveState(this, heroAnimator, inputService, heroMover, jumper, dodge));
            States.Add(typeof(DodgeState), new DodgeState(this, heroAnimator, inputService, dodge, heroHealth));
            States.Add(typeof(JumpState), new JumpState(this, heroAnimator, inputService, jumper, heroMover));
            States.Add(typeof(DiedState), new DiedState(heroAnimator, heroMover));
            States.Add(typeof(ReviveState), new ReviveState(this, heroAnimator, heroHealth));

            inputService.MoveStopped += OnMoveStopped;
        }

        private void OnMoveStopped()
        {
            Enter<PlayerIdleState>();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            _inputService.MoveStopped -= OnMoveStopped;
        }
    }
}