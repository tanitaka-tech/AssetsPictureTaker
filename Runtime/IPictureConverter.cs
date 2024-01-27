using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker
{
    public interface IPictureConverter
    {
        PictureConvertResult ConvertPicture(string saveDirectory, Texture2D renderResult, string prefabName);
    }
    
    [Serializable]
    public class PictureConverter : IPictureConverter
    {
        private string pattern = @"P_GameObject_(\d+)";
        private string replacement = "P_GameObject__$1";
        
        public PictureConvertResult ConvertPicture(string saveDirectory, Texture2D renderResult, string prefabName)
        {
            byte[] imageBytes = renderResult.EncodeToPNG();
            var fileName = Regex.Replace(prefabName, pattern, replacement);
            string fileNamePath = Path.Combine(saveDirectory, $"{fileName}.png");
            return new PictureConvertResult(fileNamePath, imageBytes);
        }
    }

    public struct PictureConvertResult
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