using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Item
{
    public class Compass : Item, IUsable
    {
        public Compass(StorableStaticData staticData) : base(staticData)
        {
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}