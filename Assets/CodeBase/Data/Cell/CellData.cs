using System;
using UnityEngine;

namespace CodeBase.Data.Cell
{
    [Serializable]
    public class CellData
    {
        public Texture2D Texture;

        protected CellData(Texture2D texture = null)
        {
            Texture = texture;
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public static CellData Copy(CellData cellData)
        {
            return cellData switch
            {
                Key key => new Key(key.Color),
                Door door => new Door(door.Color),
                Plate plate => new Plate(),
                Air air => new Air(),
                Wall wall => new Wall(),
                _ => new Air()
            };
        }
    }
}