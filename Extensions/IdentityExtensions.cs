using System.Security.Claims;
using System.Security.Principal;

namespace Guardian_BugTracker_23.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetCompanyId(this IIdentity identity)
        {
            Claim claim = ((ClaimsIdentity)identity).FindFirst("CompanyId")!;
            return int.Parse(claim.Value);
        }
    }
}
