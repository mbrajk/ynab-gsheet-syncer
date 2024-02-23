using System.Text.Json.Serialization;

namespace BudgetSync.YnabApi.Budget
{
    internal class BudgetResponse
    {
        [JsonPropertyName("budgets")]
        public IReadOnlyCollection<Budget> Budgets { get; init; }
    }
}
