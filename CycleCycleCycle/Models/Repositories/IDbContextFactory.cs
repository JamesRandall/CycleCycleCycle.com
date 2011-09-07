using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CycleCycleCycle.Models.Repositories
{
    public interface IDbContextFactory
    {
        DbContext CreateContext();
    }
}
