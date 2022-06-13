using BookingDesk.Domain;

namespace BookingDesk.Datainterface
{
    public interface IDeskRepo
    {
        public IEnumerable<Desk> GetDesksByDate(DateTime date);
        public IEnumerable<Desk> GetAllDesks();

    }
}