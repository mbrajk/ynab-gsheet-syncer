using System.Text.Json.Serialization;

namespace BudgetSync.YnabApi
{
    internal class QueryResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; init; }
    }
}
