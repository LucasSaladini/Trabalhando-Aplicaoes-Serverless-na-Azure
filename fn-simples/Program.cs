using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder().ConfigureFunctionWebAppLocation().ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerServices();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();