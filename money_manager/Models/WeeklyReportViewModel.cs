using money_manager.Entities;

namespace money_manager.Models;

public class WeeklyReportViewModel
{
    public decimal Income => TransactionsByWeek.Sum(t => t.Income);
    public decimal Bills => TransactionsByWeek.Sum(t => t.Bills);
    public decimal Total => Income - Bills;
    public DateTime ReferenceDate { get; set; }
    public IEnumerable<ExpectedResultsPerWeekEntity> TransactionsByWeek { get; set; } = [];
}
