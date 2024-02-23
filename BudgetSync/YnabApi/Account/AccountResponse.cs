using System.Text.Json.Serialization;

namespace BudgetSync.YnabApi.Account
{
    internal class AccountResponse
    {
        [JsonPropertyName("accounts")]
        public IReadOnlyCollection<Account> Accounts { get; set; }
    }
}
