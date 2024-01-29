using System;
using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.StringConverter
{
    [Serializable]
    public class RegexStringConverter : IStringConverter
    {
        [SerializeField] private string pattern = @"P_GameObject_(\d+)";
        [SerializeField] private string replacement = "P_GameObjectPicture__$1";
        
        string IStringConverter.ConvertString(string stringToConvert)
        {
            return System.Text.RegularExpressions.Regex.Replace(stringToConvert, pattern, replacement);
        }
    }
}