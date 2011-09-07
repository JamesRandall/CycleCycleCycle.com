using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CycleCycleCycle.Models
{
    public class CycleInitializer : DropCreateDatabaseIfModelChanges<CycleContext>
    {
        protected override void Seed(CycleContext context)
        {
            
            base.Seed(context);
        }
    }
}