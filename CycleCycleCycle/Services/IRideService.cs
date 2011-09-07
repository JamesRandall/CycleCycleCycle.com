using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CycleCycleCycle.Services
{
    public interface IRideService
    {
        bool Create(int accountId, int routeId, DateTime dateRidden, int? hours, int? minutes, int? seconds);
    }
}
