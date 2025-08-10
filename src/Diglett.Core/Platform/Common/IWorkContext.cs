using Diglett.Core.Domain.Identity;

namespace Diglett.Core
{
    public interface IWorkContext
    {
        User? CurrentUser { get; }
    }
}
