using BudgetSync.YnabApi.Category;

namespace BudgetSync.YnabApi.Account
{
    internal interface IBudgetQueryService
    {
        Task<IReadOnlyCollection<Budget.Budget>> GetBudgets();
        Task<IReadOnlyCollection<CategoryGroup>> GetBudgetCategories(Budget.Budget selectedBudget);
        Task<IReadOnlyCollection<Account>> GetBudgetAccounts(Budget.Budget selectedBudget);
    }
}
