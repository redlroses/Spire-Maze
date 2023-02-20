using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Air : CellData
    {
        public Air()
        {
            Texture = Resources.Load<Texture2D>("Textures/AirIcon");
        }
    }
}