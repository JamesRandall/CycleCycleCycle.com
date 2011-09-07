using System.Linq;
using System.Security;

namespace CycleCycleCycle.Services.Utilities
{
    public class SortSecurity : ISortSecurity
    {
        public void ValidateSortOrder(string sortOrder)
        {
            string loweredSortOrder = sortOrder.ToLower();
            if (loweredSortOrder != "asc" && loweredSortOrder != "desc") throw new SecurityException("Invalid sort order passed to JSON grid provider");
        }

        public void ValidateSortIndex(string sortIndex, string[] allowedValues)
        {
            if (!allowedValues.Contains(sortIndex)) throw new SecurityException("Invalid sort index passed to JSON grid provider");
        }
    }
}