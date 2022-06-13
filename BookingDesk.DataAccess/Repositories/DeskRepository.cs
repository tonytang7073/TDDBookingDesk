using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Datainterface;
using BookingDesk.Domain;

namespace BookingDesk.DataAccess.Repositories
{
    public class DeskRepository : IDeskRepo
    {
        private readonly BookingDeskDbContext _context;

        public DeskRepository(BookingDeskDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Desk> GetAllDesks()
        {
            return _context.Desk.ToList();
        }

        public IEnumerable<Desk> GetDesksByDate(DateTime date)
        {
            var bookedDesks = _context.DeskBook.Where(x => x.Date == date)
                .Select(b => b.DeskId)
                .ToList();

            return _context.Desk.Where(x => !bookedDesks.Contains(x.Id)).ToList();
        }
    }
}
