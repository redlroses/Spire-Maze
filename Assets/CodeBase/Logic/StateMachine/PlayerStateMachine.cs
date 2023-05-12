using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine
{
    public class PlayerStateMachine : EntityStateMachine
    {
        public PlayerStateMachine(HeroAnimator heroAnimator, IPlayerInputService playerInputService, HeroMover heroMover, Dodge dodge, Jumper jumper)
        {
            States.Add(typeof(PlayerIdleState), new PlayerIdleState(this, heroAnimator, playerInputService, heroMover, jumper, dodge));
            States.Add(typeof(PlayerMoveState), new PlayerMoveState(this, heroAnimator, playerInputService, heroMover, jumper, dodge));
            States.Add(typeof(DodgeState), new DodgeState(this, heroAnimator, playerInputService, dodge));
            States.Add(typeof(JumpState), new JumpState(this, heroAnimator, playerInputService, jumper, heroMover));
        }
    }
}