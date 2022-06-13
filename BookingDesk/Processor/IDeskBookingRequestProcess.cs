using BookingDesk.Domain;

namespace BookingDesk.Processor
{
    public interface IDeskBookingRequestProcess
    {
        BookingResult BookDesk(BookingRequest request);
    }
}