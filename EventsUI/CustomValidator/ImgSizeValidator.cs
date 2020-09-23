using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsUI.CustomValidator
{
    public class ImgSizeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            if (file != null)
            {
                var size = file.ContentLength;
                if (size > 200000)
                {
                    return new ValidationResult(this.ErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}