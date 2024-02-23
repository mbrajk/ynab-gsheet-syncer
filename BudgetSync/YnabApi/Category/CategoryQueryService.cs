using BudgetSync.YnabApi.Category;
using MyBudget.Api;

namespace BudgetSync.YnabApi.Account
{
    internal class CategoryQueryService(IYnabApi _ynabApi)
    {
        public async Task<IReadOnlyCollection<CategoryGroup>?> GetBudgetCategoriesAsync(Budget.Budget budget)
        {
            var response = await _ynabApi.GetBudgetCategoriesAsync(budget.Id);
            
            return response?.Data?.Groups;
        }
    }
}
