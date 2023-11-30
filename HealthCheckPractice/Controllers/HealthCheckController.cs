using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }



        //do same as we add service in application middleware . using to make custom healthCheck
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HealthReport report =await _healthCheckService.CheckHealthAsync();
            return Ok(report.Status.ToString());

        }

    }
}
