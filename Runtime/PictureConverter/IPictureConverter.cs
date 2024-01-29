using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PictureConverter
{
    public interface IPictureConverter
    {
        PictureConvertResult ConvertPicture(string saveDirectory, Texture2D renderResult, string prefabName);
    }

    public class PictureConvertResult
    {
        public string FileNamePath { get; }
        public byte[] PictureBytes { get; }

        public PictureConvertResult(string fileNamePath, byte[] pictureBytes)
        {
            FileNamePath = fileNamePath;
            PictureBytes = pictureBytes;
        }
    }
}