namespace DomainDriven
{
    /// <summary>
    /// Used in the Repository to convert strings containing field names and directions
    /// into .OrderBy() or .OrderByDescending() lambda expressions for Linq
    /// </summary>
    public interface ISortField
    {
        string SortField { get; }
        string SortDirection { get; }
    }
}