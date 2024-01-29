using System;
using System.Collections.Generic;
using TanitakaTech.AssetsPictureTaker.PictureConverter;

namespace TanitakaTech.AssetsPictureTaker.ConvertResultHandler
{
    [Serializable]
    public class NothingConvertResultHandler : IConvertResultHandler
    {
        void IConvertResultHandler.HandleConvertResult(IReadOnlyList<PictureConvertResult> pictureConvertResults)
        {
            // Do nothing
            return;
        }
    }
}