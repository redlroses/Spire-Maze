using System;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Data.Cell
{
    [Serializable]
    public class Door : Plate
    {
        public Colors Color;

        public Door(Colors colorType = Colors.None)
        {
            var key = Resources.Load<Texture2D>("Textures/DoorIcon");

            switch (colorType)
            {
                case Colors.Blue:
                    key = key.Tint(new Color32(46, 51, 214, 255));
                    break;
                case Colors.Green:
                    key = key.Tint(new Color32(39, 214, 61, 255));
                    break;
                case Colors.Red:
                    key = key.Tint(new Color32(214, 40, 48, 255));
                    break;
            }

            Color = colorType;
            Texture = Texture.CombineTexture(key);
        }
    }
}