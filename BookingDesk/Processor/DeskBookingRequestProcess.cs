using BookingDesk.Datainterface;
using BookingDesk.Domain;

namespace BookingDesk.Processor
{
    public class DeskBookingRequestProcess : IDeskBookingRequestProcess
    {
        private IDeskBookRepo _deskBookRepo;
        private IDeskRepo _deskRepo;

        public DeskBookingRequestProcess(IDeskBookRepo repo, IDeskRepo deskRepo)
        {
            this._deskBookRepo = repo;
            this._deskRepo = deskRepo;
        }



        public BookingResult BookDesk(BookingRequest request)
        {
            if (request == null) { throw new ArgumentNullException("request"); }

            var desks = _deskRepo.GetDesksByDate(request.Date);
            var ret = Create<BookingResult>(request);
            if (desks.Any())
            {
                var deskbook = Create<DeskBook>(request);
                deskbook.DeskId = desks.First().Id;
                deskbook.Id = Guid.NewGuid();
                _deskBookRepo.Save(deskbook);
                ret.BookingId = deskbook.Id;
                ret.ResultCode = BookingResultCode.Success;

            }
            else
            {
                ret.ResultCode = BookingResultCode.NoDeskAvailable;
            }



            return ret;

        }

        private static T Create<T>(BookingRequest request) where T : BookingBase, new()
        {

            return new T()
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Date = request.Date,
                Email = request.Email
            };
        }

    }
}