namespace money_manager.ParametersClass;

public class GetTransactionsByAccountParameter
{
    public int UserId { get; set; }
    public int AccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
