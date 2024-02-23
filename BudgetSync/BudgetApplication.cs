using BudgetSync.YnabApi.Account;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using MyBudget;

internal class BudgetApplication(IBudgetQueryService budgetQueryService) : IBudgetApplication
{
    private readonly IBudgetQueryService _budgetQueryService = budgetQueryService;

    public async Task RunAsync(IConfigurationRoot config)
    {
        var budgetGuidString = string.IsNullOrWhiteSpace(config["Ynab:BudgetId"]) ? "" : config["Ynab:BudgetId"];
        if(!Guid.TryParse(budgetGuidString, out var budgetId))
        {
            Console.WriteLine("Invalid YNAB budget Guid");
            return;
        }

        var budgets = await _budgetQueryService.GetBudgets();
        var selectedBudget = budgets.FirstOrDefault(b => b.Id == budgetId);

        if (selectedBudget == null)
        {
            Console.WriteLine($"Budget not found!");
        }

        var accounts = await _budgetQueryService.GetBudgetAccounts(selectedBudget);

        var assets = 0m;
        var debts = 0m;

        foreach (var account in accounts)
        {
            var balance = account.Balance;

            if (balance > 0)
            {
                assets += balance;
            }
            else
            {
                debts += balance;
            }
        }

        assets /= 1000;
        debts /= 1000;
        var netWorth = assets + debts;

        string[] Scopes = { SheetsService.Scope.Spreadsheets };
        string ApplicationName = config["GoogleSheets:ApplicationName"];
        string SpreadsheetId = config["GoogleSheets:SpreadsheetId"];
        string sheet = config["GoogleSheets:SheetName"];

        var googleCredentials = await GoogleCredential.FromFileAsync(
            "./svc.json",
            CancellationToken.None
        );

        var client = new SheetsService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = googleCredentials,
                ApplicationName = ApplicationName,
            }
        );

        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var dateValue = $"{currentDate.Month}/1/{currentDate.Year}";

        var rowNumberToWriteTo = ((currentDate.Year - 2023) * 12) + currentDate.Month + 67;

        var updateRange = $"{sheet}!A{rowNumberToWriteTo}:C{rowNumberToWriteTo}";

        var updateGetQuery = client.Spreadsheets.Values.Get(SpreadsheetId, updateRange);
        var updateRow = await updateGetQuery.ExecuteAsync();

        var updateRowValue = new List<IList<object>>
        {
            new List<object> { dateValue, assets, debts }
        };

        updateRow.Values = updateRowValue;
        var updateRequest = client.Spreadsheets.Values.Update(
            updateRow,
            SpreadsheetId,
            updateRange
        );

        var userEntered = SpreadsheetsResource
            .ValuesResource
            .UpdateRequest
            .ValueInputOptionEnum
            .USERENTERED;
        updateRequest.ValueInputOption = userEntered;
        try
        {
            updateRequest.Execute();
            Console.WriteLine($"wrote to {updateRange}, values: {dateValue}|{assets}|{debts}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unable to write to sheet");
        }
    }
}
