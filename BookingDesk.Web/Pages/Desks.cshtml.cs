using BookingDesk.Datainterface;
using BookingDesk.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingDesk.Web.Pages
{
    public class DesksModel : PageModel
    {
        private readonly IDeskRepo _deskRepository;

        public DesksModel(IDeskRepo deskRepository)
        {
            _deskRepository = deskRepository;
        }

        public IEnumerable<Desk> Desks { get; set; }

        public void OnGet()
        {
            Desks = _deskRepository.GetAllDesks();
        }
    }
}
