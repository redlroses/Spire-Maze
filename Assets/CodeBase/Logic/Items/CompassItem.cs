using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public sealed class CompassItem : Item, IUsable, IReloadable
    {
        public float ReloadTime { get; }

        public CompassItem(StorableStaticData staticData) : base(staticData)
        {
            ReloadTime = staticData.ReloadTime;
        }

        public void Use()
        {
            Debug.Log("CompassItem Used");
        }
    }
}