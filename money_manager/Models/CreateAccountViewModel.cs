using Microsoft.AspNetCore.Mvc.Rendering;
using money_manager.Entities;

namespace money_manager.Models;

public class CreateAccountViewModel : AccountEntity
{
    public IEnumerable<SelectListItem> AccountTypes { get; set; } = [];
}
