using Diglett.Core.Domain.Identity;

namespace Diglett.Core.Identity
{
    public interface IUserService
    {
        Task<User?> GetAuthenticatedUserAsync();
    }
}
