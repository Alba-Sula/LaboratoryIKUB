using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace EventsUI.CustomValidator
{
    public class HeightWidthImgValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file != null)
            {
                var fileName = file.FileName;
                if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".gif"))
                {
                    var img = Image.FromStream(file.InputStream, true, true);
                    var height = img.Height;
                    var width = img.Width;
                    if (height <= 400 && height >= 100 && width <= 400 && width >= 100)
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