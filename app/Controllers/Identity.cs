
namespace sunstealer.mvc.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("~/Identity")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    // [Microsoft.AspNetCore.Authorization.Authorize(Policy = "Admin")] // ajm: => 403
    [Microsoft.Identity.Web.Resource.RequiredScope("swagger")]    // ajm: => 401
    public class Identity : Microsoft.AspNetCore.Mvc.Controller
    {
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Get()
        {
            return new Microsoft.AspNetCore.Mvc.JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
