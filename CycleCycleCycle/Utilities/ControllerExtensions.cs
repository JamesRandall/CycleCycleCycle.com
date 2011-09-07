using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace CycleCycleCycle.Utilities
{
    public static class ControllerExtensions
    {
        public static void ValidateSortOrder(this Controller controller, string sortOrder)
        {
            string loweredSortOrder = sortOrder.ToLower();
            if (loweredSortOrder != "asc" && loweredSortOrder != "desc") throw new SecurityException("Invalid sort order passed to JSON grid provider");
        }

        public static void ValidateSortIndex(this Controller controller, string sortIndex, string[] allowedValues)
        {
            if (!allowedValues.Contains(sortIndex)) throw new SecurityException("Invalid sort index passed to JSON grid provider");
        }
    }
}