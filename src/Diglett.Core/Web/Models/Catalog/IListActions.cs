using Diglett.Core.Collections;

namespace Diglett.Core.Web.Models.Catalog
{
    public interface IListActions
    {
        IPageable PagedList { get; }
    }
}
