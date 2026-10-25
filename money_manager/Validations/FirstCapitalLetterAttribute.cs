using System.ComponentModel.DataAnnotations;

namespace money_manager.Validations;

public class FirstCapitalLetterAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        string? str = value as string;

        if (!string.IsNullOrEmpty(str) && !char.IsUpper(str[0]))
        {
            string message = string.IsNullOrEmpty(ErrorMessage) ? "La primera letra debe ser mayúscula." : ErrorMessage;
            return new ValidationResult(message);
        }

        return ValidationResult.Success;
    }
}
