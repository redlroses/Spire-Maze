using CodeBase.Infrastructure.States;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Movement;
using UnityEngine;

namespace CodeBase.Logic.StateMachine.States
{
    public class DiedState : IState
    {
        private readonly HeroAnimator _heroAnimator;
        private readonly HeroMover _heroMover;

        public DiedState(HeroAnimator heroAnimator, HeroMover heroMover)
        {
            _heroAnimator = heroAnimator;
            _heroMover = heroMover;
        }

        public void Enter()
        {
            _heroMover.Move(MoveDirection.Stop);
            _heroAnimator.PlayDied();
        }

        public void Exit() { }
    }
}