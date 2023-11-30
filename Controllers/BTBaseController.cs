using Guardian_BugTracker_23.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Guardian_BugTracker_23.Controllers
{
    [Controller]
    public abstract class BTBaseController : Controller
    {
        protected string? _userId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        protected int _companyId => User.Identity!.GetCompanyId();
    }
}
