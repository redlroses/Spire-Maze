using CodeBase.StaticData.Storable;

namespace CodeBase.Logic.Items
{
    public class Key : Item, IExpendable
    {
        public Key(StorableStaticData staticData) : base(staticData) { }
    }
}