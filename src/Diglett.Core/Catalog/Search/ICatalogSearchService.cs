namespace Diglett.Core.Catalog.Search
{
    public interface ICatalogSearchService
    {
        Task<CatalogSearchResult> SearchAsync(CatalogSearchQuery searchQuery);
    }
}
