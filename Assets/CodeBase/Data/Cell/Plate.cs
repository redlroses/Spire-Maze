using System;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Data.Cell
{
    [Serializable]
    public class Plate : CellData
    {
        public Plate()
        {
            Texture2D plate = Resources.Load<Texture2D>("Textures/PlateIcon");
            Texture = plate.Tint(new Color32(104, 204, 32, 255));
        }
    }
}