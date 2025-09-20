using Diglett.Web.Modelling;
using System.ComponentModel.DataAnnotations;

namespace Diglett.Web.Models.Collection
{
    public class BinderModel : EntityModelBase
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 50, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int PageCount { get; set; } = 1;

        [Required]
        [Range(4, 16, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int PocketSize { get; set; } = 9;

        public BinderPageModel Page { get; set; } = new();
    }
}
