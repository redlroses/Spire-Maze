using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CodeBase.EditorCells;
using TMPro;
using Unity.Collections;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
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
                    newData[i] = new Color32(0, 0, 0, 0);
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

        public static Texture2D RotateTo(this Texture2D texture, PlateMoveDirection direction)
        {
            NativeArray<Color32> textureData = texture.GetRawTextureData<Color32>();
            NativeArray<Color32> newData = new NativeArray<Color32>(textureData.Length, Allocator.Temp);
            int width = texture.width;
            int height = texture.height;

            newData = direction switch
            {
                PlateMoveDirection.Right => RotateToRight(height, width, textureData, newData),
                PlateMoveDirection.Left => RotateToLeft(height, width, newData, textureData),
                PlateMoveDirection.Down => RotateToDown(height, width, newData, textureData),
                _ => textureData,
            };

            Texture2D newTexture = new Texture2D(texture.width, texture.height, texture.format, false);
            newTexture.SetPixelData(newData, 0);
            newTexture.Apply();

            return newTexture;
        }

        private static NativeArray<Color32> RotateToDown(
            int height,
            int width,
            NativeArray<Color32> newData,
            NativeArray<Color32> textureData)
        {
            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    int iRotated = (((height - 1) - j) * width) + i;
                    int iOriginal = (j * width) + i;
                    newData[iRotated] = textureData[iOriginal];
                }
            }

            return newData;
        }

        private static NativeArray<Color32> RotateToLeft(
            int height,
            int width,
            NativeArray<Color32> newData,
            NativeArray<Color32> textureData)
        {
            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    int iRotated = (((i + 1) * height) - j) - 1;
                    int iOriginal = (j * width) + i;
                    newData[iRotated] = textureData[iOriginal];
                }
            }

            return newData;
        }

        private static NativeArray<Color32> RotateToRight(
            int height,
            int width,
            NativeArray<Color32> textureData,
            NativeArray<Color32> newData)
        {
            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    int iRotated = (((i + 1) * height) - j) - 1;
                    int iOriginal = (textureData.Length - 1) - ((j * width) + i);
                    newData[iRotated] = textureData[iOriginal];
                }
            }

            return newData;
        }
    }
}