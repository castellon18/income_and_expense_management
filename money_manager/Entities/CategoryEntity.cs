using System.ComponentModel.DataAnnotations;

namespace money_manager.Entities;

public class CategoryEntity
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 2)]
    [Display(Name = "Categoría")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Tipo de operación")]
    public OperationTypeEntity OperationTypeId { get; set; }

    public string OperationTypeName { get; set; } = string.Empty;

    public int UserId { get; set; }
}
