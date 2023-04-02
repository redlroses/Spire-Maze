using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Сollectible
{
    public interface ICollectible
    {
        public StorableData StorableData { get; }
        public void Disable();
    }
}