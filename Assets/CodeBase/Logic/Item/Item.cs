using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Item
{
    public class Item : IItem
    {
        private readonly StorableType _itemType;
        private readonly Sprite _sprite;
        private readonly string _name;
        private readonly string _description;

        public string Description => _description;
        public string Name => _name;
        public Sprite Sprite => _sprite;
        public StorableType ItemType => _itemType;

        public Item(StorableStaticData staticData)
        {
            _itemType = staticData.ItemType;
            _sprite = staticData.Sprite;
            _name = staticData.Name;
            _description = staticData.Description;
        }
    }
}