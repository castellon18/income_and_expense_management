using System.ComponentModel.DataAnnotations;

namespace money_manager.Entities;

public class TransactionEntity
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha Transacción")]
    public DateTime TransactionDate { get; set; } = DateTime.Now;

    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
    [Display(Name = "Monto")]
    public decimal Amount { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Por favor, selecciona una cuenta válida..")]
    [Display(Name = "Cuenta")]
    public int AccountId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Por favor, selecciona una categoría válida..")]
    [Display(Name = "Categoría")]
    public int CategoryId { get; set; }

    [StringLength(maximumLength: 1000, ErrorMessage = "La nota de la transacción es demasiado larga.")]
    [Display(Name = "Nota de Transacción")]
    public string TransactionNote { get; set; } = string.Empty;

    [Display(Name = "Tipo de Operación")]
    public OperationTypeEntity OperationTypeId { get; set; } = OperationTypeEntity.Ingreso;

    public string AccountName { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;
}
