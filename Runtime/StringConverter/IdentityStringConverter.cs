using System;

namespace TanitakaTech.AssetsPictureTaker.StringConverter
{
    [Serializable]
    public class IdentityStringConverter : IStringConverter
    {
        string IStringConverter.ConvertString(string stringToConvert) => stringToConvert;
    }
}