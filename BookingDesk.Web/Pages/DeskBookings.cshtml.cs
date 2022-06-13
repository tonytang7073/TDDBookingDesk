using BookingDesk.Datainterface;
using BookingDesk.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingDesk.Web.Pages
{
    public class DeskBookingsModel : PageModel
    {

        private readonly IDeskBookRepo _deskBookingRepository;

        public DeskBookingsModel(IDeskBookRepo deskBookingRepository)
        {
            _deskBookingRepository = deskBookingRepository;
        }

        public IEnumerable<DeskBook> DeskBookings { get; set; }

        public void OnGet()
        {
            DeskBookings = _deskBookingRepository.GetAll();
        }

    }
}
