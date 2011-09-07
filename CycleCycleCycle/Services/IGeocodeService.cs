using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CycleCycleCycle.Services.Utilities;

namespace CycleCycleCycle.Services
{
    public interface IGeocodeService
    {
        Location Geocode(string searchString);
    }
}
