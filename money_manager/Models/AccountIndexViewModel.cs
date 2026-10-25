using money_manager.Entities;

namespace money_manager.Models;

public class AccountIndexViewModel
{
    public string AccountType { get; set; } = string.Empty;
    public IEnumerable<AccountEntity> Accounts { get; set; } = [];
    public decimal TotalBalance => Accounts.Sum(a => a.AccountBalance);
}
