using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public class KeyItem : Item, IUsable, IExpendable
    {
        public KeyItem(StorableStaticData staticData) : base(staticData)
        {
        }

        public void Use()
        {
            Debug.Log($"{Name} Used");
        }
    }
}