namespace money_manager.Entities;

public class ExpectedResultsPerWeekEntity
{
    public int Weeks { get; set; }
    public decimal Amount { get; set; }
    public OperationTypeEntity OperationTypeId { get; set; }
    public decimal Income { get; set; }
    public decimal Bills { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
