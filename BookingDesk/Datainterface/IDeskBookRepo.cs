using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Domain;

namespace BookingDesk.Datainterface
{
    public interface IDeskBookRepo
    {
        public int Save(DeskBook book); 

        public IEnumerable<DeskBook> GetAll();
    }
}
