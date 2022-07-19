using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Validation;

public class TicketNumberValidation:ValidationAttribute
{
    private static string GetErrorMessage() => "invalid ticket number!";
        
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var ticketNumber = value.ToString();

        if (ticketNumber.Length!=13)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
    
}