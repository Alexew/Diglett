using Diglett.Web.Modelling;

namespace Diglett.Web.Models.Collection
{
    public class BinderPageModel : ModelBase
    {
        public int Page { get; set; } = 1;
        public int PocketSize { get; set; } = 9;
        public Dictionary<int, BinderItemModel> Items { get; set; } = [];
    }
}
