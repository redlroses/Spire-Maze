using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Сollectible
{
    public interface ICollectible
    {
        public StorableStaticData StorableStaticData { get; }
        public void Disable();
    }
}