
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PictureEncoder
{
    public interface IPictureEncoder
    {
        public PictureEncodeResult EncodePicture(Texture2D texture2D);
    }

    public class PictureEncodeResult
    {
        public byte[] PictureBytes { get; }
        public string Extension { get; }

        public PictureEncodeResult(byte[] pictureBytes, string extension)
        {
            PictureBytes = pictureBytes;
            Extension = extension;
        }
    }
}