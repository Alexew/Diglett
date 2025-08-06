namespace Diglett.Core.Search
{
    public interface ISearchFilter
    {
        string FieldName { get; }
    }

    public interface IAttributeSearchFilter : ISearchFilter
    {
        object? Term { get; }
        IndexTypeCode TypeCode { get; }
    }
}
