﻿using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Key : ColoredCell
    {
        public Key(Texture2D texture, Colors colorType = Colors.None) : base(texture, colorType)
        {
        }

        public override CellData Copy2() =>
            new Key(Texture, Color);
    }
}