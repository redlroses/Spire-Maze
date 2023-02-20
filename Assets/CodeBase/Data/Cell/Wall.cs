using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Wall : CellData
    {
        public Wall()
        {
            Texture2D wall = Resources.Load<Texture2D>("Textures/WallIcon");
            Texture = wall.Tint(new Color32(104, 204, 32, 255));
        }
    }
}