using System;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.Texture2DProcessor
{
    [Serializable]
    public class Texture2DProcessor_Resize : ITexture2DProcessor
    {
        [SerializeField] private int width = 512;
        [SerializeField] private int height = 512;
        
        Texture2D ITexture2DProcessor.Process(Texture2D originalTexture)
        {
            // 新しいTexture2Dを作成
            Texture2D resizedTexture = new Texture2D(width, height, originalTexture.format, false);

            // 新しい解像度でピクセルをサンプリングして設定
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xFraction = x / (float)width;
                    float yFraction = y / (float)height;
                    Color sampledColor = originalTexture.GetPixelBilinear(xFraction, yFraction);
                    resizedTexture.SetPixel(x, y, sampledColor);
                }
            }

            // テクスチャの更新を適用
            resizedTexture.Apply();

            return resizedTexture;
        }
    }
}