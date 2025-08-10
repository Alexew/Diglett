using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Data;

namespace Diglett.Web.Controllers
{
    public partial class CatalogHelper
    {
        private readonly DiglettDbContext _db;
        private readonly ICatalogSearchQueryFactory _catalogSearchQueryFactory;

        public CatalogHelper(DiglettDbContext db, ICatalogSearchQueryFactory catalogSearchQueryFactory)
        {
            _db = db;
            _catalogSearchQueryFactory = catalogSearchQueryFactory;
        }
    }
}
