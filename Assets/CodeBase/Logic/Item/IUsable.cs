using CodeBase.Logic.Сollectible;

namespace CodeBase.Logic.Item
{
    public interface IUsable : ICollectible
    {
        public void Use();
    }
}