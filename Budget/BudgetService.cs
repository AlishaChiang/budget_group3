using System.Globalization;

namespace Budget;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        var budgets = _budgetRepo.GetAll();
        
        decimal totalBudget = 0m;
        for (var s = start; s <= end; s = s.AddDays(1))
        {
            totalBudget += budgets
                .First(x => x.YearMonthDateTime.Month == s.Month 
                            && x.YearMonthDateTime.Year == s.Year).DailyAmount;
        }

        return totalBudget;
    }
}

public interface IBudgetRepo
{
    List<BudgetModel> GetAll();
}

public class BudgetModel
{
    public BudgetModel()
    {
    }

    public BudgetModel(string yearMonth, int amount)
    {
        YearMonth = yearMonth;
        Amount = amount;
    }

    public string YearMonth { get; set; }
    public int Amount { get; set; }
    public decimal DailyAmount => ConvertToDailyAmount();

    private decimal ConvertToDailyAmount()
    {
        var year = Convert.ToInt32(YearMonth.Substring(0, 4));
        var month = Convert.ToInt32(YearMonth.Substring(4, 2));

        return (decimal)Amount / DateTime.DaysInMonth(year, month);
    }

    public DateTime YearMonthDateTime => ConvertToDateTime(YearMonth);

    private DateTime ConvertToDateTime(string yearMonth)
    {
        return DateTime.ParseExact(yearMonth, "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None);
    }
}