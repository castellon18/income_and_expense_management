namespace money_manager.Entities;

public class ExpectedResultsPerMonthEntity
{
    public int Months { get; set; }
    public DateTime ReferenceDate { get; set; }
    public decimal Amount { get; set; }
    public OperationTypeEntity OperationTypeId { get; set; }
    public decimal Income { get; set; }
    public decimal Bills { get; set; }
}
