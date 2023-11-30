using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection.PortableExecutable;

namespace HealthCheckPractice.AppHealthCheck
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        public ApiHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var req = await _httpClient.GetAsync("https://localhost:44376/api/Product");

            if (req.IsSuccessStatusCode)
                return new HealthCheckResult(HealthStatus.Healthy, "Api Is Up");

            return new HealthCheckResult(HealthStatus.Unhealthy, "Api Is Down");
        }
    }
}
