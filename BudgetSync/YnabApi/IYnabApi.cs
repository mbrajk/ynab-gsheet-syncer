using BudgetSync.YnabApi;
using BudgetSync.YnabApi.Account;
using BudgetSync.YnabApi.Budget;
using BudgetSync.YnabApi.Category;
using Refit;

namespace MyBudget.Api
{
    internal interface IYnabApi
    {
        [Get("/budgets")]
        internal Task<QueryResponse<BudgetResponse>> GetBudgetsAsync();

        [Get("/budgets/{id}/categories")]
        internal Task<QueryResponse<CategoryResponse>> GetBudgetCategoriesAsync(Guid id);

        [Get("/budgets/{id}/accounts")]
        internal Task<QueryResponse<AccountResponse>> GetBudgetAccountsAsync(Guid id);
    }
}
