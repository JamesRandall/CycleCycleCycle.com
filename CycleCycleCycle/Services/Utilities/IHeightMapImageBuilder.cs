using System.Drawing;
using CycleCycleCycle.Models;

namespace CycleCycleCycle.Services.Utilities
{
    public interface IHeightMapImageBuilder
    {
        Bitmap HeightMapImage(Route route, int width, int height);
    }
}
