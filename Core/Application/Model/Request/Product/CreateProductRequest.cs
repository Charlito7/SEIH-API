namespace Core.Application.Model.Request;

using System;
using System.ComponentModel.DataAnnotations;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    public string Description { get; set; }

    [RegularExpression("^[0-9]*$", ErrorMessage = "BarCode can only contain numbers")]

    public string BarCode { get; set; }

    public string CategoryName { get; set; }

    public bool IsReturnAccepted { get; set; }
    public int ReturnTimeAccepted { get; set; }

    [Required(ErrorMessage = "ExpiredDate is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [CustomValidation(typeof(CreateProductRequest), "ValidateExpiredDate")]
    public DateTime ExpiredDate { get; set; }

    public static ValidationResult ValidateExpiredDate(DateTime expiredDate, ValidationContext context)
    {
        if (expiredDate < DateTime.Now)
        {
            return new ValidationResult("ExpiredDate cannot be a date in the past");
        }

        return ValidationResult.Success;

    }
}
