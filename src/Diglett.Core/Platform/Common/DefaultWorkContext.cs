using Diglett.Core.Domain.Identity;
using Diglett.Core.Identity;

namespace Diglett.Core
{
    public class DefaultWorkContext : IWorkContext
    {
        private readonly User? _user;
        private readonly IUserService _userService;

        public DefaultWorkContext(IUserService userService)
        {
            _userService = userService;
        }

        public User? CurrentUser => _user ?? _userService.GetAuthenticatedUserAsync().Await();
    }
}
