using Diglett.Core.Collections;

namespace Diglett.Web.Models.Catalog
{
    public interface IListActions
    {
        IPageable PagedList { get; }
    }
}
