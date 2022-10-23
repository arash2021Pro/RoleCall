using System.Security.Claims;

namespace LicenseProject.StartupModuleServices.AuthenticationService;

public static class AuthenticatedUser
{
    public static int GetCurrentUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = 0;
        try
        {
            userId = int.Parse(claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
        catch (Exception e)
        {
            // ignored
        }

        return userId;
    }
}