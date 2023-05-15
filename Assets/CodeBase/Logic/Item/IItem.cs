using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Item
{
    public interface IItem
    {
        string Description { get; }
        string Name { get; }
        Sprite Sprite { get; }
        StorableType ItemType { get; }
    }
}