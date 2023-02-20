﻿using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class TextureExtension
    {
        public static Texture2D CombineTexture(this Texture2D first, Texture2D second)
        {
            NativeArray<Color32> firstData = first.GetRawTextureData<Color32>();
            NativeArray<Color32> secondData = second.GetRawTextureData<Color32>();
            Color32[] newData = new Color32[firstData.Count()];

            for (int i = 0; i < firstData.Length; i++)
            {
                if (firstData[i].a != 0)
                {
                    newData[i] = firstData[i];
                }
                else if (secondData[i].a != 0)
                {
                    newData[i] = secondData[i];
                }
                else
                {
                    newData[i] = new Color32(0,0,0,0);
                }
            }

            NativeArray<Color32> newColorData = new NativeArray<Color32>(newData, Allocator.Temp);
            Texture2D newTexture = new Texture2D(first.width, first.height, first.format, false);
            newTexture.SetPixelData(newColorData, 0);
            newTexture.Apply();
            return newTexture;
        }

        public static Texture2D Tint(this Texture2D texture, Color32 color)
        {
            NativeArray<Color32> textureData = texture.GetRawTextureData<Color32>();
            NativeArray<Color32> newData = new NativeArray<Color32>(textureData.Length, Allocator.Temp);

            for (int i = 0; i < textureData.Length; i++)
            {
                newData[i] = textureData[i].Multiply(color);
            }

            Texture2D newTexture = new Texture2D(texture.width, texture.height, texture.format, false);
            newTexture.SetPixelData(newData, 0);
            newTexture.Apply();
            return newTexture;
        }
    }
}