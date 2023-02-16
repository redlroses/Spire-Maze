namespace CodeBase.Infrastructure.States.Enemy
{
    public interface IEnemyState : IState, IExitableState
    {
        public void Update();
    }
}