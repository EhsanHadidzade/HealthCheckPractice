using HealthCheckPractice.AppHealthCheck;
using HealthCheckPractice.EF;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.


var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecksUI(op =>
{
    op.AddHealthCheckEndpoint("HealthCheck API", "/HealthCheck");
}).AddInMemoryStorage();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(configuration.GetSection("connectionString").Value).EnableSensitiveDataLogging());



builder.Services.AddHealthChecks()
    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck))
    .AddDbContextCheck<AppDBContext>();


builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//just check the application if its up and running
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] =StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] =StatusCodes.Status503ServiceUnavailable
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthcheck", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(op => op.UIPath = "/dashboard");

app.Run();
