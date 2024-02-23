using System.Text.Json.Serialization;

namespace BudgetSync.YnabApi.Budget
{
    internal class Budget
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        public Guid Id { get; init; }
    }
}
