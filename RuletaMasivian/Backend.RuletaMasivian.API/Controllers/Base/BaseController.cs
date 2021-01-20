namespace Backend.RuletaMasivian.API.Controllers.Base
{
    using Backend.RuletaMasivian.Utilities;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// Base Controller
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets the get email.
        /// </summary>
        /// <value>
        /// The get email.
        /// </value>
        internal string GetEmail => HttpContext.User?.Claims?.Where(w => w.Type.Equals(ClaimTypes.UserData))?.FirstOrDefault()?.Value?.Deserialize<dynamic>()?.User;

        /// <summary>
        /// Gets the get email.
        /// </summary>
        /// <value>
        /// The get email.
        /// </value>
        internal string GetUserId => HttpContext.User?.Claims?.Where(w => w.Type.Equals(ClaimTypes.UserData))?.FirstOrDefault()?.Value?.Deserialize<dynamic>()?.UserId;

        /// <summary>
        /// Gets the get roles.
        /// </summary>
        /// <value>
        /// The get roles.
        /// </value>
        internal System.Collections.Generic.List<string> GetRolesIds()
        {
            return HttpContext.User?.Claims?.Where(w => w.Type.Equals(ClaimTypes.Role))?.Select(s => s.Value)?.ToList();
        }
    }
}
