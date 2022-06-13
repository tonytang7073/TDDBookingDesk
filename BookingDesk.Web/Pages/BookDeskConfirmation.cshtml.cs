using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingDesk.Web.Pages
{
    public class BookDeskConfirmationModel : PageModel
    {
        public int DeskBookingId { get; set; }

        public string Firstname { get; set; }

        public DateTime Date { get; set; }

        public void OnGet(int deskBookingId, string firstName, DateTime date)
        {
            DeskBookingId = deskBookingId;
            Firstname = firstName;
            Date = date;
        }
    }
}
