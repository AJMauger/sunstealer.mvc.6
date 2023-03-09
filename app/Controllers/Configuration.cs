using Microsoft.Extensions.Logging;

namespace sunstealer.mvc.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("~/Configuration")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    // [Microsoft.AspNetCore.Authorization.Authorize(Policy = "Admin")] // ajm: => 403
    [Microsoft.Identity.Web.Resource.RequiredScope("swagger")]    // ajm: => 401
    public class Configuration : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly Microsoft.Extensions.Logging.ILogger<Configuration> _logger;
        private Services.IApplication applicationService;
        private System.Net.Http.HttpClient hhtpClient;

        public Configuration(
            Services.IApplication applicationService,
            System.Net.Http.HttpClient httpClient,
            Microsoft.Extensions.Logging.ILogger<Configuration> logger) {

            this.applicationService = applicationService;
            this.hhtpClient = httpClient;
            this._logger = logger;
            this._logger.LogInformation("Configuration.Configuration()");
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Microsoft.AspNetCore.Mvc.JsonResult))]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Microsoft.AspNetCore.Mvc.Route("~/Configuration")]
        public Microsoft.AspNetCore.Mvc.ActionResult Get() {
            try
            {
                this._logger.LogInformation("Configuration.Get()");
                Models.Configuration configuration = this.applicationService.Configuration;
                return Json(configuration);
            } catch(System.Exception e) {
                _logger.LogError(e, "Configuration.Get()");
            }
            return new Microsoft.AspNetCore.Mvc.BadRequestResult(); 
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Microsoft.AspNetCore.Mvc.JsonResult))]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(StatusCodes.Status403Forbidden)]
        // [Authorize(Policy = )]
        [Microsoft.AspNetCore.Mvc.Route("~/Configuration")]
        public Microsoft.AspNetCore.Mvc.ActionResult Post([Microsoft.AspNetCore.Mvc.FromBody] Models.Configuration configuration)
        {
            try {
                return Json(applicationService.Reconfigure(configuration));
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Configuration.Post()");
            }
            return new Microsoft.AspNetCore.Mvc.BadRequestResult();
        }
    }
}