namespace money_manager.ParametersClass;

public class GetTransactionsByUserParameter
{
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
