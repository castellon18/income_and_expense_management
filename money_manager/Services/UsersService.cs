using System.Security.Claims;
using System.Security.Principal;

namespace money_manager.Services;

public interface IUsersService
{
    int GetUserId();
}

public class UsersService(IHttpContextAccessor httpContextAccessor) : IUsersService
{
    private readonly HttpContext httpContext = httpContextAccessor.HttpContext ?? throw new ApplicationException("No HttpContext available.");

    public int GetUserId()
    {
        ClaimsPrincipal httpContextUser = httpContext.User ?? throw new ApplicationException("No user available in HttpContext.");

        IIdentity? identity = httpContextUser.Identity;

        if (identity != null && identity.IsAuthenticated)
        {
            // Intenta obtener la claim estándar NameIdentifier, si no existe prueba "sub".
            string? idClaimValue = httpContextUser.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                   ?? httpContextUser.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(idClaimValue))
                throw new ApplicationException("User id claim not found.");

            if (int.TryParse(idClaimValue, out int userId))
                return userId;

            throw new ApplicationException("User id claim is not a valid integer.");
        }

        throw new ApplicationException("User is not authenticated.");
    }
}
