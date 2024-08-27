
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PictureEncoder
{
    public interface IPictureEncoder
    {
        public PictureEncodeResult EncodePicture(Texture2D texture2D);
        public string Extension { get; }
    }

    public class PictureEncodeResult
    {
        public byte[] PictureBytes { get; }

        public PictureEncodeResult(byte[] pictureBytes)
        {
            PictureBytes = pictureBytes;
        }
    }
}