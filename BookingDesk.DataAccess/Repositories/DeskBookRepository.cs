using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Datainterface;
using BookingDesk.Domain;

namespace BookingDesk.DataAccess.Repositories
{
    public class DeskBookRepository : IDeskBookRepo
    {
        private readonly BookingDeskDbContext _context;

        public DeskBookRepository(BookingDeskDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<DeskBook> GetAll()
        {
            return _context.DeskBook.ToList();
        }

        public int Save(DeskBook book)
        {
            _context.DeskBook.Add(book);
            return _context.SaveChanges();
        }
    }
}
