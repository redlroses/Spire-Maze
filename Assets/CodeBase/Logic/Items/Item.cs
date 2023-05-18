using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public class Item : IItem
    {
        public string Description { get; }
        public string Name { get; }
        public Sprite Sprite { get; }
        public StorableType ItemType { get; }
        public bool IsInteractive { get; }

        public Item(StorableStaticData staticData)
        {
            ItemType = staticData.ItemType;
            Sprite = staticData.Sprite;
            Name = staticData.Name;
            Description = staticData.Description;
            IsInteractive = staticData.IsInteractive;
        }
    }
}