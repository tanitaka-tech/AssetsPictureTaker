using System;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PictureEncoder
{
    [Serializable]
    public class JpgPictureEncoder : IPictureEncoder
    {
        [SerializeField] private int quality = 75;
        
        public PictureEncodeResult EncodePicture(Texture2D texture2D)
        {
            var pictureBytes = texture2D.EncodeToJPG(quality: quality);
            return new PictureEncodeResult(pictureBytes, "jpg");
        }
    }
}