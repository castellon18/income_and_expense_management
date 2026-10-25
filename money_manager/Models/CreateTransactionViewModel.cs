using Microsoft.AspNetCore.Mvc.Rendering;
using money_manager.Entities;

namespace money_manager.Models;

public class CreateTransactionViewModel : TransactionEntity
{
    public IEnumerable<SelectListItem> Accounts { get; set; } = [];

    public IEnumerable<SelectListItem> Categories { get; set; } = [];
}
