using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;

namespace CycleCycleCycle.App_Start
{
    public class CycleContextFactory : IDbContextFactory
    {
        public DbContext CreateContext()
        {
            return new CycleContext();
        }
    }
}