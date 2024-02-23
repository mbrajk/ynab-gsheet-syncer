using BudgetSync.YnabApi.Category;
using MyBudget.Api;

namespace BudgetSync.YnabApi.Account
{
    internal class BudgetQueryService(IYnabApi _ynabApi) : IBudgetQueryService
    {
        public async Task<IReadOnlyCollection<CategoryGroup>> GetBudgetCategories(
            Budget.Budget selectedBudget
        )
        {
            var response = await _ynabApi.GetBudgetCategoriesAsync(selectedBudget.Id);

            return response.Data.Groups;
        }

        public async Task<IReadOnlyCollection<Account>> GetBudgetAccounts(Budget.Budget selectedBudget)
        {
            var response = await _ynabApi.GetBudgetAccountsAsync(selectedBudget.Id);

            return response.Data.Accounts;
        }

        public async Task<IReadOnlyCollection<Budget.Budget>> GetBudgets()
        {
            var response = await _ynabApi.GetBudgetsAsync();

            return response.Data.Budgets;
        }
    }
}
