namespace Diglett.Core.Catalog.Search.Modelling
{
    public interface ICatalogSearchQueryFactory
    {
        CatalogSearchQuery? Current { get; }
        CatalogSearchQuery? CreateFromQuery();
    }
}
