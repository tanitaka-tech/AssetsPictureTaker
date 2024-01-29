using System;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PictureEncoder
{
    [Serializable]
    public class PngPictureEncoder : IPictureEncoder
    {
        public PictureEncodeResult EncodePicture(Texture2D texture2D)
        {
            var pictureBytes = texture2D.EncodeToPNG();
            return new PictureEncodeResult(pictureBytes, "png");
        }
    }
}