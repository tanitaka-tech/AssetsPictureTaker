using UnityEngine;

namespace TanitakaTech.AssetsPictureTaker.Texture2DProcessor
{
    public interface ITexture2DProcessor
    {
        Texture2D Process(Texture2D texture2D); 
    }
}