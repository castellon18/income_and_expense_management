using money_manager.Validations;
using System.ComponentModel.DataAnnotations;

namespace money_manager.Entities;

public class AccountEntity
{
    public int AccountId { get; set; }

    [Required(ErrorMessage = "El Campo {0} es Requerido")]
    [StringLength(50, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 2)]
    [FirstCapitalLetter]
    [Display(Name = "Nombre")]
    public string AccountName { get; set; } = string.Empty;

    [Display(Name = "Tipo de Cuenta")]
    public int AccountTypeId { get; set; }

    public string AccountTypeName { get; set; } = string.Empty;

    [Display(Name = "Saldo")]
    public decimal AccountBalance { get; set; }

    [StringLength(100, ErrorMessage = "El Campo {0} debe tener máximo {1} caracteres")]
    [Display(Name = "Descripción")]
    public string AccountDescription { get; set; } = string.Empty;
}
