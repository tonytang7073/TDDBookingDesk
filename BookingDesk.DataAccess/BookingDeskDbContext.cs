using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookingDesk.DataAccess
{
    public class BookingDeskDbContext : DbContext
    {
        public BookingDeskDbContext(DbContextOptions<BookingDeskDbContext> options): base(options)
        {

        }

        public DbSet<Desk> Desk { get; set; }
        public DbSet<DeskBook> DeskBook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);

            modelBuilder.Entity<DeskBook>()
                .HasOne(d => d.Desk);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Desk>().HasData(
                new Desk { Id=new Guid("C0843157-B797-4C8D-B063-B55E53822F02"), Description="Desk #1"},
                new Desk { Id = new Guid("3B1CCD31-C286-45FD-AD73-5B2229916606"), Description = "Desk #2" },
                new Desk { Id = new Guid("7BC3C939-FE90-47FB-A410-DAA8B3A95A8B"), Description = "Desk #11" }
                );
        }
    }


}
