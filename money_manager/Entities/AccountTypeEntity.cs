using Microsoft.AspNetCore.Mvc;
using money_manager.Validations;
using System.ComponentModel.DataAnnotations;

namespace money_manager.Entities;

public class AccountTypeEntity
{
    public int AccountTypeId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
    [FirstCapitalLetter(ErrorMessage = "Estimado usuario, la primera letra debe ser mayúscula")]
    [Display(Name = "Nombre")]
    [Remote(action: "VerifyAccountTypeExists", controller: "AccountTypes", AdditionalFields = nameof(UserId) + "," + nameof(AccountTypeId))]
    public string AccountTypeName { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int OrderBy { get; set; }
}
