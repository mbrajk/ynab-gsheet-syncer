using BudgetSync.YnabApi.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBudget;
using MyBudget.Api;
using Polly;
using Polly.Extensions.Http;
using Refit;

var config = BuildConfiguration();
var services = BuildServiceProvider(config);

var budgetApplication = services.GetRequiredService<IBudgetApplication>();
await budgetApplication.RunAsync(config);

IConfigurationRoot BuildConfiguration()
{
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    return config;
}

ServiceProvider BuildServiceProvider(IConfigurationRoot config)
{
    var services = new ServiceCollection();

    // logging
    services.AddLogging(
        logging =>
        {
            if (Environment.UserInteractive)
            {
                logging
                    .AddConsole()
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
            }
        }
    );

    // refit http client
    services
        .AddRefitClient<IYnabApi>()
        .ConfigureHttpClient(
            httpClient =>
            {
                var endpoint = config["YnabApi:Endpoint"];
                var version = config["YnabApi:Version"];
                var token = config["YnabApi:Token"];

                httpClient.BaseAddress = new Uri($"{endpoint}/{version}");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        )
        .AddPolicyHandler(
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(4, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)))
        );

    // other required dependencies
    services.AddSingleton<IBudgetApplication, BudgetApplication>();
    services.AddSingleton<IBudgetQueryService, BudgetQueryService>();

    return services.BuildServiceProvider();
}
