using UnityEngine;

namespace CodeBase.EditorCells
{
    public class EnemySpawnPoint : Plate
    {
        public EnemyType Type;

        public EnemySpawnPoint(Texture2D texture, EnemyType type = EnemyType.Generic)
            : base(texture)
        {
            Type = type;
        }

        public override CellData Copy() =>
            new EnemySpawnPoint(Texture, Type);
    }
}