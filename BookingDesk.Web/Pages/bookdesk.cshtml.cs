using BookingDesk.Domain;
using BookingDesk.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingDesk.Web.Pages
{
    public class BookDeskModel : PageModel
    {
        private readonly IDeskBookingRequestProcess _deskBookRequestProcess;

        public BookDeskModel(IDeskBookingRequestProcess deskBookRequestProcess)
        {
            this._deskBookRequestProcess = deskBookRequestProcess;
        }

        [BindProperty]
        public BookingRequest BookingRequest { get; set; }

        public IActionResult OnPost()
        {
            IActionResult ret = Page();

            if (ModelState.IsValid)
            {
                var result = _deskBookRequestProcess.BookDesk(BookingRequest);
                if (result.ResultCode == BookingResultCode.Success)
                {
                    ret = RedirectToPage("BookDeskConfirmation", new 
                    {
                        result.BookingId,
                        result.Firstname,
                        result.Date
                    });
                }
                else if(result.ResultCode == BookingResultCode.NoDeskAvailable)
                {
                    ModelState.AddModelError("BookingRequest.Date", "No desk available for selected date");
                }
            }
            return ret;
        }

    }
}
