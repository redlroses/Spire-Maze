namespace CodeBase.Logic.Movement
{
    public interface IAccelerable : IMover
    {
        public void EnableBonusSpeed();
        public void DisableBonusSpeed();
    }
}