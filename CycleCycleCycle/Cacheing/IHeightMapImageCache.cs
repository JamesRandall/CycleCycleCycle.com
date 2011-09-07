using System.Drawing;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Cacheing
{
    public interface IHeightMapImageCache
    {
        Bitmap HeightMapImage(Route route, int width, int height);
        void CacheHeightMapImage(Bitmap bitmap, Route route, int width, int height);
    }
}
