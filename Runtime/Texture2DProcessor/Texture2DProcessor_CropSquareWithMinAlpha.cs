using System;
using System.Linq;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.Texture2DProcessor
{
    [Serializable]
    public class Texture2DProcessor_CropSquareWithMinAlpha : ITexture2DProcessor
    {
        [SerializeField] private float cropMarginPixel = 0.0f;
        
        Texture2D ITexture2DProcessor.Process(Texture2D originalTexture)
        {
            int width = originalTexture.width;
            int height = originalTexture.height;

            // 不透明ピクセルが存在する最小の矩形を見つける
            int xMin = width, xMax = 0, yMin = height, yMax = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = originalTexture.GetPixel(x, y);
                    if (pixelColor.a > 0) // アルファ値が0より大きい場合、ピクセルは不透明
                    {
                        if (x < xMin) xMin = x;
                        if (x > xMax) xMax = x;
                        if (y < yMin) yMin = y;
                        if (y > yMax) yMax = y;
                    }
                }
            }

            // marginを考慮して矩形を拡大
            xMin = Mathf.Max(0, xMin - Mathf.FloorToInt(cropMarginPixel));
            xMax = Mathf.Min(width - 1, xMax + Mathf.FloorToInt(cropMarginPixel));
            yMin = Mathf.Max(0, yMin - Mathf.FloorToInt(cropMarginPixel));
            yMax = Mathf.Min(height - 1, yMax + Mathf.FloorToInt(cropMarginPixel));

            // 切り抜く矩形の幅と高さ
            int cropWidth = xMax - xMin + 1;
            int cropHeight = yMax - yMin + 1;

            // 新しいTexture2Dのサイズを決定（幅と高さを同じにする）
            int newSize = Mathf.Max(cropWidth, cropHeight);

            // 切り抜いたTexture2Dを作成
            Texture2D croppedTexture = new Texture2D(cropWidth, cropHeight);
            croppedTexture.SetPixels(originalTexture.GetPixels(xMin, yMin, cropWidth, cropHeight));
            croppedTexture.Apply();

            // 幅と高さが同じTexture2Dを作成し、中央に切り抜いたTexture2Dを配置
            Texture2D squareTexture = new Texture2D(newSize, newSize, TextureFormat.ARGB32, false);
            squareTexture.SetPixels(Enumerable.Repeat(Color.clear, newSize * newSize).ToArray()); // 全体を透明で埋める
            squareTexture.Apply();

            int xOffset = (newSize - cropWidth) / 2;
            int yOffset = (newSize - cropHeight) / 2;

            for (int y = 0; y < cropHeight; y++)
            {
                for (int x = 0; x < cropWidth; x++)
                {
                    squareTexture.SetPixel(x + xOffset, y + yOffset, croppedTexture.GetPixel(x, y));
                }
            }

            squareTexture.Apply();

            return squareTexture;
        }
    }
}