using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public sealed class CompassItem : Item, IUsable
    {
        public CompassItem(StorableStaticData staticData) : base(staticData)
        {
        }

        public void Use()
        {
            Debug.Log("CompassItem Used");
        }
    }
}