using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Validation;

namespace BookingDesk.Domain
{
    public class BookingBase
    {
        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [DateinFuture]
        [DateWithoutTime]
        public DateTime Date { get; set; }
    }
}
