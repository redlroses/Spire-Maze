using CodeBase.Infrastructure.States;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;

namespace CodeBase.Logic.StateMachine.States
{
    public class JumpState : IState
    {
        public JumpState(EntityStateMachine entityStateMachine, HeroAnimator heroAnimator,
            IPlayerInputService playerInputService)
        {
        }

        public void Enter()
        {
            // throw new System.NotImplementedException();
        }

        public void Exit()
        {
            // throw new System.NotImplementedException();
        }
    }
}