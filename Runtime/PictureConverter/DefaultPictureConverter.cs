using System;
using System.IO;
using TanitakaTech.AssetsPictureTaker.PictureEncoder;
using TanitakaTech.AssetsPictureTaker.StringConverter;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.PictureConverter
{
    [Serializable]
    public class DefaultPictureConverter : IPictureConverter
    {
        [SerializeReference, SubclassSelector] private IPictureEncoder pictureEncoder = new PngPictureEncoder();
        [SerializeReference, SubclassSelector] private IStringConverter fileNameConverter = new IdentityStringConverter();
        
        public PictureConvertResult ConvertPicture(string saveDirectory, Texture2D renderResult, string prefabName)
        {
            var encodedPicture = pictureEncoder.EncodePicture(renderResult);
            var fileName = fileNameConverter.ConvertString(prefabName);
            string fileNamePath = Path.Combine(saveDirectory, $"{fileName}.{encodedPicture.Extension}");
            return new PictureConvertResult(fileNamePath, encodedPicture.PictureBytes);
        }
    }
}