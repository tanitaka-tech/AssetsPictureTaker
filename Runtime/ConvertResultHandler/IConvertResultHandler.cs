using System.Collections.Generic;
using TanitakaTech.AssetsPictureTaker.PictureConverter;

namespace TanitakaTech.AssetsPictureTaker.ConvertResultHandler
{
    public interface IConvertResultHandler
    {
        void HandleConvertResult(IReadOnlyList<PictureConvertResult> pictureConvertResults);
    }
}