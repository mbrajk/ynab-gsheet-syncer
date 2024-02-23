using System.Text.Json.Serialization;

namespace BudgetSync.YnabApi.Account
{
    internal class Account
    {
        public Guid Id { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; init; }
    }
}
