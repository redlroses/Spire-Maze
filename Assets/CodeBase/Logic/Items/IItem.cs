using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public interface IItem
    {
        string Name { get; }

        string Description { get; }

        Sprite Sprite { get; }

        StorableType ItemType { get; }

        bool IsInteractive { get; }

        bool IsReloadable { get; }

        bool IsArtifact { get; }
    }
}