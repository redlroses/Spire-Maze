using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.EditorCells
{
    public class ItemSpawnPoint : Plate
    {
        public StorableType Type;

        public ItemSpawnPoint(Texture2D texture, StorableType type = StorableType.None) : base(texture)
        {
            Type = type;
        }

        public override CellData Copy() =>
            new ItemSpawnPoint(Texture, Type);
    }
}