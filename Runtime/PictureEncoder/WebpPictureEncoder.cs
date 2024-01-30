#if ASSETS_PICTURE_TAKER_WEBP_SUPPORT

using System;
using UnityEngine;
using WebP;

namespace TanitakaTech.AssetsPictureTaker.PictureEncoder
{
    [Serializable]
    public class WebpPictureEncoder : IPictureEncoder
    {
        [SerializeField] private int quality = 80;

        public WebpPictureEncoder()
        {
        }

        public WebpPictureEncoder(int quality)
        {
            this.quality = quality;
        }
        
        PictureEncodeResult IPictureEncoder.EncodePicture(Texture2D texture2D)
        {
            // NOTE: Need to flip the pixel data of Texture2D when converting to WebP
            // ref: https://github.com/netpyoung/unity.webp/issues/25
            {
                Color[] pixels = texture2D.GetPixels();
                Color[] pixelsFlipped = new Color[pixels.Length];
                var w = texture2D.width;
                var h = texture2D.height;
                for (int y = 0; y < h; y++)
                {
                    Array.Copy(pixels, y * h, pixelsFlipped, (h - y - 1) * w, w);
                }
                texture2D.SetPixels(pixelsFlipped);
            }
            
            var webpBytes = texture2D.EncodeToWebP(
                lQuality: quality,
                lError: out var encodeError
            );
            
            return new PictureEncodeResult(webpBytes, "webp");
        }
    }
}
#endif