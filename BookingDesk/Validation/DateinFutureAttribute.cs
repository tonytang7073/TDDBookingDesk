using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingDesk.Validation
{
    public class DateinFutureAttribute : ValidationAttribute
    {
        public DateinFutureAttribute()
        {
            ErrorMessage = "Date must be in the future";
        }

        public override bool IsValid(object value)
        {
           bool isValid = false;
            if (value is DateTime dateTime)
            {
                isValid = dateTime > DateTime.Now;
            }
            return isValid;
        }
    }
}
