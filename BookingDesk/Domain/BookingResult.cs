using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingDesk.Domain
{
    public class BookingResult : BookingBase
    {
      public BookingResultCode ResultCode { get; set; }
        public Guid BookingId { get; set; }
    }
}
