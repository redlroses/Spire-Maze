using System;
using UnityEngine;

namespace CodeBase.Data.Cell
{
    [Serializable]
    public class CellData
    {
        public Texture2D Texture;

        public static CellData Copy(CellData cellData)
        {
            return cellData switch
            {
                Key key => new Key(cellData.Texture, key.Color),
                Door door => new Door(cellData.Texture, door.Color),
                InitialPlate plate => new InitialPlate(cellData.Texture),
                Plate plate => new Plate(cellData.Texture),
                Air air => new Air(cellData.Texture),
                Wall wall => new Wall(cellData.Texture),
                MovingMarker marker => new MovingMarker(cellData.Texture, marker.Direction, marker.IsLiftHolder),
                _ => throw new ArgumentException(nameof(cellData)),
            };
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
            Texture.Apply();
        }

        public CellData(Texture2D texture)
        {
            Texture = texture;
            Texture.Apply();
        }
    }
}