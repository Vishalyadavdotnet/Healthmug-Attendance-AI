using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AttendanceApi.Configuration
{
    public static class OpenTelemetrySetup
    {
        public static void AddObservability(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "AttendanceApi";

            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddOtlpExporter();
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddProcessInstrumentation()
                           .AddRuntimeInstrumentation()
                           .AddOtlpExporter();
                });
        }
    }
}
