namespace money_manager.Models;

public class EditTransactionViewModel : CreateTransactionViewModel
{
    public int PreviousAccountId { get; set; }
    public decimal PreviousAmount { get; set; }
    public string ReturnUrl { get; set; } = string.Empty;
}
