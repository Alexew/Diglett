using Diglett.Core;
using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Content.Media;
using Diglett.Core.Data;

namespace Diglett.Web.Controllers
{
    public partial class CatalogHelper
    {
        private readonly DiglettDbContext _db;
        private readonly IWorkContext _workContext;
        private readonly IMediaService _mediaService;
        private readonly ICatalogSearchQueryFactory _catalogSearchQueryFactory;

        public CatalogHelper(
            DiglettDbContext db,
            IWorkContext workContext,
            IMediaService mediaService,
            ICatalogSearchQueryFactory catalogSearchQueryFactory)
        {
            _db = db;
            _workContext = workContext;
            _mediaService = mediaService;
            _catalogSearchQueryFactory = catalogSearchQueryFactory;
        }
    }
}
