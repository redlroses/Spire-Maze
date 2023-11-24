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
        public bool IsReloadable { get; }
        public bool IsArtifact { get; }

        public Item(StorableStaticData storableData)
        {
            ItemType = storableData.ItemType;
            Sprite = storableData.Sprite;
            Name = storableData.Name;
            Description = storableData.Description;
            IsInteractive = storableData.IsInteractive;
            IsReloadable = storableData.IsReloadable;
            IsArtifact = storableData.IsArtifact;
        }
    }
}