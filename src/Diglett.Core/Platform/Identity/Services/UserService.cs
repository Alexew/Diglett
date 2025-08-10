using Diglett.Core.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Diglett.Core.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private User? _authUser;
        private bool _authUserResolved;

        public UserService(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User?> GetAuthenticatedUserAsync()
        {
            if (!_authUserResolved)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    return null;

                var principal = await EnsureAuthentication(httpContext);

                if (principal?.Identity?.IsAuthenticated == true)
                {
                    _authUser = await _userManager.GetUserAsync(principal);
                }

                _authUserResolved = true;
            }

            return _authUser;
        }

        private static async Task<ClaimsPrincipal> EnsureAuthentication(HttpContext context)
        {
            var authenticateResult = (context.Features.Get<IAuthenticateResultFeature>()?.AuthenticateResult)
                ?? await context.AuthenticateAsync();

            if (authenticateResult.Succeeded)
                return authenticateResult.Principal ?? context.User;

            return context.User;
        }
    }
}
