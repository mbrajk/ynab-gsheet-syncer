using Microsoft.Extensions.Configuration;

namespace MyBudget
{
    internal interface IBudgetApplication
    {
        Task RunAsync(IConfigurationRoot config);
    }
}
