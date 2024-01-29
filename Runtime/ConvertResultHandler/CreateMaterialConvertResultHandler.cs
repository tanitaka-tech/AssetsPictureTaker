using System;
using System.Collections.Generic;
using System.IO;
using TanitakaTech.AssetsPictureTaker.PictureConverter;
using TanitakaTech.AssetsPictureTaker.StringConverter;
using UnityEditor;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.ConvertResultHandler
{
    [Serializable]
    public class CreateMaterialConvertResultHandler : IConvertResultHandler
    {
        [SerializeField] private Shader shader;
        [SerializeReference, SubclassSelector] private IStringConverter fileNameConverter = new IdentityStringConverter();

        public CreateMaterialConvertResultHandler(Shader shader, IStringConverter fileNameConverter)
        {
            this.shader = shader;
            this.fileNameConverter = fileNameConverter;
        }
        
        void IConvertResultHandler.HandleConvertResult(IReadOnlyList<PictureConvertResult> pictureConvertResults)
        {
            foreach (var pictureConvertResult in pictureConvertResults)
            {
                var material = new Material(shader)
                {
                    mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(pictureConvertResult.FileNamePath)
                };

                var filePath = FilePath(pictureConvertResult: pictureConvertResult);

                AssetDatabase.CreateAsset(material, filePath);
            }
            
            AssetDatabase.Refresh();
        }

        private string FilePath(PictureConvertResult pictureConvertResult)
        {
            var oldFileName = Path.GetFileName(pictureConvertResult.FileNamePath);
            var newFileName = fileNameConverter.ConvertString(oldFileName);
            string filePath = pictureConvertResult.FileNamePath.Replace(
                oldFileName,
                newFileName);
            return filePath;
        }
    }
}