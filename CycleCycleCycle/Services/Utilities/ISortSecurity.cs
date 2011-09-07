namespace CycleCycleCycle.Services.Utilities
{
    public interface ISortSecurity
    {
        void ValidateSortOrder(string sortOrder);
        void ValidateSortIndex(string sortIndex, string[] allowedValues);
    }
}