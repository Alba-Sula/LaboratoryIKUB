using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace EventsUI.CustomValidator
{
    public class FileTypeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file != null)
            {
                var fileName = file.FileName;
                var extention = Path.GetExtension(fileName);
                if (extention == ".jpeg" || extention == ".gif" || extention == ".png" || extention == ".jpg")
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(this.ErrorMessage);
                }

            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}