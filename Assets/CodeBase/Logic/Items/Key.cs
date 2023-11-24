using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Items
{
    public class Key : Item, ISpendable
    {
        public Key(StorableStaticData storableData) : base(storableData) { }
    }
}