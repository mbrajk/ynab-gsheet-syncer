using MyBudget.Api;

namespace BudgetSync.YnabApi.Account
{
    internal class AccountQueryService(IYnabApi _ynabApi)
    {
        internal async Task<IReadOnlyCollection<Account>> GetBudgetAccounts(Budget.Budget selectedBudget)
        {
            var response = await _ynabApi.GetBudgetAccountsAsync(selectedBudget.Id);

            return response.Data.Accounts;
        }
    }
}
