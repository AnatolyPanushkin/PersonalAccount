using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Validation;

public class DocNumberValidation:ValidationAttribute
{
    private static string GetErrorMessage() => "Invalid document data!";
        
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var docNumber = (string)value;

        if (docNumber.Length<6)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }
}