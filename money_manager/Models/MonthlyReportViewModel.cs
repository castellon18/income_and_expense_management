using money_manager.Entities;

namespace money_manager.Models;

public class MonthlyReportViewModel
{
    public IEnumerable<ExpectedResultsPerMonthEntity> TransactionsByMonth { get; set; } = [];
    public decimal Income => TransactionsByMonth.Sum(t => t.Income);
    public decimal Bills => TransactionsByMonth.Sum(t => t.Bills);
    public decimal Total => Income - Bills;
    public int Year { get; set; }
}
