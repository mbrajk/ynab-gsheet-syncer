using System.Text.Json.Serialization;

namespace BudgetSync.YnabApi.Category
{
    internal class CategoryResponse
    {
        [JsonPropertyName("category_groups")]
        public IReadOnlyCollection<CategoryGroup>? Groups { get; init; }
    }
}
