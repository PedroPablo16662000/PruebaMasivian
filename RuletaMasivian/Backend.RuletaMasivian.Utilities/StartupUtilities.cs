namespace Backend.RuletaMasivian
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Startup Utilities
    /// </summary>
    public static class StartupUtilities
    {
        /// <summary>
        /// Adds the utilities.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        public static void AddUtilities(this IServiceCollection services, string instrumentationKey)
        {
            services.AddApplicationInsightsTelemetry(instrumentationKey);

            services.AddSingleton<Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer, Utilities.Telemetry.TelemetryInitializer>();

            services.AddTransient<Utilities.Telemetry.ITelemetryException, Utilities.Telemetry.TelemetryException>();
            services.AddTransient<Utilities.SendMail.ISendMailService, Utilities.SendMail.SendMailService>();
        }
    }
}