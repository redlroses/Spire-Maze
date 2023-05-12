using CodeBase.Logic.Player;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine
{
    public class PlayerStateMachine : EntityStateMachine
    {
        public PlayerStateMachine(HeroAnimator heroAnimator, IPlayerInputService playerInputService)
        {
            States.Add(typeof(PlayerMoveState), new PlayerIdleState(this, heroAnimator, playerInputService));
            // States.Add(typeof(PlayerMoveState), new PlayerMoveState(this, heroAnimator, playerInputService));
        }
    }
}