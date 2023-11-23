using System;
using UnityEngine;

namespace CodeBase.EditorCells
{
    [Serializable]
    public abstract class CellData
    {
        public Texture2D Texture;

        public CellData(Texture2D texture)
        {
            SetTexture(texture);
        }

        public abstract CellData Copy();

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
            Texture.Apply();
        }
    }
}