using CodeBase.Logic.Item;

namespace CodeBase.Logic.Сollectible
{
    public interface ICollectible
    {
        public IItem Item { get; }
        public void Disable();
    }
}